using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ria_libros.Models;

namespace ria_libros.Data
{
    public class ria_librosContext : DbContext
    {
        

        public ria_librosContext (DbContextOptions<ria_librosContext> options)
            : base(options)
        {
        }

        public DbSet<ria_libros.Models.Libros> Libros { get; set; }
        public DbSet<ria_libros.Models.UsuariosAdmin> UsuaiosAdmin { get; set; }
    }
}
