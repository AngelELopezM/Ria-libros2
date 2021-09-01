using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace ria_libros.Services
{
    public class Services
    {
        string directorioActual;
        #region metodos

        public void agregarLibro() 
        {
            
        }

        public void CrearCarpetaGuardado()
        {
            /*Aqui validamos si existe una carpeta con el nombre de libros, si no existe pues la creamos
             porque ahi es donde se van a guardar los PDFs de los libros que se suban*/
            string directorioLibros = directorioActual + @"/libros";
            if (!Directory.Exists(directorioLibros))
            {
                Directory.CreateDirectory(directorioLibros);
            }
            
        }
        public string conseguirDirectorioActual() 
        {
            /*Aqui aqui conseguimos el directorio en el cual el programa se esta ejecutando,
             luego de eso dividimos el directorio para quitarle las doble \\ y convertirlo a una ruta
            Que se pueda utilizar, finalmente utilizamos la variable goblar para almacenar el directorio actual*/
            var dir = Directory.GetCurrentDirectory();
            string[] directorioDiv = dir.Split("\\").ToArray();

            foreach (var item in directorioDiv)
            {
                dir = item + @"/";
                directorioActual = directorioActual + dir;
            }
            return directorioActual;
        }

        #endregion
    }
}
