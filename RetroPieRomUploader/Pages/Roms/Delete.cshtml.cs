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
        private readonly IRomFileManager _romFileManager;

        public DeleteModel(RetroPieRomUploader.Data.RetroPieRomUploaderContext context, IRomFileManager romFileManager)
        {
            _context = context;
            _romFileManager = romFileManager;
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
                await DeleteRomFile(rom);
            }

            return RedirectToPage("./Index");
        }

        private async Task DeleteRomFile(Rom rom)
        {
            var console = await _context.ConsoleType.FindAsync(rom.ConsoleTypeID);
            _romFileManager.DeleteRomFile(console.ID, rom.Filename);
        }
    }
}
