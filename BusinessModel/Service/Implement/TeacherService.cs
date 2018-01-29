using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel
{
    /// <summary>
    /// Class for saving entities in database 
    /// and obtaining, creating
    /// teacher electives and statement and for their management
    /// </summary>
    public class TeacherService:ITeacherService
    {
        private IDefaultRepository<Facultative> _facultativeRepository;
        private IDefaultRepository<Teacher> _teacherRepository;
        private IDefaultRepository<Report> _reportRepository;
        private IDefaultRepository<ReportBook> _reportBookRepository;
        private IDefaultRepository<Log> _logRepository;

        public TeacherService(IDefaultRepository<Facultative> facultativeRepository
            , IDefaultRepository<Teacher> teacherRepository
            , IDefaultRepository<Report> reportRepository
            , IDefaultRepository<ReportBook> reportBookRepository
            , IDefaultRepository<Log> logRepository)
        {
            _facultativeRepository = facultativeRepository;
            _teacherRepository = teacherRepository;
            _reportRepository = reportRepository;
            _reportBookRepository = reportBookRepository;
            _logRepository = logRepository;
        }

        public void AddFacultative(Facultative facultative, Teacher teacher)
        {
            _facultativeRepository.Add(facultative);

            _logRepository.Add(new Log(GetType().ToString(), "Add", LogStatus.info, "AddFacultative", facultative.Name));
        }

        public Facultative GetFacultative(Facultative facultative)
        {
            return _facultativeRepository.Get(facultative.Id);
        }

        public void DeleteFacultative(Facultative facultative)
        {
            _logRepository.Add(new Log(GetType().ToString(), "Delete", LogStatus.info, "DeleteFacultative", facultative.Name));
            _facultativeRepository.Remove(facultative);
        }

        public ReportBook CreateReportBook(string name, Facultative facultative)
        {
            if (facultative.Log != null) return facultative.Log;

            _logRepository.Add(new Log(GetType().ToString(), "Add", LogStatus.info, "CreateReportBook", facultative.Name));

            ReportBook reportBook = new ReportBook(name, facultative);

            _reportBookRepository.Add(reportBook);

            facultative.Log = reportBook;

            ChangeStateEntity(facultative);

            return reportBook;
        }

        public void AddReport(ReportBook reportBook, Report report)
        {
            _logRepository.Add(new Log(GetType().ToString(), "Add", LogStatus.info, "AddReport", report.Listener.FullName + " " + report.Mark));

            reportBook.Reports.Add(report);
        }

        public Teacher GetTeacher(Account account)
        {
           return _teacherRepository.Get(account.Id);
        }

        public void ChangeStateEntity<U>(U data) where U : class
        {
            _facultativeRepository.ChangeState(data);
        }
    }
}
