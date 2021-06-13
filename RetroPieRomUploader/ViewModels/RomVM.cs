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
        public string ConsoleTypeID { get; set; }
        
        [DisplayName("Console")]
        public string ConsoleName => _consoleType?.Name;

        [DisplayName("Release Date")]
        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }

        [Required]
        [DisplayName("Upload Rom File")]
        public IFormFile RomFile { get; set; }

        public async Task WriteUploadedRomFileToDisk(IRomFileManager romFileManager)
        {
            if (romFileManager.RomFileExists(ConsoleTypeID, RomFile.FileName))
                throw new ArgumentException($"File {RomFile.FileName} already exists on disk.");

            var filepath = romFileManager.GetRomFilePath(ConsoleTypeID, RomFile.FileName);
            using (var stream = System.IO.File.OpenWrite(filepath))
                await RomFile.CopyToAsync(stream);
        }

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
