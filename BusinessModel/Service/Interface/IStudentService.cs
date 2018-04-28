using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessModel
{
    /// <summary>
    /// Set of methods for obtaining 
    /// student electives and for their management
    /// </summary>
    public interface IStudentService
    {
        IEnumerable<Facultative> GetAllFacultative();

        bool RegToFacultative(Student student, Guid id);

        Facultative GetFacultative(Facultative facultative);

        Student GetStudent(Account account);
    }
}
