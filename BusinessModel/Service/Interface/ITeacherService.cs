using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel
{
    /// <summary>
    /// Set of methods for obtaining and creating
    /// teacher electives and statement and for their management
    /// </summary>
    public interface ITeacherService
    {
        void AddFacultative(Facultative facultative, Teacher teacher);

        Facultative GetFacultative(Facultative facultative);

        void DeleteFacultative(Facultative facultative);

        ReportBook CreateReportBook(string name, Facultative facultative);

        void AddReport(ReportBook reportBook, Report report);

        Teacher GetTeacher(Account account);

        void ChangeStateEntity<U>(U data) where U : class;
    }
}
