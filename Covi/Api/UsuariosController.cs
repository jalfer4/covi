using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Covi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Covi.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsuariosController : ControllerBase
    {
        private readonly CoviContext _context;
        private readonly IConfiguration config;
        public UsuariosController(CoviContext context, IConfiguration config)
        {
            _context = context;
            this.config = config;
        }


        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuario()
        {
            return await _context.Usuario.ToListAsync();
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
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

        // POST: api/Usuarios
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                   password: usuario.Clave,
                   salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                   prf: KeyDerivationPrf.HMACSHA1,
                   iterationCount: 1000,
                   numBytesRequested: 256 / 8));
            usuario.Clave = hashed;

            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.UsuarioId }, usuario);
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Usuario>> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuario.Remove(usuario);
            await _context.SaveChangesAsync();

            return usuario;
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.UsuarioId == id);
        }


        // GET api/usuarios/login/
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginView loginView)
        {
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: loginView.Clave,
                    salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));
                var p = _context.Usuario.FirstOrDefault(x => x.Email == loginView.Email);
                if (p == null || p.Clave != hashed)
                {
                    return BadRequest("Nombre de usuario o clave incorrecta");
                }
                else
                {
                    var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
                    var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, p.Email),
                        new Claim("FullName", p.Nombre + " " + p.Apellido),
                        new Claim(ClaimTypes.Role, "Propietario"),
                    };

                    var token = new JwtSecurityToken(
                        issuer: config["TokenAuthentication:Issuer"],
                        audience: config["TokenAuthentication:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddYears(2),
                        signingCredentials: credenciales
                    );
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        // GET: api/Usuarios/GetUsuarioByEmail/5
        [HttpGet("GetUsuarioByEmail/{id}")]
        public async Task<ActionResult<Usuario>> GetUsuarioByEmail(String id)
        {
            //var usuario = await _context.Usuario.FindAsync(id);

            var usuario = from o_Usuario in _context.Usuario
                          where o_Usuario.Email == id
                          select o_Usuario;


            if (usuario == null)
            {
                return NotFound();
            }

            return usuario.FirstOrDefault();
        }

        [HttpGet("validarEmail/{id}")]
        public async Task<ActionResult<bool>> validarEmail(String id)
        {
            //var usuario = await _context.Usuario.FindAsync(id);

            var usuario = from o_Usuario in _context.Usuario
                          where o_Usuario.Email == id
                          select o_Usuario;


            if (usuario == null)
            {
                return false;
            }

            return true;
        }

        [HttpGet("validarClave/{id}")]
        public async Task<ActionResult<bool>> validarClave(String id)
        {
            //var usuario = await _context.Usuario.FindAsync(id);

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                   password: id,
                   salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                   prf: KeyDerivationPrf.HMACSHA1,
                   iterationCount: 1000,
                   numBytesRequested: 256 / 8));


            var usuario = (from o_Usuario in _context.Usuario
                           where o_Usuario.Clave == hashed
                           select o_Usuario).FirstOrDefault();


            if (usuario == null)
            {
                return false;
            }

            return true;
        }

        [HttpPost("validarUsuario")]
        [AllowAnonymous]
        public async Task<ActionResult<Usuario>> validarUsuario(LoginView loginView)
        {
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: loginView.Clave,
                    salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));
                var p = _context.Usuario.FirstOrDefault(x => x.Email == loginView.Email);
                if (p == null || p.Clave != hashed)
                {
                    return NotFound();
                }
                else
                {
                    var usuario = await _context.Usuario.FindAsync(p.UsuarioId);
  
                    return usuario;

                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
