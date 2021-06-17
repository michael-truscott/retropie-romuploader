using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RetroPieRomUploader.Models;

namespace RetroPieRomUploader.Data
{
    public class RetroPieRomUploaderContext : DbContext
    {
        public RetroPieRomUploaderContext (DbContextOptions<RetroPieRomUploaderContext> options)
            : base(options)
        {
        }

        public DbSet<Rom> Rom { get; set; }
        public DbSet<ConsoleType> ConsoleType { get; set; }
        public DbSet<RomFileEntry> RomFileEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RomFileEntry>().ToTable("RomFileEntry");
        }
    }
}
