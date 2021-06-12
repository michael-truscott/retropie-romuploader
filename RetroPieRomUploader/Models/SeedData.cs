using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RetroPieRomUploader.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetroPieRomUploader.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var service = serviceProvider.GetRequiredService<DbContextOptions<RetroPieRomUploaderContext>>();
            using (var context = new RetroPieRomUploaderContext(service))
            {
                if (context.ConsoleType.Any())
                    return;

                context.ConsoleType.AddRange(ConsoleTypes);
                context.SaveChanges();
            }
        }

        private static List<ConsoleType> ConsoleTypes => new List<ConsoleType>
        {
            new ConsoleType
            {
                Name = "Playstation",
                DirectoryName = "psx"
            },
            new ConsoleType
            {
                Name = "Nintendo 64",
                DirectoryName = "n64"
            },
            new ConsoleType
            {
                Name = "NES",
                DirectoryName = "nes"
            },
            new ConsoleType
            {
                Name = "SNES",
                DirectoryName = "snes"
            },
        };
    }
}
