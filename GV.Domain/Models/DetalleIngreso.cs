using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GV.Domain.Models
{
    public class DetalleIngreso
    {
        [Key]
        public int IdDetalleIngreso { get; set; }

        [Required]

        public int IdIngreso { get; set; }

        [Required]

        public int IdArticulo { get; set; }

        [Required]

        public decimal Cantidad { get; set; }

        [Required]

        public decimal PrecioVenta { get; set; }


    }
}
