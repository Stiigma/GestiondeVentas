using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GV.Domain.Models
{
    public class Articulo
    {
        [Key]
        public int IdArticulo {get; set; }

        public int IdCategoria { get; set; }

        public string CodigoArticulo { get; set; }

        public string NombreArticulo { get; set; }

        public decimal PrecioVenta { get; set; }

        public int Stock { get; set; }

        public string DescripcionArticulo { get; set; }

        public bool Estado { get; set; }
    }
}
