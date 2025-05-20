using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GV.Domain.Models
{
    public class Categoria
    {
        [Key]
        public int IdCategoria { get; set; }

        public string ClaveCategoria { get; set; }

        public string NombreCategoria { get; set; }

        public string DescripcionCategorica { get; set; }

        public bool Estado { get; set; }

    }
}
