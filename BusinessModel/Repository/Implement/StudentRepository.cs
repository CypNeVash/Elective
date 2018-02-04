﻿using System;
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
                .Include(s => s.RegistrFacultatives.Select(i=>i.Audience)).Single();
        }

        public override void Remove(Student data)
        {
            data = _electiveContext.Students.Where(s => s.Id == data.Id)
                .Include(s => s.RegistrFacultatives)
                .Include(s => s.MessageSend)
                .Include(s => s.MessagesReceive).FirstOrDefault();

            foreach (var item in data.MessageSend.ToList())
                _electiveContext.Messages.Remove(item);

            foreach (var item in data.MessagesReceive.ToList())
                _electiveContext.Messages.Remove(item);

            _electiveContext.Reports.RemoveRange( _electiveContext.Reports.Where(s => s.Listener.Id == data.Id));

            base.Remove(data);
        }
    }
}
