using Microsoft.AspNetCore.Http;
using RetroPieRomUploader.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RetroPieRomUploader.ViewModels
{
    public class RomVM
    {
        public int ID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DisplayName("Console")]
        public int ConsoleTypeID { get; set; }
        
        [DisplayName("Console")]
        public string ConsoleName => _consoleType?.Name;

        [DisplayName("Release Date")]
        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }

        [Required]
        [DisplayName("Upload Rom File")]
        public IFormFile RomFile { get; set; }

        public string Filename => $"{_filename}";


        private ConsoleType _consoleType { get; set; }
        private string _filename;

        public Rom ToRom()
        {
            return new Rom
            {
                ID = ID,
                Title = Title,
                ConsoleTypeID = ConsoleTypeID,
                ReleaseDate = ReleaseDate,
                Filename = RomFile?.FileName ?? _filename,
            };
        }

        public static RomVM FromRom(Rom r)
        {
            return new RomVM
            {
                ID = r.ID,
                Title = r.Title,
                ConsoleTypeID = r.ConsoleTypeID,
                _consoleType = r.ConsoleType,
                ReleaseDate = r.ReleaseDate,
                _filename = r.Filename,
            };
        }
    }
}
