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

        public RomDetailsVM Rom { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rom = await _context.Rom
                .Include(r => r.ConsoleType)
                .Include(r => r.FileEntries)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (rom == null)
            {
                return NotFound();
            }
            Rom = new RomDetailsVM(rom);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rom = await _context.Rom
                .Include(e => e.FileEntries)
                .FirstOrDefaultAsync(e => e.ID == id);

            if (rom != null)
            {
                _context.Rom.Remove(rom);
                await _context.SaveChangesAsync();
                DeleteRomFile(rom);
            }

            return RedirectToPage("./Index");
        }

        private void DeleteRomFile(Rom rom)
        {
            var console = rom.ConsoleTypeID;
            foreach (var entry in rom.FileEntries)
                _romFileManager.DeleteRomFile(console, entry.Filename);
        }
    }
}
