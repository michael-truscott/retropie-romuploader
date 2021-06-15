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
    public class EditRomVM
    {
        public int ID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DisplayName("Console")]
        public string ConsoleTypeID { get; set; }

        [DisplayName("Release Date")]
        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }

        public List<string> FileEntries { get; set; }

        public static EditRomVM FromRom(Rom r)
        {
            return new EditRomVM
            {
                ID = r.ID,
                Title = r.Title,
                ConsoleTypeID = r.ConsoleTypeID,
                ReleaseDate = r.ReleaseDate,
                FileEntries = r.FileEntries.Select(e => e.Filename).ToList(),
            };
        }
    }
}
