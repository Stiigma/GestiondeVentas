using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GV.Application.DTOs
{
    public class IngresoViewDTO
    {

        public int IdUsuario { get; set; }
        public int IdPersona { get; set; }

        public string TipoComprobante { get; set; }

        public string SerieComprobante { get; set; }

        public string NumeroComprabante { get; set; }

        public decimal Impuesto { get; set; }

        public decimal TotalIngreso { get; set; }


        public List<DetalleIngresoDTO> Articulos { get; set; }

    }
}
