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
    public class DeleteModel : PageModel
    {
        private readonly RetroPieRomUploader.Data.RetroPieRomUploaderContext _context;

        public DeleteModel(RetroPieRomUploader.Data.RetroPieRomUploaderContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RomVM Rom { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rom = await _context.Rom.Include(r => r.ConsoleType).FirstOrDefaultAsync(m => m.ID == id);

            if (rom == null)
            {
                return NotFound();
            }
            Rom = RomVM.FromRom(rom);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rom = await _context.Rom.FindAsync(id);

            if (rom != null)
            {
                _context.Rom.Remove(rom);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
