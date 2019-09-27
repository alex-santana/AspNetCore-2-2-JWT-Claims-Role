using AspNetCore_2_2_JWT.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore_2_2_JWT.Data
{
    public class CourseDbContext : DbContext
    {
        public CourseDbContext(DbContextOptions<CourseDbContext> options):base(options)
        {

        }

        public DbSet<CourseViewModel> Courses { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CourseViewModel>()
                .HasKey(c => c.Id);
                
        }
    }
}
