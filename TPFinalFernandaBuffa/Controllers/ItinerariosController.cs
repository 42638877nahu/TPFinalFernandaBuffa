using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using TPFinalFernandaBuffa.Data;
using TPFinalFernandaBuffa.Models;

namespace TPFinalFernandaBuffa.Controllers
{
    [Authorize]
    public class ItinerariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItinerariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Itinerarios
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Itinerarios.Include(i => i.Usuario);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Itinerarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itinerario = await _context.Itinerarios
                .Include(i => i.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itinerario == null)
            {
                return NotFound();
            }

            return View(itinerario);
        }

        // GET: Itinerarios/Create
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre");
            return View();
        }

        // POST: Itinerarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,FechaInicio,FechaFin,UsuarioId,Descripcion")] Itinerario itinerario, IFormFile fileUpload)
        {
            if (ModelState.IsValid)
            {
                if (fileUpload != null && fileUpload.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await fileUpload.CopyToAsync(stream);
                        using (var workbook = new XLWorkbook(stream))
                        {
                            var worksheet = workbook.Worksheet(1); // Considerando que los datos están en la primera hoja
                            var row = worksheet.Row(2); // Saltamos la fila de encabezado

                            itinerario.Descripcion = row.Cell(1).GetString(); // Suponiendo que la descripción está en la primera columna
                        }
                    }
                }

                _context.Add(itinerario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre", itinerario.UsuarioId);
            return View(itinerario);
        }

        // GET: Itinerarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itinerario = await _context.Itinerarios.FindAsync(id);
            if (itinerario == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id", itinerario.UsuarioId);
            return View(itinerario);
        }

        // POST: Itinerarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,FechaInicio,FechaFin,UsuarioId,Descripcion")] Itinerario itinerario)
        {
            if (id != itinerario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itinerario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItinerarioExists(itinerario.Id))
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
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id", itinerario.UsuarioId);
            return View(itinerario);
        }

        // GET: Itinerarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itinerario = await _context.Itinerarios
                .Include(i => i.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itinerario == null)
            {
                return NotFound();
            }

            return View(itinerario);
        }

        // POST: Itinerarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var itinerario = await _context.Itinerarios.FindAsync(id);
            if (itinerario != null)
            {
                _context.Itinerarios.Remove(itinerario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItinerarioExists(int id)
        {
            return _context.Itinerarios.Any(e => e.Id == id);
        }
    }
}
