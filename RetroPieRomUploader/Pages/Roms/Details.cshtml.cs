using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RetroPieRomUploader.Data;
using RetroPieRomUploader.Models;

namespace RetroPieRomUploader.Pages.Roms
{
    public class DetailsModel : PageModel
    {
        private readonly RetroPieRomUploader.Data.RetroPieRomUploaderContext _context;

        public DetailsModel(RetroPieRomUploader.Data.RetroPieRomUploaderContext context)
        {
            _context = context;
        }

        public Rom Rom { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Rom = await _context.Rom.Include(m => m.ConsoleType).FirstOrDefaultAsync(m => m.ID == id);

            if (Rom == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
