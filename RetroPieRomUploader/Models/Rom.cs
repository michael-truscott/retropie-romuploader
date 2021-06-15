using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RetroPieRomUploader.Models
{
    public class Rom
    {
        public int ID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string ConsoleTypeID { get; set; }
        public ConsoleType ConsoleType { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }

        public List<RomFileEntry> FileEntries { get; set; }
    }
}
