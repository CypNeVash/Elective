using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel
{
    public class MessageRepository : DefaultRepository<Message>
    {
        public MessageRepository(ElectiveContext electiveContext) : base(electiveContext)
        {
        }

        public override IQueryable<Message> Get()
        {
            return _electiveContext.Messages.Include(s => s.From).Include(s => s.To);
        }

        public override Message Get(Guid id)
        {
            return _electiveContext.Messages.Where(s => s.Id == id).Include(s => s.From).Include(s => s.To).Single();
        }
    }
}
