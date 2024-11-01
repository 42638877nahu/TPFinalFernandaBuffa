using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TPFinalFernandaBuffa.Data;
using TPFinalFernandaBuffa.Models;

namespace TPFinalFernandaBuffa.Controllers
{
    [Authorize]
    public class ExcursionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExcursionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Excursiones
        public async Task<IActionResult> Index(string searchString)
        {
            // Incluir el destino en la consulta
            IQueryable<Excursion> applicationDbContext = _context.Excursiones.Include(e => e.Destino);

            // Filtrar por nombre si se proporciona un valor de búsqueda
            if (!string.IsNullOrEmpty(searchString))
            {
                applicationDbContext = applicationDbContext.Where(e => e.Nombre.Contains(searchString));
            }

            ViewData["CurrentFilter"] = searchString; // Mantener el valor de búsqueda en la vista

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Excursiones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var excursion = await _context.Excursiones
                .Include(e => e.Destino)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (excursion == null)
            {
                return NotFound();
            }

            return View(excursion);
        }

        // GET: Excursiones/Create
        public IActionResult Create()
        {
            ViewData["DestinoId"] = new SelectList(_context.Destinos, "Id", "Nombre");
            return View();
        }

        // POST: Excursiones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Precio,DestinoId")] Excursion excursion, IFormFile Fotografia)
        {
            if (ModelState.IsValid)
            {
                if (Fotografia != null && Fotografia.Length > 0)
                {
                    // Definir la ruta donde se almacenará la imagen
                    var fileName = Path.GetFileName(Fotografia.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Fotografias/Excursiones", fileName);

                    // Guardar el archivo en el servidor
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Fotografia.CopyToAsync(stream);
                    }

                    // Guardar la ruta relativa en la base de datos
                    excursion.Fotografia = "/Fotografias/Excursiones/" + fileName;
                }

                _context.Add(excursion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(excursion);
        }

        // GET: Excursiones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var excursion = await _context.Excursiones.FindAsync(id);
            if (excursion == null)
            {
                return NotFound();
            }
            ViewData["DestinoId"] = new SelectList(_context.Destinos, "Id", "Id", excursion.DestinoId);
            return View(excursion);
        }

        // POST: Excursiones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Precio,DestinoId")] Excursion excursion,IFormFile NuevaFotografia)
        {
            if (id != excursion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Si se carga una nueva fotografía, reemplaza la existente
                    if (NuevaFotografia != null && NuevaFotografia.Length > 0)
                    {
                        // Definir la ruta de la nueva imagen
                        var fileName = Path.GetFileName(NuevaFotografia.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Fotografias/Excursiones", fileName);

                        // Guardar el nuevo archivo en el servidor
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await NuevaFotografia.CopyToAsync(stream);
                        }

                        // Actualizar la ruta de la imagen en la base de datos
                        excursion.Fotografia = "/Fotografias/Excursiones/" + fileName;
                    }

                    _context.Update(excursion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExcursionExists(excursion.Id))
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

            ViewData["DestinoId"] = new SelectList(_context.Destinos, "Id", "Nombre", excursion.DestinoId);
            return View(excursion);
        }
            // GET: Excursiones/Delete/5
            public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var excursion = await _context.Excursiones
                .Include(e => e.Destino)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (excursion == null)
            {
                return NotFound();
            }

            return View(excursion);
        }

        // POST: Excursiones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var excursion = await _context.Excursiones.FindAsync(id);
            if (excursion != null)
            {
                _context.Excursiones.Remove(excursion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExcursionExists(int id)
        {
            return _context.Excursiones.Any(e => e.Id == id);
        }
    }
}