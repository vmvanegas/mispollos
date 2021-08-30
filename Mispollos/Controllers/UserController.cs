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
using Mispollos.Application.Services;
using Mispollos.Configuration;
using Mispollos.Domain.Contracts.Services;
using Mispollos.Domain.Entities;
using Mispollos.Domain.Models;
using MySql.Data.MySqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mispollos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        // Metodo traer lista de usuarios
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _service.GetUsers();
            return Ok(result);
        }

        // GET: api/<UserController>
        [HttpGet("p/{page}")]
        public async Task<IActionResult> Get(int page, string search = null)
        {
            var result = await _service.GetUsersPaged(page, search);
            return Ok(result);
        }

        // Traer un usuario por id
        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public Usuario Get(Guid id)
        {
            return _service.GetUserById(id);
        }

        // Traer un usuario por token
        // GET api/<UserController>/5
        [HttpGet("recuperar-clave/{token}")]
        public Usuario GetByToken(Guid token)
        {
            return _service.GetUserByToken(token);
        }

        //Crear usuario
        // POST api/user
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Usuario usuario)
        {
            Boolean repeatedEmail = _service.ValidateEmailExists(usuario.Correo);

            if (!repeatedEmail)
            {
                await _service.CreateUser(usuario);

                return Ok();
            }

            return BadRequest(new { message = "El correo ya esta en uso" });
        }

        // POST api/<UserController>/empleado
        //[Authorize(Roles = Role.Admin)]
        //[HttpPost("empleado")]
        public async Task<IActionResult> PostEmpleado([FromBody] Usuario usuario)
        {
            Boolean repeatedEmail = _service.ValidateEmailExists(usuario.Correo);

            if (!repeatedEmail)
            {
                await _service.CreateEmployee(usuario);

                return Ok();
            }

            return BadRequest(new { message = "El correo ya esta en uso" });
        }

        // POST api/user/recuperar-cuenta
        //[HttpPost("recuperar-cuenta")]
        public IActionResult PostRecuperarCuenta([FromBody] RecoverPasswordEmail email)
        {
            Boolean emailExists = _service.ValidateEmailExists(email.Email);

            if (emailExists)
            {
                _service.RecoverAccount(email);
                return Ok();
            }
            return BadRequest(new { message = "El correo no existe" });
        }

        // POST api/user/authenticate
        [HttpPost("authenticate")]
        public IActionResult Authenticate(Authenticate model)
        {
            Usuario user = _service.AuthenticateUser(model);

            if (user == null)
            {
                return BadRequest(new { message = "El correo o clave son incorrectos" });
            }

            AuthenticatedUserInfo authenticatedUserInfo = _service.GenerateJwt(user);

            // return basic user info and authentication token
            return Ok(authenticatedUserInfo);
        }

        // Actualizar usuario
        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Usuario user)
        {
            await _service.UpdateUser(user);

            return Ok();
        }

        // Borrar usuario
        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteUser(id);
            return Ok();
        }
    }
}