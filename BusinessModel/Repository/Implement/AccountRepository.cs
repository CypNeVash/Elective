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

        public override IEnumerable<Account> Get()
        {
            return _electiveContext.Accounts.Include(s => s.Identity).ToList();
        }

        public override Account Get(Guid id)
        {
            return _electiveContext.Accounts.Where(s => s.Id == id).Include(s => s.Identity).Single();
        }

        public override void Remove(Account data)
        {
            foreach (var item in data.MessageSend.ToList())
                _electiveContext.Messages.Remove(item);

            foreach (var item in data.MessagesReceive.ToList())
                _electiveContext.Messages.Remove(item);

            base.Remove(data);
        }
    }
}
