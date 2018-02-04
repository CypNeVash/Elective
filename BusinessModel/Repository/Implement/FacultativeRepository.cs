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
    /// entity Facultative in the database 
    /// </summary>
    public class FacultativeRepository : DefaultRepository<Facultative>
    {
        public FacultativeRepository(ElectiveContext electiveContext) : base(electiveContext)
        {
        }

        public override IQueryable<Facultative> Get()
        {
           return  _electiveContext.Facultatives.Include(s => s.Lecturer)
                .Include(s => s.Audience)
                .Include(s => s.Log)
                .Include(s => s.Log.Reports.Select(rep => rep.Listener))
                .Include(s => s.Log.Elective);
        }

        public override Facultative Get(Guid id)
        {
            return _electiveContext.Facultatives.Where(s=>s.Id == id).Include(s => s.Audience)
                .Include(s => s.Lecturer)
                .Include(s => s.Log)
                .Include(s => s.Log.Reports)
                .Include(s => s.Log.Elective).Single();
        }

        public override void Remove(Facultative data)
        {
            if (data.Log != null)
            {
                foreach (var i in data.Log.Reports.ToList())
                    _electiveContext.Reports.Remove(i);

                _electiveContext.ReportBooks.Remove(data.Log);
            }

            base.Remove(data);
        }
    }
}
