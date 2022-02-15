using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Escuela_de_Magia.Models;

namespace Escuela_de_Magia.Models
{
    public class CASA
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }  
    }
}
