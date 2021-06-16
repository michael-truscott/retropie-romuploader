using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
    public class IndexModel : PageModel
    {
        private readonly RetroPieRomUploader.Data.RetroPieRomUploaderContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(RetroPieRomUploader.Data.RetroPieRomUploaderContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IList<RomDetailsVM> RomDetails { get;set; }
        public SelectList ConsoleList { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ConsoleFilter { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public async Task OnGetAsync()
        {
            IQueryable<Rom> query = _context.Rom.Include(rom => rom.ConsoleType)
                                                .Include(rom => rom.FileEntries)
                                                .AsNoTracking();
            if (!string.IsNullOrEmpty(ConsoleFilter))
                query = query.Where(rom => rom.ConsoleTypeID == ConsoleFilter);

            // SQLite string LIKE is case-sensitive, so compare uppercase'd strings
            if (!string.IsNullOrEmpty(SearchString))
                query = query.Where(rom => rom.Title.ToUpper().Contains(SearchString.ToUpper()));

            RomDetails = (await query.ToListAsync())
                .Select(r => new RomDetailsVM(r)).ToList();

            ConsoleList = new SelectList(await _context.ConsoleType.OrderBy(c => c.Name).ToListAsync(),
                                nameof(ConsoleType.ID), nameof(ConsoleType.Name));
        }

        public ActionResult OnPostReboot()
        {
            _logger.LogInformation("EmulationStation reboot requested");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                System.IO.File.Create("/tmp/es-restart");
                Process.Start("killall", "/opt/retropie/supplementary/emulationstation/emulationstation");
            }
            return StatusCode(200);
        }
    }
}
