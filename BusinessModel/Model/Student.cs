using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel
{
    /// <summary>
    /// Entity student which contain information 
    /// about elective and himself
    /// </summary>
    public class Student : Account
    {

        public ICollection<Facultative> RegistrFacultatives { get; set; } = new List<Facultative>();

        public Student()
        {
        }

        public Student(User identity, string firstName, string secondName, int age, DateTime birthDate)
            : base(identity, firstName, secondName, age, birthDate)
        {
        }
    }
}
