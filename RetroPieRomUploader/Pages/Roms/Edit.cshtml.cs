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
        public RomVM RomVM { get; set; }

        public SelectList ConsoleList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rom = await _context.Rom.FirstOrDefaultAsync(m => m.ID == id);

            if (rom == null)
            {
                return NotFound();
            }
            RomVM = RomVM.FromRom(rom);
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
            // if user hasn't uploaded a file, don't throw a validation error and just process the remaining fields
            if (ModelState[$"{nameof(RomVM)}.{nameof(RomVM.RomFile)}"].ValidationState != ModelValidationState.Valid)
                ModelState.Remove($"{nameof(RomVM)}.{nameof(RomVM.RomFile)}");

            if (!ModelState.IsValid)
            {
                return await InitPage();
            }

            var rom = await _context.Rom.FindAsync(id);
            if (rom == null)
            {
                return NotFound();
            }
            var oldConsoleType = rom.ConsoleTypeID;

            // Write the new file if one was uploaded
            try
            {
                if (RomVM.RomFile != null)
                {

                    await RomVM.WriteUploadedRomFileToDisk(_romFileManager);
                    _romFileManager.DeleteRomFile(rom.ConsoleTypeID, rom.Filename);
                }
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error writing rom file to disk");
                ModelState.AddModelError("Rom.RomFile", "File already exists");
                return await InitPage();
            }

            var entry = _context.Attach(rom);
            var newValues = new Dictionary<string, object>()
            {
                { "Title", RomVM.Title },
                { "ReleaseDate", RomVM.ReleaseDate },
                { "ConsoleTypeID", RomVM.ConsoleTypeID },
            };
            if (RomVM.RomFile != null)
                newValues.Add("Filename", RomVM.RomFile.FileName);
            entry.CurrentValues.SetValues(newValues);

            await _context.SaveChangesAsync();
            if (RomVM.RomFile == null)
                MoveRomFileIfRequired(oldConsoleType, rom);

            return RedirectToPage("./Index");
        }

        private void MoveRomFileIfRequired(string oldConsoleType, Rom updatedRom)
        {
            if (oldConsoleType == updatedRom.ConsoleTypeID)
                return;

            _romFileManager.MoveFileToConsoleDir(oldConsoleType, updatedRom.ConsoleTypeID, updatedRom.Filename);
        }
    }
}
