using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RetroPieRomUploader.Data;
using RetroPieRomUploader.Models;
using RetroPieRomUploader.ViewModels;

namespace RetroPieRomUploader.Pages.Roms
{
    public class CreateModel : PageModel
    {
        private readonly RetroPieRomUploader.Data.RetroPieRomUploaderContext _context;
        private readonly IRomFileManager _romFileManager;
        private ILogger<CreateModel> _logger;

        public CreateModel(RetroPieRomUploader.Data.RetroPieRomUploaderContext context, IRomFileManager romFileManager, ILogger<CreateModel> logger)
        {
            _context = context;
            _romFileManager = romFileManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ConsoleList = new SelectList(await _context.ConsoleType.OrderBy(c => c.Name).ToListAsync(), nameof(ConsoleType.ID), nameof(ConsoleType.Name));
            return Page();
        }

        [BindProperty]
        public RomVM Rom { get; set; }

        public SelectList ConsoleList { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            // todo: make validation display more nicely
            try
            {
                await Rom.WriteUploadedRomFileToDisk(_romFileManager);
            }
            catch (ArgumentException ex)
            {
                // todo: show error
                _logger.LogError(ex, "Error writing rom file to disk");
                ModelState.AddModelError("Rom.RomFile", "File already exists");
                return Page();
            }

            var rom = Rom.ToRom();
            _context.Rom.Add(rom);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
