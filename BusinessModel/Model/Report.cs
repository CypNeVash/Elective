using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel
{
    /// <summary>
    /// Entity that contains a student's mark
    /// </summary>
    public class Report : BaseEntity
    {
        public Student Listener { get; set; }
        public int Mark { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        public Report()
        {
        }

        public Report(Student listener, int mark)
        {
            Listener = listener;
            Mark = mark;
        }
    }
}
