using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using UserWebAPIService.Models;

namespace UserWebAPIService
{
    public class UserContext : DbContext
    {
        public UserContext()
            : base("UserConnectionString")
        {
        }

        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();
            modelBuilder.Entity<User>().ToTable("User");
        }
    }
}