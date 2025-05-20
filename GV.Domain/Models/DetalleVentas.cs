using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GV.Domain.Models
{
    public class DetalleVenta
    {
        [Key]
        public int IdDetalleVenta { get; set; }

        [Required]

        public int IdVenta { get; set; }

        [Required]

        public int IdArticulo { get; set; }

        [Required]

        public int Cantidad {  get; set; }

        [Required]

        public decimal PrecioVenta { get; set; }

        [Required]

        public decimal Descuento { get; set; }
    }
}
