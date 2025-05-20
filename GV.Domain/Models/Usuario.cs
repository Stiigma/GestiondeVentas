using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GV.Domain.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        public int IdRol { get; set; }

        [Required]
        [MaxLength(150)]
        public string NombreUsuario { get; set; }

        [MaxLength(30)]
        public string? TipoDocumento { get; set; }

        [MaxLength(20)]
        public string? NumeroDocumento { get; set; }

        [MaxLength(150)]
        public string? Direccion { get; set; }

        [MaxLength(14)]
        public string? Telefono { get; set; }

        [MaxLength(50)]
        public string? EmailUsuario { get; set; }

        [Required]
        [MaxLength(150)]
        public string PasswordHash { get; set; }

        [Required]
        [MaxLength(150)]
        public string PasswordSalt { get; set; }

        [Required]
        public bool Estado { get; set; }
    }
}
