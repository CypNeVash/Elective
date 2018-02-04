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
                .Include(s => s.MyFacultatives.Select(i => i.Lecturer))
                .Include(s => s.MyFacultatives.Select(i => i.Audience));
        }

        public override Teacher Get(Guid id)
        {
            return _electiveContext.Teachers.Where(s => s.Id == id)
                .Include(s => s.MyFacultatives.Select(i => i.Lecturer))
                .Include(s => s.MyFacultatives.Select(i => i.Audience)).Single();
        }

        public override void Remove(Teacher data)
        {
            _electiveContext.Teachers.Where(s => s.Id == data.Id)
                .Include(s => s.MyFacultatives)
                .Include(s => s.MessageSend)
                .Include(s => s.MessagesReceive)
                .Include(s=>s.MyFacultatives.Select(i=>i.Log))
                .Include(s => s.MyFacultatives.Select(i => i.Log.Reports)).ToList();

            foreach (var item in data.MyFacultatives.ToList())
            {
                if (item.Log != null)
                {
                    foreach (var i in item.Log.Reports.ToList())
                        _electiveContext.Reports.Remove(i);

                    _electiveContext.ReportBooks.Remove(item.Log);
                }

                _electiveContext.Facultatives.Remove(item);
            }

            foreach (var item in data.MessageSend.ToList())
                _electiveContext.Messages.Remove(item);

            foreach (var item in data.MessagesReceive.ToList())
                _electiveContext.Messages.Remove(item);

            base.Remove(data);
        }
    }
}
