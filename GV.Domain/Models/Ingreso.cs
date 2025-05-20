using System.ComponentModel.DataAnnotations;

namespace GV.Domain.Models
{
    public class Ingreso
    {
        [Key]
        public int IdIngreso { get; set; }

        [Required]
        public int IdUsuario { get; set; }

        [Required]
        public int IdPersona { get; set; }

        [Required]
        public string TipoComprobante { get; set; }

        [Required]
        public string SerieComprobante { get; set; }

        [Required]
        public string NumeroComprabante { get; set; }


        [Required]

        public DateTime FechaHoraIngreso { get; set; }

        [Required]

        public decimal Impuesto { get; set; }

        [Required]


        public decimal TotalIngreso { get; set; }

        [Required]

        public bool Estado {  get; set; }


    }
}
