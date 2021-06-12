using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RetroPieRomUploader.Data;
using RetroPieRomUploader.Models;

namespace RetroPieRomUploader.Pages.Roms
{
    public class CreateModel : PageModel
    {
        private readonly RetroPieRomUploader.Data.RetroPieRomUploaderContext _context;

        public CreateModel(RetroPieRomUploader.Data.RetroPieRomUploaderContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ConsoleList = new SelectList(await _context.ConsoleType.ToListAsync(), nameof(ConsoleType.ID), nameof(ConsoleType.Name));
            return Page();
        }

        [BindProperty]
        public Rom Rom { get; set; }

        public SelectList ConsoleList { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Rom.Add(Rom);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
