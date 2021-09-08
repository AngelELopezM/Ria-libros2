using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ria_libros.Models
{
    public class Libros
    {
         
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Este campo es requerido")]
        [MaxLength(100)]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "Este campo es requerido")]
        [MaxLength(100)]
        public string Genero { get; set; }
        [Required(ErrorMessage = "Este campo es requerido")]
        public string Autor { get; set; }
        [Required(ErrorMessage = "Este campo es requerido")]
        public int año { get; set; }

        /*El archivo que vamos a guardas no lo vamos a guardar como binario en la base de datos
         en vez de eso vamos a guardar una copia del archivo en una ubicacion especifica*/
        public string ubicacion { get; set; }

    }
}
