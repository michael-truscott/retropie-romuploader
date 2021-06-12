using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RetroPieRomUploader.Data;
using RetroPieRomUploader.Models;

namespace RetroPieRomUploader.Pages.Roms
{
    public class IndexModel : PageModel
    {
        private readonly RetroPieRomUploader.Data.RetroPieRomUploaderContext _context;

        public IndexModel(RetroPieRomUploader.Data.RetroPieRomUploaderContext context)
        {
            _context = context;
        }

        public IList<Rom> Rom { get;set; }

        public async Task OnGetAsync()
        {
            Rom = await _context.Rom.Include(rom => rom.ConsoleType).ToListAsync();

            foreach (var rom in Rom)
            {
                var ctype = rom.ConsoleType;
                Console.WriteLine(ctype?.Name);
            }
        }
    }
}
