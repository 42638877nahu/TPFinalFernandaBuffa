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
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,CorreoElectronico,Contraseña,Fotografia")] Usuario usuario, IFormFile Fotografia)
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
                    usuario.Fotografia = "/Fotografias/Excursiones/" + fileName;
                }

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

            // GET: Usuarios/Edit/5
            public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,CorreoElectronico,Contraseña,Fotografia")] Usuario usuario, IFormFile nuevaFotografia)
        {
            if (id != usuario.Id)
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
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Fotografias/Usuarios", fileName);

                        // Guardar el archivo en el servidor
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await nuevaFotografia.CopyToAsync(stream);
                        }

                        // Guardar la nueva ruta relativa en la base de datos
                        usuario.Fotografia = "/Fotografias/Usuarios/" + fileName;
                    }
                    else
                    {
                        // Mantener la fotografía actual si no se ha subido una nueva
                        usuario.Fotografia = _context.Usuarios.AsNoTracking().FirstOrDefault(u => u.Id == id)?.Fotografia;
                    }

                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
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
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
