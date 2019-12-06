﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Covi.Models;

namespace Covi.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoUsuariosController : ControllerBase
    {
        private readonly CoviContext _context;

        public TipoUsuariosController(CoviContext context)
        {
            _context = context;
        }

        // GET: api/TipoUsuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoUsuario>>> GetTipoUsuario()
        {
            return await _context.TipoUsuario.ToListAsync();
        }

        // GET: api/TipoUsuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoUsuario>> GetTipoUsuario(int id)
        {
            var tipoUsuario = await _context.TipoUsuario.FindAsync(id);

            if (tipoUsuario == null)
            {
                return NotFound();
            }

            return tipoUsuario;
        }

        // PUT: api/TipoUsuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoUsuario(int id, TipoUsuario tipoUsuario)
        {
            if (id != tipoUsuario.TipoUsuarioId)
            {
                return BadRequest();
            }

            _context.Entry(tipoUsuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoUsuarioExists(id))
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

        // POST: api/TipoUsuarios
        [HttpPost]
        public async Task<ActionResult<TipoUsuario>> PostTipoUsuario(TipoUsuario tipoUsuario)
        {
            _context.TipoUsuario.Add(tipoUsuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoUsuario", new { id = tipoUsuario.TipoUsuarioId }, tipoUsuario);
        }

        // DELETE: api/TipoUsuarios/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TipoUsuario>> DeleteTipoUsuario(int id)
        {
            var tipoUsuario = await _context.TipoUsuario.FindAsync(id);
            if (tipoUsuario == null)
            {
                return NotFound();
            }

            _context.TipoUsuario.Remove(tipoUsuario);
            await _context.SaveChangesAsync();

            return tipoUsuario;
        }

        private bool TipoUsuarioExists(int id)
        {
            return _context.TipoUsuario.Any(e => e.TipoUsuarioId == id);
        }
    }
}
