using RetroPieRomUploader.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RetroPieRomUploader.ViewModels
{
    public class RomDetailsVM
    {
        private Rom _rom;

        public RomDetailsVM(Rom rom)
        {
            _rom = rom;
        }

        public int ID => _rom.ID;
        public string Title => _rom.Title;

        [DisplayName("Console")]
        public ConsoleType ConsoleType => _rom.ConsoleType;

        [DisplayName("Release Date")]
        [DataType(DataType.Date)]
        public DateTime? ReleaseDate => _rom.ReleaseDate;

        [DisplayName("Files")]
        public List<string> FileEntries => _rom.FileEntries?.Select(f => f.Filename).ToList();
    }
}
