using System;
using System.Data.Entity;
using System.Linq;

namespace BusinessModel
{
    /// <summary>
    /// Repository for manipulating with 
    /// entity Student in the database 
    /// </summary>
    public class StudentRepository : DefaultRepository<Student>
    {
        public StudentRepository(ElectiveContext electiveContext) : base(electiveContext)
        {
        }

        public override IQueryable<Student> Get()
        {
            return _electiveContext.Students
                .Include(s => s.RegistrFacultatives);
        }

        public override Student Get(Guid id)
        {
            return _electiveContext.Students.Where(s=>s.Id == id)
                .Include(s => s.RegistrFacultatives.Select(i=>i.Audience)).FirstOrDefault();
        }
    }
}
