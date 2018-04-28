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
    /// entity Messages in the database 
    /// </summary>
    public class MessageRepository : DefaultRepository<Message>
    {
        public MessageRepository(ElectiveContext electiveContext) : base(electiveContext)
        {
        }

        public override IEnumerable<Message> Get()
        {
            return _electiveContext.Messages.Include(s => s.From).Include(s => s.To).ToList();
        }

        public override Message Get(Guid id)
        {
            return _electiveContext.Messages.Where(s => s.Id == id).Include(s => s.From).Include(s => s.To).Single();
        }
    }
}
