using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TPFinalFernandaBuffa.Data;
using TPFinalFernandaBuffa.Models;

namespace TPFinalFernandaBuffa.Controllers
{
    [Authorize]
    public class DestinosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DestinosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Destinos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Destinos.ToListAsync());
        }

        // GET: Destinos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var destino = await _context.Destinos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (destino == null)
            {
                return NotFound();
            }

            return View(destino);
        }

        // GET: Destinos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Destinos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Ubicacion,Fotografia")] Destino destino, IFormFile Fotografia)
        {
            if (ModelState.IsValid)
            {
                if (Fotografia != null && Fotografia.Length > 0)
                {
                    // Definir la ruta donde se almacenará la imagen
                    var fileName = Path.GetFileName(Fotografia.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Fotografias/Destinos", fileName);

                    // Guardar el archivo en el servidor
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Fotografia.CopyToAsync(stream);
                    }

                    // Guardar la ruta relativa en la base de datos
                    destino.Fotografia = "/Fotografias/Destinos/" + fileName;
                }

                _context.Add(destino);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(destino);
        }

        // GET: Destinos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var destino = await _context.Destinos.FindAsync(id);
            if (destino == null)
            {
                return NotFound();
            }
            return View(destino);
        }

        // POST: Destinos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Ubicacion,Fotografia")] Destino destino, IFormFile nuevaFotografia)
        {
            if (id != destino.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar si se ha subido una nueva fotografía
                    if (nuevaFotografia != null && nuevaFotografia.Length > 0)
                    {
                        // Definir la ruta donde se almacenará la nueva imagen
                        var fileName = Path.GetFileName(nuevaFotografia.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Fotografias/Destinos", fileName);

                        // Guardar el archivo en el servidor
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await nuevaFotografia.CopyToAsync(stream);
                        }

                        // Guardar la nueva ruta relativa en la base de datos
                        destino.Fotografia = "/Fotografias/Destinos/" + fileName;
                    }
                    else
                    {
                        // Mantener la fotografía actual si no se ha subido una nueva
                        destino.Fotografia = _context.Usuarios.AsNoTracking().FirstOrDefault(u => u.Id == id)?.Fotografia;
                    }

                    _context.Update(destino);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DestinoExists(destino.Id))
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
            return View(destino);
        }

        // GET: Destinos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var destino = await _context.Destinos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (destino == null)
            {
                return NotFound();
            }

            return View(destino);
        }

        // POST: Destinos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var destino = await _context.Destinos.FindAsync(id);
            if (destino != null)
            {
                _context.Destinos.Remove(destino);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DestinoExists(int id)
        {
            return _context.Destinos.Any(e => e.Id == id);
        }
    }
}
