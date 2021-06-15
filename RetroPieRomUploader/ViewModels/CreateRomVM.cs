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
    public class CreateRomVM
    {
        #region Rom model properties

        [Required]
        public string Title { get; set; }

        [Required]
        [DisplayName("Console")]
        public string ConsoleTypeID { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Release Date")]
        public DateTime? ReleaseDate { get; set; }

        #endregion

        [Required]
        [DisplayName("Upload Rom File(s)")]
        public List<IFormFile> FileUploads { get; set; }

        public async Task WriteUploadedRomFilesToDisk(IRomFileManager romFileManager)
        {
            var existingFile = FileUploads.FirstOrDefault(file => romFileManager.RomFileExists(ConsoleTypeID, file.FileName));
            if (existingFile != null)
                throw new ArgumentException($"File {existingFile.FileName} already exists on disk.");

            foreach (var file in FileUploads)
            {
                var filepath = romFileManager.GetRomFilePath(ConsoleTypeID, file.FileName);
                using (var stream = System.IO.File.OpenWrite(filepath))
                    await file.CopyToAsync(stream);
            }
        }
    }
}
