using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GV.Application.DTOs
{
    public class DetalleVentaDTO
    {
        public int IdArticulo { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioVenta { get; set; }

        public decimal Descuento { get; set; }

    }
}
