using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel
{
    /// <summary>
    /// Class for saving entities in database 
    /// and manages accounts
    /// </summary>
    public class AccountService : IAccountService
    {
        private IDefaultRepository<Account> _accountRepository;
        private IDefaultRepository<Student> _studentRepository;
        private IDefaultRepository<Log> _logRepository;
        private IDefaultRepository<Teacher> _teacherRepository;
        private IDefaultRepository<Message> _messageRepository;

        public AccountService(IDefaultRepository<Account> accountRepository, IDefaultRepository<Student> studentRepository, IDefaultRepository<Log> logRepository, IDefaultRepository<Teacher> teacherRepository, IDefaultRepository<Message> messageRepository)
        {
            _accountRepository = accountRepository;
            _studentRepository = studentRepository;
            _logRepository = logRepository;
            _teacherRepository = teacherRepository;
            _messageRepository = messageRepository;
        }

        public void SendMessage(string identityNameFrom, string identityNameTo, string theme, string message)
        {
            Message messenger = new Message(GetAccount(identityNameFrom)
                , GetAccount(identityNameTo)
                , MessageState.NotRead, theme, message, DateTime.Now);

            _messageRepository.Add(messenger);
        }

        public Account GetAllMessages(string identityName)
        {
            return _accountRepository.Get().Where(s => s.Identity.UserName == identityName)
                .Include(s => s.MessageSend)
                .Include(s => s.MessagesReceive)
                .Include(s => s.MessageSend.Select(i => i.To))
                .Include(s => s.MessagesReceive.Select(i => i.From)).FirstOrDefault();
        }

        public Account GetAccount(string identityName)
        {
            return _accountRepository.Get().Where(s => s.Identity.UserName == identityName).FirstOrDefault();
        }

        public void ChangeStateEntity<U>(U data) where U : class
        {
            _accountRepository.ChangeState(data);
        }

        public void CreateStudent(User user, string firstName, string secondName, int age, DateTime birthDate)
        {
            _studentRepository.Add(new Student(user, firstName, secondName, age, birthDate));

            _logRepository.Add(new Log(GetType().ToString(), "Add", LogStatus.info, "CreateStudent", user.Email));
        }

        public void DeleteStudent(User user)
        {
            Student student = _studentRepository.Get(GetAccount(user.UserName).Id);

            _studentRepository.Remove(student);

            _logRepository.Add(new Log(GetType().ToString(), "Delete", LogStatus.info, "DeleteStudent", user.Email));
        }


        public void DeleteTeacher(User user)
        {
            _logRepository.Add(new Log(GetType().ToString(), "Delete", LogStatus.info, "DeleteTeacher", user.Email));

            Teacher teacher = _teacherRepository.Get(GetAccount(user.UserName).Id);

            _teacherRepository.Remove(teacher);
        }

        public void CreateTeacher(User user, string firstName, string secondName, int age, DateTime birthDate)
        {
            _logRepository.Add(new Log(GetType().ToString(), "Add", LogStatus.info, "CreateTeacher", user.Email));

            _teacherRepository.Add(new Teacher(user, firstName, secondName, age, birthDate));
        }

        public int GetNotReadMessages(string identityName)
        {
            return GetAllMessages(identityName).MessagesReceive.Where(s => s.Status == MessageState.NotRead).Count();
        }

        public Message GetMessageReceive(string identityName, Guid id)
        {
            Message message = GetAllMessages(identityName).MessagesReceive.Where(s => s.Id == id).FirstOrDefault();

            if (message != null)
                message.Status = MessageState.Read;

            _accountRepository.ChangeState(message);

            return message;
        }

        public Message GetMessageSend(string identityName, Guid id)
        {
            return GetAllMessages(identityName).MessageSend.Where(s => s.Id == id).FirstOrDefault();
        }
    }
}
