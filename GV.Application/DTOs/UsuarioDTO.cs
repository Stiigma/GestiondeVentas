using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GV.Application.DTOs
{
    public class UsuarioDTO
    {
        [Key]
        public int  IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
    }
}
