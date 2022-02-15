using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Escuela_de_Magia.Models
{
    public class ESTUDIANTE
    {
        [Key]
        public int Id { get; set; }

        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Solo Caracteres")]
        [MaxLength(20), Required()]
        public string Nombre { get; set; } = "";

        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Solo Caracteres")]
        [MaxLength(20), Required()]
        public string Apellido { get; set; } = "";

        [Required()]
        public int Identificacion { get; set; } = 0;

        [Required()]
        public int Edad { get; set; } = 0;

        [ForeignKey("Casa")]
        public int CasaId { get; set; } = 0;
        public virtual CASA Casa { get; set; }
    }
}
