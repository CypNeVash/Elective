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
    /// entity Account in the database 
    /// </summary>
    public class AccountRepository : DefaultRepository<Account>
    {
        public AccountRepository(ElectiveContext electiveContext) : base(electiveContext)
        {
        }

        public override IQueryable<Account> Get()
        {
            return _electiveContext.Accounts.Include(s => s.Identity);
        }

        public override Account Get(Guid id)
        {
            return _electiveContext.Accounts.Where(s => s.Id == id).Include(s => s.Identity).FirstOrDefault();
        }
    }
}
