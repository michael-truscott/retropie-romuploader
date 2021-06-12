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

        public DbSet<RetroPieRomUploader.Models.Rom> Rom { get; set; }
        public DbSet<RetroPieRomUploader.Models.ConsoleType> ConsoleType { get; set; }
    }
}
