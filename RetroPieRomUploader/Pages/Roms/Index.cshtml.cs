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
using RetroPieRomUploader.ViewModels;

namespace RetroPieRomUploader.Pages.Roms
{
    public class IndexModel : PageModel
    {
        private readonly RetroPieRomUploader.Data.RetroPieRomUploaderContext _context;

        public IndexModel(RetroPieRomUploader.Data.RetroPieRomUploaderContext context)
        {
            _context = context;
        }

        public IList<RomDetailsVM> RomDetails { get;set; }
        public SelectList ConsoleList { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ConsoleFilter { get; set; }

        public async Task OnGetAsync()
        {
            IQueryable<Rom> query = _context.Rom.Include(rom => rom.ConsoleType)
                                                .Include(rom => rom.FileEntries)
                                                .AsNoTracking();
            if (!string.IsNullOrEmpty(ConsoleFilter))
                query = query.Where(rom => rom.ConsoleTypeID == ConsoleFilter);

            RomDetails = (await query.ToListAsync())
                .Select(r => new RomDetailsVM(r)).ToList();

            ConsoleList = new SelectList(await _context.ConsoleType.OrderBy(c => c.Name).ToListAsync(),
                                nameof(ConsoleType.ID), nameof(ConsoleType.Name));
        }
    }
}
