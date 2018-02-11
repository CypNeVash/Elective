using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessModel
{
    /// <summary>
    /// Base entity which contain 
    /// information about person 
    /// </summary>
    public class Account : BaseEntity
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string FullName { get { return FirstName + " " + SecondName; } }
        public int Age { get; set; }

        public User Identity { get; set; }
        public DateTime BirthDate { get; set; } = DateTime.Now;
        public ICollection<Message> MessagesReceive { get; set; } = new List<Message>();
        public ICollection<Message> MessageSend { get; set; } = new List<Message>();

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
    }
}
