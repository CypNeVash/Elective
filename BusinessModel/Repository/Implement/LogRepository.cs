using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel
{
    /// <summary>
    /// Repository for manipulating with 
    /// entity Log in the database 
    /// </summary>
    public class LogRepository : DefaultRepository<Log>
    {
        public LogRepository(ElectiveContext electiveContext) : base(electiveContext)
        {
        }

        public override IQueryable<Log> Get()
        {
            return _electiveContext.Logs;
        }

        public override Log Get(Guid id)
        {
            return _electiveContext.Logs.Where(s=>s.Id == id).FirstOrDefault();
        }
    }
}
