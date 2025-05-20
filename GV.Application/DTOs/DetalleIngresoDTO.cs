using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GV.Application.DTOs
{
    public class DetalleIngresoDTO
    {
        public int IdArticulo { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioVenta { get; set; }
    }
}
