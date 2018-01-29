using System;
using System.Collections.Generic;

namespace BusinessModel
{
    /// <summary>
    /// Entity teacher which contain information 
    /// about elective and himself
    /// </summary>
    public class Teacher : Account
    {
        public ICollection<Facultative> MyFacultatives { get; set; } = new List<Facultative>();

        public Teacher()
        {
        }


        public Teacher(User identity, string firstName, string secondName, int age, DateTime birthDate) 
            : base(identity, firstName, secondName, age, birthDate)
        {
        }
    }
}
