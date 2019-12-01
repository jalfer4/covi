using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Covi.Models;

namespace Covi.Controllers
{
    public class TipoLocalsController : Controller
    {
        private readonly CoviContext _context;

        public TipoLocalsController(CoviContext context)
        {
            _context = context;
        }

        // GET: TipoLocals
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipoLocal.ToListAsync());
        }

        // GET: TipoLocals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoLocal = await _context.TipoLocal
                .FirstOrDefaultAsync(m => m.TipoLocald == id);
            if (tipoLocal == null)
            {
                return NotFound();
            }

            return View(tipoLocal);
        }

        // GET: TipoLocals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoLocals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TipoLocald,Nombre,EstaHabilitado,EstaPublicado")] TipoLocal tipoLocal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoLocal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoLocal);
        }

        // GET: TipoLocals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoLocal = await _context.TipoLocal.FindAsync(id);
            if (tipoLocal == null)
            {
                return NotFound();
            }
            return View(tipoLocal);
        }

        // POST: TipoLocals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TipoLocald,Nombre,EstaHabilitado,EstaPublicado")] TipoLocal tipoLocal)
        {
            if (id != tipoLocal.TipoLocald)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoLocal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoLocalExists(tipoLocal.TipoLocald))
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
            return View(tipoLocal);
        }

        // GET: TipoLocals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoLocal = await _context.TipoLocal
                .FirstOrDefaultAsync(m => m.TipoLocald == id);
            if (tipoLocal == null)
            {
                return NotFound();
            }

            return View(tipoLocal);
        }

        // POST: TipoLocals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoLocal = await _context.TipoLocal.FindAsync(id);
            _context.TipoLocal.Remove(tipoLocal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoLocalExists(int id)
        {
            return _context.TipoLocal.Any(e => e.TipoLocald == id);
        }
    }
}
