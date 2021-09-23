using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ria_libros.Models;
using ria_libros.Data;


namespace ria_libros.Services
{
    public class Services
    {
        private readonly ria_librosContext _context;
        public bool admin= false;
        
        public Services(ria_librosContext context)   
        {
            _context = context;
            
        }
            public  string directorioLibros, ubicacion;
        
        
        #region metodos

        /*Utilizamos la interface the IFormFile para para poder manejar el file que va a subir el usuario*/
        public async Task<string> agregarLibro(IFormFile file) 
        {
            
            try
            {
                CrearCarpetaGuardadoGeneral(file);
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var filePath = Path.Combine(directorioLibros, file.FileName);
                
                ubicacion = filePath;
                var extension = Path.GetExtension(file.FileName);
                if (!File.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }

            return  ubicacion;
            
            
        }

        public void CrearCarpetaGuardadoGeneral(IFormFile file)
        {
            /*Aqui validamos si existe una carpeta con el nombre de libros que es la que utilizaremos
             * para guardar las subcarpetas que van a contener los archivos PDF, si no existe pues la creamos
             porque ahi es donde se van a guardar los PDFs de los libros que se suban*/

             directorioLibros = Path.Combine( Directory.GetCurrentDirectory() + "\\libros\\");
            if (!Directory.Exists(directorioLibros))
            {
                Directory.CreateDirectory(directorioLibros);
            }
            
            
        }
        public List<Libros> FiltrarLibros(FiltroLibrosViewModel filtro)
        {
            var libros = from m in _context.Libros
                         select m;
            if (!string.IsNullOrEmpty(filtro.Titulo))
            {
                libros = libros.Where(x => x.Titulo.Contains(filtro.Titulo.ToString()));
            }
            if (!string.IsNullOrEmpty(filtro.Autor))
            {
                libros = libros.Where(x => x.Autor.Contains(filtro.Autor.ToString()));
            }
            if (!string.IsNullOrEmpty(filtro.GenereLibro))
            {
                if (filtro.GenereLibro != "Todos")
                {
                    libros = libros.Where(x => x.Genero == filtro.GenereLibro.ToString());
                }
            }
            return libros.ToList();
        }

        /*Aqui utilizamos para saber si el usuario que ingresaron es un admin(LOS ADMIN SON LOS UNICOS QUE TIENEN DERECHO A EDITAR Y ELIMINAR LIBROS O INFORMACION)*/
        public bool ConfirmAdmin(UsuariosAdmin usuario)
        {
            try
            {
                
                var listadoAdmin = from m in _context.UsuaiosAdmin
                                   select m;
                listadoAdmin = listadoAdmin.Where(x => x.Usuario == usuario.Usuario && x.Contra == usuario.Contra);
                if (listadoAdmin.Any())
                {
                    return true;
                }
                
                return false; 
            }
            catch (Exception)
            {

                throw;
            }
            
        }
      /*  public string conseguirDirectorioActual() 
        {
            //Aqui aqui conseguimos el directorio en el cual el programa se esta ejecutando,
             //luego de eso dividimos el directorio para quitarle las doble \\ y convertirlo a una ruta
            //Que se pueda utilizar, finalmente utilizamos la variable goblar para almacenar el directorio actual

            var dir = Directory.GetCurrentDirectory();
            string[] directorioDiv = dir.Split("\\").ToArray();

            foreach (var item in directorioDiv)
            {
                dir = item + @"/";
                directorioActual = directorioActual + dir;
            }
            return directorioActual;
           }
                    */
        #endregion
    }
}
