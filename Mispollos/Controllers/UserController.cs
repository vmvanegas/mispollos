using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mispollos.Configuration;
using Mispollos.DataAccess;
using Mispollos.Entities;
using Mispollos.Models;
using MySql.Data.MySqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mispollos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MisPollosContext _context = new MisPollosContext();
        private readonly AppSettings _appSettings;

        public UserController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        // Metodo traer lista de usuarios
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { data = _context.Usuarios.Include(x => x.Tienda).Include(x => x.Rol).AsEnumerable() });
        }

        // GET: api/<UserController>
        [HttpGet("p/{page}")]
        public IActionResult Get(int page)
        {
            return Ok(new { data = _context.Usuarios.Where(x => x.Rol.Nombre == Role.User).Skip((page - 1) * 10).Include(x => x.Tienda).Include(x => x.Rol).Take(10).AsEnumerable(), total = _context.Usuarios.Where(x => x.Rol.Nombre == Role.User).Count() });
        }

        // Traer un usuario por id
        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public Usuario Get(Guid id)
        {
            return _context.Usuarios.FirstOrDefault(x => x.Id == id);
        }

        // Crear usuario
        // POST api/user
        [HttpPost]
        public IActionResult Post([FromBody] Usuario usuario)
        {
            var userWithSameEmail = _context.Usuarios.FirstOrDefault(x => x.Correo == usuario.Correo);

            if (userWithSameEmail == null)
            {
                usuario.IdRol = _appSettings.IdRolAdmin;
                usuario.Clave = StringExtension.HashPassword(usuario.Clave);
                var result = _context.Usuarios.Add(usuario);
                _context.SaveChanges();
                return Created("", result.Entity);
            }
            else
            {
                return BadRequest(new { message = "El correo ya esta en uso" });
            }
        }

        // POST api/<UserController>/empleado
        [Authorize(Roles = Role.Admin)]
        [HttpPost("empleado")]
        public IActionResult PostEmpleado([FromBody] Usuario usuario)
        {
            var userWithSameEmail = _context.Usuarios.FirstOrDefault(x => x.Correo == usuario.Correo);

            if (userWithSameEmail == null)
            {
                usuario.IdRol = _appSettings.IdRolUser;
                usuario.Clave = StringExtension.HashPassword(usuario.Clave);
                var result = _context.Usuarios.Add(usuario);
                _context.SaveChanges();
                return Created("", result.Entity);
            }
            else
            {
                return BadRequest(new { message = "El correo ya esta en uso" });
            }
        }

        // POST api/user/authenticate
        [HttpPost("authenticate")]
        public IActionResult Authenticate(Authenticate model)
        {
            model.Password = StringExtension.HashPassword(model.Password);

            var user = _context.Usuarios.Include(x => x.Rol).FirstOrDefault(x => x.Correo == model.Email && x.Clave == model.Password);

            Console.WriteLine(user);

            if (user == null)
            {
                return BadRequest(new { message = "El correo o clave son incorrectos" });
            }

            #region Login Jwt

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Rol.Nombre)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            #endregion Login Jwt

            // return basic user info and authentication token
            return Ok(new
            {
                Id = user.Id,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Correo = user.Correo,
                IdTienda = user.IdTienda,
                rol = user.Rol.Nombre,
                Token = tokenString
            });
        }

        // Actualizar usuario
        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Usuario usuario)
        {
            var result = _context.Usuarios.Attach(usuario);
            _context.Entry(usuario).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(result.Entity);
        }

        // Borrar usuario
        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(x => x.Id == id);
            if (_context.Entry(usuario).State == EntityState.Detached)
            {
                _context.Usuarios.Attach(usuario);
            }
            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
        }
    }
}