using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Domain; 
namespace Repository
{
    public class FileNameTaggerDBContext : DbContext 
    {
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Studio> Tag { get; set; }

        public string DbPath { get;  }

        public FileNameTaggerDBContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "filenametagger.db");
        }

        // Configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.Entity<Actor>(entity =>
           {
               entity.HasKey(e => e.ActorId);
               entity.Property(e => e.Name); 

               entity.HasData(new Actor //seed data
               {
                   ActorId = 1,
                   Name = "Jennifer Lawrence"
               })
           })
        }
    }
}
