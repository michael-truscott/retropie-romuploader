using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RetroPieRomUploader.Models
{
    public class ConsoleType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        // The unique subdirectory name for the console
        public string ID { get; set; }

        [Required]
        public string Name { get; set; }

        public List<Rom> Roms { get; set; }
    }
}
