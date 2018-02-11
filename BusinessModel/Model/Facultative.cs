using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel
{
    public enum FacultativeStatus { Started, Finished, Registration, NotAvailable }

    /// <summary>
    /// Entity that contains information 
    /// about facultative
    /// </summary>
    public class Facultative : BaseEntity
    {
        public string Name{ get; set; }
        public string Theme { get; set; }
        public string Description { get; set; }

        public FacultativeStatus Status { get; set; }
        [Required]
        public Teacher Lecturer { get; set; }
        public ICollection<Student> Audience { get; set; } = new List<Student>();
        public DateTime StartFacultative { get; set; }
        public int Duration { get; set; }
        public ReportBook Log { get; set; }

        public Facultative()
        {
        }

        public Facultative(string name, string theme, string description, FacultativeStatus status, Teacher lecturer, DateTime startFacultative, int duration)
        {
            Name = name;
            Theme = theme;
            Description = description;
            Status = status;
            Lecturer = lecturer;
            StartFacultative = startFacultative;
            Duration = duration;
        }
    }
}
