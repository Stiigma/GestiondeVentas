using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GV.Domain.Models;

namespace GV.Application.DTOs
{
    public class VentaViewDTO
    {
        public int IdVenta { get; set; }
        public int IdUsuario { get; set; }
        public int IdPersona { get; set; }

        public string TipoComprobante { get; set; }

        public string SerieComprobante { get; set; }

        public string NumeroComprabante { get; set; }

        public decimal ImpuestoVenta { get; set; }

        public decimal TotalVenta { get; set; }


        public List<DetalleVentaDTO> Articulos { get; set; }
    }
}
