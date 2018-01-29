using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BusinessModel
{
    /// <summary>
    ///     Can be used to query from a database and group together
    ///     changes that will then be written back to the store as a unit.
    ///</summary>
    public class ElectiveContext : IdentityDbContext<User>
    {
        public ElectiveContext()
            : base("name=ElectiveContext") => Database.SetInitializer(new ElectiveContextInitializer());

        virtual public DbSet<Facultative> Facultatives { get; set; }
        virtual public DbSet<Teacher> Teachers { get; set; }
        virtual public DbSet<Student> Students { get; set; }
        virtual public DbSet<ReportBook> ReportBooks { get; set; }
        virtual public DbSet<Report> Reports { get; set; }
        virtual public DbSet<Log> Logs { get; set; }
        virtual public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ReportBook>().HasOptional(x => x.Elective).WithOptionalDependent(x => x.Log);
        }

        public virtual void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
            SaveChanges();
        }
    }

}