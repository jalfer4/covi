using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Covi.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Covi.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepositorioUsuario usuarios;
        private readonly IConfiguration config;
        private readonly CoviContext _context;
        public HomeController(IRepositorioUsuario usuarios, IConfiguration config, CoviContext context)
        {
            this.usuarios = usuarios;
            this.config = config;
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Titulo = "Página de Inicio";
            ViewData["TotalEventos"] = new SelectList(_context.Evento, "EventoId", "NombreArtista").Count();
            ViewData["TotalUsuario"] = new SelectList(_context.Usuario, "UsuarioId", "Apellido").Count();
            ViewData["TotalLocales"] = new SelectList(_context.Local, "LocalId", "Nombre").Count();
            ViewData["TotalReservas"] = new SelectList(_context.Reserva, "ReservaId", "FechaAlta").Count();
            return View();
            //return View(clientes);
        }

        // GET: Home/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Home/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginView loginView)
        {
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: loginView.Clave,
                    salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));
                var p = usuarios.ObtenerPorEmail(loginView.Email);
                if (p == null || p.Clave != hashed)
                {
                    ViewBag.Mensaje = "Datos inválidos";
                    return View();
                }
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, p.Email),
                   
                    // new Claim("FullName", p.Nombre + " " + p.Apellido)
                     new Claim("ApiyNom", p.ApyNomb ),
                     new Claim("Id",Convert.ToString(p.UsuarioId)),
                    //new Claim(ClaimTypes.Role, p.IdPropietario < 10? "Administrador":"Propietario"),
                    new Claim(ClaimTypes.Role, "Administrador"),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    //AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.
                    AllowRefresh = true,
                    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    //IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    //IssuedUtc = <DateTimeOffset>,
                    // The time at which the authentication ticket was issued.

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View();
            }
        }

        // GET: Home/Login
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Seguro()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            return View(claims);
        }

        [Authorize(Policy = "Administrador")]
        public ActionResult Admin()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            return View(claims);
        }

        public ActionResult Restringido()
        {
            return View();
        }
    }
}
