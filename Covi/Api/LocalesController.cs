using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Covi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Covi.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LocalesController : ControllerBase
    {
        private readonly CoviContext _context;

        public LocalesController(CoviContext context)
        {
            _context = context;
        }

        // GET: api/Locales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Local>>> GetLocal()
        {
            var resultSet = _context.Local.ToListAsync();

            return await resultSet;
        }



        // GET: api/Locales/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Local>> GetLocal(int id)
        {
            var local = await _context.Local.FindAsync(id);

            if (local == null)
            {
                return NotFound();
            }

            return local;
        }



        // GET: api/Locales/GetLocalCompleto/2
        [HttpGet("GetLocalCompleto/{id}")]
        public async Task<ActionResult<Object>> GetLocalCompleto(int id)
        {
            var returnResults = new List<Object>();

            var locales =  _context.Local;
           
            foreach (var dato in locales)
            {

                dato.Evento.Add(_context.Evento.FirstOrDefault());
                returnResults.Add(dato);
            }
               

           

            return returnResults;
        }




        // PUT: api/Locales/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocal(int id, Local local)
        {
            if (id != local.LocalId)
            {
                return BadRequest();
            }

            _context.Entry(local).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocalExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Locales
        [HttpPost]
        public async Task<ActionResult<Local>> PostLocal(Local local)
        {
            _context.Local.Add(local);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLocal", new { id = local.LocalId }, local);
        }

        // DELETE: api/Locales/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Local>> DeleteLocal(int id)
        {
            var local = await _context.Local.FindAsync(id);
            if (local == null)
            {
                return NotFound();
            }

            _context.Local.Remove(local);
            await _context.SaveChangesAsync();

            return local;
        }

        private bool LocalExists(int id)
        {
            return _context.Local.Any(e => e.LocalId == id);
        }
    }
}
