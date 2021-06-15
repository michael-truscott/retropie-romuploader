using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RetroPieRomUploader.Models
{
    public class RomFileEntry
    {
        public int ID { get; set; }

        [Required]
        public int RomID { get; set; }
        public Rom Rom { get; set; }

        [Required]
        public string Filename { get; set; }
    }
}
