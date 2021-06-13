using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RetroPieRomUploader.Data;
using RetroPieRomUploader.Models;
using RetroPieRomUploader.ViewModels;

namespace RetroPieRomUploader.Pages.Roms
{
    public class CreateModel : PageModel
    {
        private readonly RetroPieRomUploader.Data.RetroPieRomUploaderContext _context;
        private readonly IRomFileManager _romFileManager;

        public CreateModel(RetroPieRomUploader.Data.RetroPieRomUploaderContext context, IRomFileManager romFileManager)
        {
            _context = context;
            _romFileManager = romFileManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ConsoleList = new SelectList(await _context.ConsoleType.ToListAsync(), nameof(ConsoleType.ID), nameof(ConsoleType.Name));
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

            var console = await _context.ConsoleType.FirstOrDefaultAsync(c => c.ID == Rom.ConsoleTypeID);
            // todo: make validation display more nicely
            if (_romFileManager.RomFileExists(console.DirectoryName, Rom.RomFile.FileName))
                throw new ArgumentException($"File {Rom.RomFile.FileName} already exists on disk.");

            var filepath = _romFileManager.GetRomFilePath(console.DirectoryName, Rom.RomFile.FileName);
            using (var stream = System.IO.File.OpenWrite(filepath))
                await Rom.RomFile.CopyToAsync(stream);


            var rom = Rom.ToRom();
            _context.Rom.Add(rom);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
