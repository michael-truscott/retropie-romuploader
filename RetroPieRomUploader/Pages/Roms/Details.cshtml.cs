using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RetroPieRomUploader.Data;
using RetroPieRomUploader.Models;
using RetroPieRomUploader.ViewModels;

namespace RetroPieRomUploader.Pages.Roms
{
    public class DetailsModel : PageModel
    {
        private readonly RetroPieRomUploader.Data.RetroPieRomUploaderContext _context;

        public DetailsModel(RetroPieRomUploader.Data.RetroPieRomUploaderContext context)
        {
            _context = context;
        }

        public RomVM Rom { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rom = await _context.Rom.Include(m => m.ConsoleType).FirstOrDefaultAsync(m => m.ID == id);
            if (rom == null)
            {
                return NotFound();
            }
            Rom = RomVM.FromRom(rom);
            return Page();
        }
    }
}
