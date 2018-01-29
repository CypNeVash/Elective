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
    /// entity Teacher in the database 
    /// </summary>
    public class TeacherRepository : DefaultRepository<Teacher>
    {
        public TeacherRepository(ElectiveContext electiveContext) : base(electiveContext)
        {
        }

        public override IQueryable<Teacher> Get()
        {
            return _electiveContext.Teachers
                .Include(s=>s.MyFacultatives.Select(i=>i.Lecturer))
                .Include(s => s.MyFacultatives.Select(i=>i.Audience));
        }

        public override Teacher Get(Guid id)
        {
            return _electiveContext.Teachers.Where(s=>s.Id == id)
                .Include(s => s.MyFacultatives.Select(i => i.Lecturer))
                .Include(s => s.MyFacultatives.Select(i => i.Audience)).FirstOrDefault();
        }
    }
}
