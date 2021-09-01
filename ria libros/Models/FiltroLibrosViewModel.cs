using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ria_libros.Models
{
    public class FiltroLibrosViewModel
    {
        public List<Libros> Libros { get; set; }
        public SelectList Generos { get; set; }
        public string GenereLibro { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
    }
}
