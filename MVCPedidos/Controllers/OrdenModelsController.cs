using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCPedidos.Data;
using MVCPedidos.Models;

namespace MVCPedidos.Controllers
{
    public class OrdenModelsController : Controller
    {
        private readonly PedidosDBContext _context;

        public OrdenModelsController(PedidosDBContext context)
        {
            _context = context;
        }

        // GET: OrdenModels
        public async Task<IActionResult> Index()
        {
            var pedidosDBContext = _context.Orden.Include(o => o.Usuario);
            return View(await pedidosDBContext.ToListAsync());
        }

        // GET: OrdenModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordenModel = await _context.Orden
                .Include(o => o.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ordenModel == null)
            {
                return NotFound();
            }

            return View(ordenModel);
        }

        // GET: OrdenModels/Create
        public IActionResult Create()
        {
            ViewData["IdUsuario"] = new SelectList(_context.Usuario, "Id", "Email");
            return View();
        }

        // POST: OrdenModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdUsuario,Fecha,Estado,Total")] OrdenModel ordenModel)
        {
            //if (ModelState.IsValid)
            //{
                _context.Add(ordenModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            ViewData["IdUsuario"] = new SelectList(_context.Usuario, "Id", "Email", ordenModel.IdUsuario);
            return View(ordenModel);
        }

        // GET: OrdenModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordenModel = await _context.Orden.FindAsync(id);
            if (ordenModel == null)
            {
                return NotFound();
            }
            ViewData["IdUsuario"] = new SelectList(_context.Usuario, "Id", "Email", ordenModel.IdUsuario);
            return View(ordenModel);
        }

        // POST: OrdenModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdUsuario,Fecha,Estado,Total")] OrdenModel ordenModel)
        {
            if (id != ordenModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ordenModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdenModelExists(ordenModel.Id))
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
            ViewData["IdUsuario"] = new SelectList(_context.Usuario, "Id", "Email", ordenModel.IdUsuario);
            return View(ordenModel);
        }

        // GET: OrdenModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordenModel = await _context.Orden
                .Include(o => o.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ordenModel == null)
            {
                return NotFound();
            }

            return View(ordenModel);
        }

        // POST: OrdenModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ordenModel = await _context.Orden.FindAsync(id);
            if (ordenModel != null)
            {
                _context.Orden.Remove(ordenModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdenModelExists(int id)
        {
            return _context.Orden.Any(e => e.Id == id);
        }
    }
}
