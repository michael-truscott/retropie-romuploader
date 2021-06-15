using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RetroPieRomUploader.Data;
using RetroPieRomUploader.Models;
using RetroPieRomUploader.ViewModels;

namespace RetroPieRomUploader.Pages.Roms
{
    public class EditModel : PageModel
    {
        private readonly RetroPieRomUploader.Data.RetroPieRomUploaderContext _context;
        private readonly IRomFileManager _romFileManager;
        private readonly ILogger<EditModel> _logger;

        public EditModel(RetroPieRomUploader.Data.RetroPieRomUploaderContext context, IRomFileManager romFileManager, ILogger<EditModel> logger)
        {
            _context = context;
            _romFileManager = romFileManager;
            _logger = logger;
        }

        [BindProperty]
        public EditRomVM RomVM { get; set; }
        public SelectList ConsoleList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rom = await _context.Rom
                .Include(e => e.FileEntries)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (rom == null)
            {
                return NotFound();
            }
            RomVM = EditRomVM.FromRom(rom);
            return await InitPage();
        }

        private async Task<IActionResult> InitPage()
        {
            ConsoleList = new SelectList(await _context.ConsoleType.OrderBy(c => c.Name).ToListAsync(), nameof(ConsoleType.ID), nameof(ConsoleType.Name));
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return await InitPage();
            }

            var rom = await _context.Rom
                .Include(e => e.FileEntries)
                .FirstOrDefaultAsync(e => e.ID == id);
            if (rom == null)
            {
                return NotFound();
            }
            
            var oldConsoleType = rom.ConsoleTypeID;

            var entry = _context.Attach(rom);
            entry.CurrentValues.SetValues(RomVM);

            try
            {
                await MoveRomFileIfRequired(oldConsoleType, rom);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error moving rom files");
                ModelState.AddModelError("RomVM.ConsoleTypeID", ex.Message);
                RomVM = EditRomVM.FromRom(rom);
                return await InitPage();
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private async Task MoveRomFileIfRequired(string oldConsoleType, Rom updatedRom)
        {
            if (oldConsoleType == updatedRom.ConsoleTypeID)
                return;

            var existingFile = updatedRom.FileEntries.FirstOrDefault(e => _romFileManager.RomFileExists(updatedRom.ConsoleTypeID, e.Filename));
            if (existingFile != null)
            {
                var owningRom = await _context.Rom.Where(rom => rom.ConsoleTypeID == updatedRom.ConsoleTypeID &&
                                    rom.FileEntries.Any(fe => fe.Filename == existingFile.Filename))
                                    .FirstOrDefaultAsync();
                throw new ArgumentException($"A file named {existingFile.Filename} already exists in the {updatedRom.ConsoleTypeID} console folder (Associated with the rom \"{owningRom?.Title ?? "(unknown)"}\")");
            }

            foreach (var entry in updatedRom.FileEntries)
                _romFileManager.MoveFileToConsoleDir(oldConsoleType, updatedRom.ConsoleTypeID, entry.Filename);
        }
    }
}
