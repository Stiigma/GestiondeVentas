using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GV.Application.DTOs
{
    public class VentaDTO
    {

        public int IdVenta { get; set; }


        public int IdUsuario { get; set; }
        public string NombreUsu { get; set; }

        public int IdPersona { get; set; }

        public string NombreProv { get; set; }


        public string TipoComprobante { get; set; }

      
        public string SerieComprobante { get; set; }

      
        public string NumeroComprobante { get; set; }

      

        public DateTime FechaHora { get; set; }



        public decimal ImpuestoVenta { get; set; }

  
        public decimal TotalVenta { get; set; }

        public bool Estado { get; set; }
    }
}
