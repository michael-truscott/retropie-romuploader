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
                var existingConsoles = new HashSet<string>(context.ConsoleType.Select(c => c.ID));
                var missingConsoles = ConsoleTypes.Where(c => !existingConsoles.Contains(c.ID));
                context.ConsoleType.AddRange(missingConsoles);
                context.SaveChanges();
            }
        }

        private static List<ConsoleType> ConsoleTypes => new List<ConsoleType>
        {
            new ConsoleType { ID = "atari2600",     Name = "Atari 2600" },
            new ConsoleType { ID = "gb",            Name = "Game Boy" },
            new ConsoleType { ID = "gbc",           Name = "Game Boy Color" },
            new ConsoleType { ID = "gba",           Name = "Game Boy Advance" },
            new ConsoleType { ID = "mastersystem",  Name = "Sega Master System" },
            new ConsoleType { ID = "megadrive",     Name = "Sega Megadrive" },
            new ConsoleType { ID = "neogeo",        Name = "Neo Geo" },
            new ConsoleType { ID = "sega32x",       Name = "Sega 32X" },
            new ConsoleType { ID = "n64",           Name = "Nintendo 64" },
            new ConsoleType { ID = "nes",           Name = "NES" },
            new ConsoleType { ID = "psx",           Name = "Playstation" },
            new ConsoleType { ID = "snes",          Name = "SNES" },
        };
    }
}
