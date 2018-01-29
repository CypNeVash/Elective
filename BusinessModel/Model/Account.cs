using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel
{
    /// <summary>
    /// Base entity which contain 
    /// information about person 
    /// </summary>
    public class Account : BaseEntity
    {
        public Account()
        {
        }

        public Account(User identity, string firstName, string secondName, int age, DateTime birthDate)
        {
            Identity = identity;
            FirstName = firstName;
            SecondName = secondName;
            Age = age;
            BirthDate = birthDate;
        }

        public User Identity { get; set; }

        public string FirstName { get; set; }
        public string SecondName { get; set; }

        public string FullName { get { return FirstName + " " + SecondName; } }

        public int Age { get; set; }
        public DateTime BirthDate { get; set; } = DateTime.Now;
    }
}
