using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GV.Domain.Models
{
    public class Persona
    {
        [Key]
        public int IdPersona { get; set; }

        [Required]

        public string TipoPersona { get; set; }

        [Required]

        public string NombrePersona { get; set; }


        public string TipoDocumento { get; set; }


        public string NumeroDocumento { get;set; }
        

        public string DireccionPersona { get; set; }

        
        public string TelefonoPersona { get; set; }

        public string EmailPersona { get; set; }

        [Required]

        public bool Estado {  get; set; } 

    }
}
