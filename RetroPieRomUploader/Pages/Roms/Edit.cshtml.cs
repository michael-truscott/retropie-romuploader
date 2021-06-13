using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RetroPieRomUploader.Data;
using RetroPieRomUploader.Models;
using RetroPieRomUploader.ViewModels;

namespace RetroPieRomUploader.Pages.Roms
{
    public class EditModel : PageModel
    {
        private readonly RetroPieRomUploader.Data.RetroPieRomUploaderContext _context;

        public EditModel(RetroPieRomUploader.Data.RetroPieRomUploaderContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RomVM Rom { get; set; }

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
            Rom = RomVM.FromRom(rom);
            return await InitPage();
        }

        private async Task<IActionResult> InitPage()
        {
            ConsoleList = new SelectList(await _context.ConsoleType.ToListAsync(), nameof(ConsoleType.ID), nameof(ConsoleType.Name));
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            // temp fix to stop validation failing on IFormFile
            ModelState.Remove("Rom.RomFile");
            if (!ModelState.IsValid)
            {
                return await InitPage();
            }

            var rom = await _context.Rom.FindAsync(id);
            if (rom == null)
            {
                return NotFound();
            }
            var entry = _context.Attach(rom);
            entry.CurrentValues.SetValues(new
            {
                Title = Rom.Title,
                ReleaseDate = Rom.ReleaseDate,
                ConsoleTypeID = Rom.ConsoleTypeID,
            });
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
