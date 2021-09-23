using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ria_libros.Models
{
    public class UsuariosAdmin
    {
        [Key]
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Contra { get; set; }
    }
}
