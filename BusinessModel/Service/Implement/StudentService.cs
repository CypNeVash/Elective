using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel
{
    /// <summary>
    /// Class for saving entities 
    /// in database and obtaining 
    /// student electives and 
    /// for their management
    /// </summary>
    public class StudentService : IStudentService
    {
        private IDefaultRepository<Facultative> _facultativeRepository;
        private IDefaultRepository<Student> _studentRepository;
        private IDefaultRepository<Log> _logRepository;

        public StudentService(IDefaultRepository<Facultative> facultativeRepository
            , IDefaultRepository<Student> studentRepository
            , IDefaultRepository<Log> logRepository
            , IDefaultRepository<Account> accountRepository)
        {
            _facultativeRepository = facultativeRepository;
            _studentRepository = studentRepository;
            _logRepository = logRepository;
        }

        public IEnumerable<Facultative> GetAllFacultative()
        {
            return _facultativeRepository.Get();
        }

        public bool RegToFacultative(Student student, Guid id)
        {

            _logRepository.Add(new Log(GetType().ToString(), "Reg", LogStatus.info, "RegToFacultative", student.FullName));

            if (student.RegistrFacultatives.Where(s => s.Id == id).Count() > 0)
                return false;
            else
            {
                student.RegistrFacultatives.Add(_facultativeRepository.Get(id));
                _studentRepository.Save();
                return true;
            }
        }

        public Facultative GetFacultative(Facultative facultative)
        {
            return _facultativeRepository.Get(facultative.Id);
        }

        public Student GetStudent(Account account)
        {
            return _studentRepository.Get(account.Id);
        }
    }
}
