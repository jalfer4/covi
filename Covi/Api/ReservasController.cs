using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Covi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace Covi.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ReservasController : ControllerBase
    {
        private readonly CoviContext _context;

        public ReservasController(CoviContext context)
        {
            _context = context;
        }

        // GET: api/Reservas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetReserva()
        {
            return await _context.Reserva.Where(obj => obj.EstaHabilitado == true && obj.EstaPublicado == true).ToListAsync();
        }

        // GET: api/GetMisReserva/1
        [HttpGet("GetMisReserva/{id}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetMisReserva(int id)
        {

            var reservas = from o_Reserva in _context.Reserva
                          join o_evento in _context.Evento on o_Reserva.EventoId equals o_evento.EventoId
                          where o_Reserva.UsuarioId == id && o_Reserva.EstaHabilitado == true &&  o_Reserva.EstaPublicado == true
                          select new {o_Reserva.ReservaId, o_Reserva.UsuarioId, fechaReserva = o_Reserva.FechaAlta, o_evento.EventoId, o_Reserva.EstaHabilitado, o_Reserva.EstaPublicado, o_evento.NombreArtista, FechaEvento = o_evento.FechaAlta, o_evento.TipoEventoNavigation.Nombre} ;

            //return await _context.Reserva.Where(obj => obj.UsuarioId == id).ToListAsync();
            return reservas.ToList();
        }

        // GET: api/Reservas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reserva>> GetReserva(int id)
        {
            var reserva = await _context.Reserva.FindAsync(id);

            if (reserva == null)
            {
                return NotFound();
            }

            return reserva;
        }

        // PUT: api/Reservas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReserva(int id, Reserva reserva)
        {
            if (id != reserva.ReservaId)
            {
                return BadRequest();
            }

            _context.Entry(reserva).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(id))
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

        // PUT: api/Reservas/5
        [HttpPut("PutBajaLogica/{id}")]
        public async Task<IActionResult> PutBajaLogica(int id)
        {

            var reserva = await _context.Reserva.FindAsync(id);

            if (id != reserva.ReservaId)
            {
                return BadRequest();
            }

            reserva.EstaHabilitado = false;
            reserva.EstaPublicado = false;

            _context.Entry(reserva).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(id))
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

        // POST: api/Reservas
        [HttpPost]
        public async Task<ActionResult<Reserva>> PostReserva(Reserva reserva)
        {
            _context.Reserva.Add(reserva);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReserva", new { id = reserva.ReservaId }, reserva);
        }

        // DELETE: api/Reservas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Reserva>> DeleteReserva(int id)
        {
            var reserva = await _context.Reserva.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }

            _context.Reserva.Remove(reserva);
            await _context.SaveChangesAsync();

            return reserva;
        }

        private bool ReservaExists(int id)
        {
            return _context.Reserva.Any(e => e.ReservaId == id && e.EstaHabilitado == true && e.EstaPublicado == true);
        }
    }
}
