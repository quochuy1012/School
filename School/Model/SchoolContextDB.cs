using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace School.Model
{
    public partial class SchoolContextDB : DbContext
    {
        public SchoolContextDB()
            : base("name=SchoolContextDB")
        {
        }

        public virtual DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
