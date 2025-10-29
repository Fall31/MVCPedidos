using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using MVCPedidos.Data;
using MVCPedidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVCPedidos.Controllers
{
    public class AccountController : Controller
    {
        private readonly PedidosDBContext _context;

        public AccountController(PedidosDBContext context)
        {
            _context = context;
        }

        // GET: UsuarioModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuario.ToListAsync());
        }

        // GET: UsuarioModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioModel = await _context.Usuario
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuarioModel == null)
            {
                return NotFound();
            }

            return View(usuarioModel);
        }

        // GET: UsuarioModels/Create
        public IActionResult Create()
        {
            ViewBag.Roles = new List<SelectListItem>
            {
                new SelectListItem { Value = "Admin", Text = "Admin" },
                new SelectListItem { Value = "Cliente", Text = "Cliente" },
                new SelectListItem { Value = "Empleado", Text = "Empleado" }
            };
            return View();
        }

        // POST: UsuarioModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Email,Password,Rol")] UsuarioModel usuarioModel)
        {
            //if (ModelState.IsValid)
            //{
                _context.Add(usuarioModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            return View(usuarioModel);

        }

        // GET: UsuarioModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioModel = await _context.Usuario.FindAsync(id);
            if (usuarioModel == null)
            {
                return NotFound();
            }
            ViewBag.Roles = new List<SelectListItem>
            {
                new SelectListItem { Value = "Admin", Text = "Admin" },
                new SelectListItem { Value = "Cliente", Text = "Cliente" },
                new SelectListItem { Value = "Empleado", Text = "Empleado" }
            };
            return View(usuarioModel);
        }

        // POST: UsuarioModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Email,Password,Rol")] UsuarioModel usuarioModel)
        {
            if (id != usuarioModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuarioModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioModelExists(usuarioModel.Id))
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
            return View(usuarioModel);
        }

        // GET: UsuarioModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioModel = await _context.Usuario
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuarioModel == null)
            {
                return NotFound();
            }

            return View(usuarioModel);
        }

        // POST: UsuarioModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuarioModel = await _context.Usuario.FindAsync(id);
            if (usuarioModel != null)
            {
                _context.Usuario.Remove(usuarioModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioModelExists(int id)
        {
            return _context.Usuario.Any(e => e.Id == id);
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var usuario = _context.Usuario.FirstOrDefault(u => u.Email == email && u.Activo);

            if (usuario != null && BCrypt.Net.BCrypt.Verify(password, usuario.Password))
            {
                // Crear claims del usuario
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Email),
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Role, usuario.Rol),
                    new Claim("Nombre", usuario.Nombre),
                    new Claim("FechaNacimiento", usuario.FechaNacimiento.ToString("yyyy-MM-dd"))
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Redirigir según el rol
                if (usuario.Rol == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Email o contraseña incorrectos";
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email, string password, string nombreCompleto, DateTime fechaNacimiento)
        {
            // Validar que el email no exista
            if (_context.Usuario.Any(u => u.Email == email))
            {
                ModelState.AddModelError("", "El email ya está registrado");
                return View();
            }

            // Crear nuevo usuario
            var usuario = new UsuarioModel
            {
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                Nombre = nombreCompleto,
                FechaNacimiento = fechaNacimiento,
                Rol = "Usuario",
                Activo = true
            };

            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();

            // Iniciar sesión automáticamente
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Role, usuario.Rol),
                new Claim("Nombre", usuario.Nombre),
                new Claim("FechaNacimiento", usuario.FechaNacimiento.ToString("yyyy-MM-dd"))
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
