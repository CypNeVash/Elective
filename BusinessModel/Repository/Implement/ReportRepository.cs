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
    /// entity Report in the database 
    /// </summary>
    public class ReportRepository : DefaultRepository<Report>
    {
        public ReportRepository(ElectiveContext electiveContext) : base(electiveContext)
        {
        }

        public override IEnumerable<Report> Get()
        {
            return _electiveContext.Reports.Include(s=>s.Listener).ToList();
        }

        public override Report Get(Guid id)
        {
            return _electiveContext.Reports.Where(s=>s.Id == id).Include(s => s.Listener).Single();
        }
    }
}
