using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ria_libros.Data;
using ria_libros.Models;
using ria_libros.Services;

namespace ria_libros.Controllers
{
    public class LibrosController : Controller
    {
        private readonly ria_librosContext _context;
        private Services.Services _services;
        private bool admin;

        public LibrosController(ria_librosContext context)
        {
            _context = context;
            _services = new Services.Services(context);
            
        }




        public async Task<IActionResult> Welcome()
        {
            
            return View();
        }
        // GET: Libros
        public async Task<IActionResult> Index(FiltroLibrosViewModel filtro, UsuariosAdmin usuarios)
        {
            if (!string.IsNullOrWhiteSpace(usuarios.Usuario))
            {
               ViewBag.ConfirmAdmin = _services.ConfirmAdmin(usuarios);
               
            }

            
            ViewBag.IsAdmin = TempData["IsAdmin"];

            var libros = _services.FiltrarLibros(filtro);

            

            IQueryable<string> Generoquery = from m in _context.Libros
                                             orderby m.Genero
                                             select m.Genero;

            /*Para que la vista pueda funcionar debemos pasarle un modelo de este tipo
             por esa razon construimos un objeto y dentro del objeto le pasamos lo que queremos
            que reciba la vista*/
            var libroscons = new FiltroLibrosViewModel
            {

                Generos = new SelectList( await Generoquery.Distinct().ToListAsync()),
                Libros = libros
                

            };

            return View(libroscons);
        }

        // GET: Libros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libros = await _context.Libros
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libros == null)
            {
                return NotFound();
            }

            return View(libros);
        }

        // GET: Libros/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Libros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Genero,Autor,año,ubicacion")] Libros libros, IFormFile files)
        {
            
            /*Aqui validamos si ya existe un libro con el mismo nombre y autor*/
            var librosexist = _context.Libros.Where(x=> x.Titulo == libros.Titulo && x.Autor == libros.Autor);
            
            if (!librosexist.Any())
            {
                if (ModelState.IsValid)
                {
                    /*En la vista tenemos una condicion para que se muestre un mensaje de error, utilizamos el 
                     viewbag para pasar la data de manera temporal sin la necesidad de persistirla o de tener
                    que utilizar un modelo*/
                    ViewBag.validacion = false;
                    
                    await _services.agregarLibro(files);
                    //en services tengo una variable publica que guarda la ubicacion del archivo, se la paso al registro y despues lo guardo
                    libros.ubicacion = _services.ubicacion;
                    _context.Add(libros);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(libros);
            }
            else 
            {
                /*En la vista tenemos una condicion para que se muestre un mensaje de error, utilizamos el 
                     viewbag para pasar la data de manera temporal sin la necesidad de persistirla o de tener
                    que utilizar un modelo, es una manera rapida de pasar data*/
                ViewBag.validacion = true;
                return View(libros);
            }
            
        }

        // GET: Libros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {   
            /*Aqui decimos que si la persona que va a editar el documento no es un admin pues entonces lo va a returnar
             a la vista de index*/
            if (!admin)
            {
                TempData["IsAdmin"] = false;
                /*Utilizamos este redirectToaction para volver a llamar la accion del index si el usuario no es un admin*/
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return NotFound();
            }

            var libros = await _context.Libros.FindAsync(id);
            if (libros == null)
            {
                return NotFound();
            }
            return View(libros);
        }

        // POST: Libros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Genero,Autor,año,ubicacion")] Libros libros)
        {
            

            if (id != libros.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(libros);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibrosExists(libros.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(libros);
        }

        // GET: Libros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!admin)
            {
                TempData["IsAdmin"] = false;
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return NotFound();
            }

            var libros = await _context.Libros
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libros == null)
            {
                return NotFound();
            }

            return View(libros);
        }

        // POST: Libros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var libros = await _context.Libros.FindAsync(id);
            _context.Libros.Remove(libros);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibrosExists(int id)
        {
            return _context.Libros.Any(e => e.Id == id);
        }

        //DownloadFile
        public async Task<IActionResult> descargarLibro(int id)
        {
            /*Aqui buscamos el libro que queremos descargar*/
            var file = _context.Libros.Where(x => x.Id == id).FirstOrDefault();
            if (file == null) return null;
            /*Este es para utilizar la ubicacion que tenemos guardada en la BD y despues dividirla
             para poder sacar el nombre del archivo*/
            string[] Filename = file.ubicacion.Split("\\");
            
            var memory = new MemoryStream();
            using (var stream = new FileStream(file.ubicacion, FileMode.Open))
            {
                //Aqui le pasamos el archivo a la memoria, para despues finalmennte procesar la descarga
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/pdf", Filename[Filename.Length-1]);
        }
    }
    
}
