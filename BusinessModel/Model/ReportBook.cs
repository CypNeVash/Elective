using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel
{
    /// <summary>
    /// Entity that contains information about 
    /// all students and about evaluations by facultative
    /// </summary>
    public class ReportBook : BaseEntity
    {
        public string Name { get; set; }
        public Facultative Elective { get; set; }
        public ICollection<Report> Reports { get; set; } = new List<Report>();

        public ReportBook()
        {
        }

        public ReportBook(string name, Facultative elective)
        {
            Name = name;
            Elective = elective;
        }

    }
}
