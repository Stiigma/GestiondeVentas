using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GV.Domain.Models
{
    public class Venta
    {
        [Key]
        public int IdVenta { get; set; }


        [Required]
        public int IdUsuario { get; set; }

        [Required]
        public int IdPersona { get; set; }

        [Required]
        public string TipoComprobante { get; set; }

        [Required]
        public string SerieComprobante { get; set; }

        [Required]
        public string NumeroComprabante { get; set; }

        [Required]

        public DateTime FechaHora { get; set; }

        [Required]
        public int ImpuestoVenta  { get; set; }

        [Required]
        public int TotalVenta { get; set; }

    }
}
