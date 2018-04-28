using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel
{
    /// <summary>
    /// Repository for manipulating with 
    /// entity ReportBook in the database 
    /// </summary>
    public class ReportBookRepository : DefaultRepository<ReportBook>
    {
        public ReportBookRepository(ElectiveContext electiveContext) : base(electiveContext)
        {
        }

        public override IEnumerable<ReportBook> Get()
        {
            return _electiveContext.ReportBooks.Include(s=>s.Elective)
                .Include(s=>s.Reports)
                .Include(s=>s.Elective.Audience)
                .Include(s =>s.Elective.Lecturer).ToList();
        }

        public override ReportBook Get(Guid id)
        { 
            return _electiveContext.ReportBooks.Where(s=>s.Id == id)
                .Include(s => s.Elective)
                .Include(s => s.Reports)
                .Include(s => s.Elective.Audience)
                .Include(s => s.Elective.Lecturer).Single();
        }
    }
}
