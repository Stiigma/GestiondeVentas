using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GV.Application.DTOs
{
    public class IngresosDTO
    {
        
        public int IdIngreso { get; set; }


        public int IdUsuario { get; set; }
        public string NombreUsu { get; set; }

        public int IdPersona { get; set; }

        public string NombreProv { get; set; }

        [Required]
        public string TipoComprobante { get; set; }

        [Required]
        public string SerieComprobante { get; set; }

        [Required]
        public string NumeroComprabante { get; set; }


        [Required]

        public DateTime FechaHoraIngreso { get; set; }

        [Required]

        public decimal Impuesto { get; set; }

        [Required]


        public decimal TotalIngreso { get; set; }

        [Required]

        public bool Estado { get; set; }


    }
}
