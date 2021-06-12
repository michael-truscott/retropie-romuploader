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
        [DisplayName("Console")]
        public int ConsoleTypeID { get; set; }
        [DisplayName("Console")]
        public ConsoleType ConsoleType { get; set; }

        [DisplayName("Release Date")]
        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }
    }
}
