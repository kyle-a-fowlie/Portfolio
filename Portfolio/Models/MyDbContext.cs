using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Portfolio.Models
{
    public class MyDbContext : DbContext
    {
        
        // Database constructor
        public MyDbContext() : base("Portfolio")
        {

        }

        //public DbSet<Project> Projects { get; set;  }

    }
}