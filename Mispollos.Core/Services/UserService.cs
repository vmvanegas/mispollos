using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mispollos.Domain.Contracts.Repositories;
using Mispollos.Domain.Contracts.Services;
using Mispollos.Domain.Entities;
using Mispollos.Domain.Models;
using Mispollos.Infrastructure.Configuration;
using Mispollos.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mispollos.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IAsyncRepository<Usuario> _userRepository;
        private EmailService _emailService;
        private readonly AppSettings _appSettings;

        public UserService(IAsyncRepository<Usuario> userRepository, IOptions<AppSettings> appSettings)
        {
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
        }

        public async Task<List<Usuario>> GetUsers()
        {
            return (await _userRepository.ListAllAsync()).OrderBy(x => x.UpdatedOn).ToList();
        }

        public async Task<PagedResult<Usuario>> GetUsersPaged(int page, string search)
        {
            var result = new PagedResult<Usuario>();
            if (!string.IsNullOrEmpty(search))
            {
                result.Data = _userRepository
                    .Query(x => x.Nombre.Contains(search))
                    .OrderByDescending(x => x.UpdatedOn)
                    .Skip((page - 1) * 10)
                    .Take(10)
                    .ToList();

                result.Total = await _userRepository.CountByQuery(x => x.Nombre.Contains(search));
            }
            else
            {
                result.Data = (await _userRepository
                    .ListAllAsync())
                    .OrderByDescending(x => x.UpdatedOn)
                    .Skip((page - 1) * 10)
                    .Take(10)
                    .ToList();

                result.Total = await _userRepository.Count();
            }

            return result;
        }

        public Usuario GetUserById(Guid id)
        {
            return _userRepository.Query().Include(x => x.Tienda).Include(x => x.Rol).FirstOrDefault(x => x.Id == id);
        }

        public async Task CreateUser(Usuario user)
        {
            user.IdRol = _appSettings.IdRolAdmin;
            user.Clave = StringExtension.HashPassword(user.Clave);
            user.CreatedOn = DateTime.Now;
            await _userRepository.AddAsync(user);
        }

        public Boolean ValidateEmailExists(string email)
        {
            var repeatedEmail = _userRepository.Query().FirstOrDefault(x => x.Correo == email);

            return (repeatedEmail != null) ? true : false;
        }

        public async Task UpdateUser(Usuario user)
        {
            if (!string.IsNullOrEmpty(user.Clave))
            {
                user.Clave = StringExtension.HashPassword(user.Clave);
            }
            else
            {
                user.Clave = _userRepository.Query(x => x.Id == user.Id).AsNoTracking().FirstOrDefault().Clave;
            }
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteUser(Guid id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public Usuario GetUserByToken(Guid token)
        {
            return _userRepository.Query().FirstOrDefault(x => x.Token == token);
        }

        public async Task CreateEmployee(Usuario user)
        {
            user.IdRol = _appSettings.IdRolUser;
            Guid token = Guid.NewGuid();
            user.TokenExpiration = DateTime.Now.AddDays(1);
            user.Token = token;
            user.Clave = StringExtension.HashPassword(user.Clave);
            user.CreatedOn = DateTime.Now;
            await _userRepository.AddAsync(user);
            _emailService.Send(user.Correo, token, "generatedPasswordEmail", "Tu cuenta en Mispollos ha sido creada");
        }

        public async Task RecoverAccount(RecoverPasswordEmail email)
        {
            Usuario user = _userRepository.Query().FirstOrDefault(x => x.Correo == email.Email);
            user.TokenExpiration = DateTime.Now.AddDays(1);
            user.Token = Guid.NewGuid();
            await _userRepository.UpdateAsync(user);
            _emailService.Send(user.Correo, user.Token, "recoverPasswordEmail", "Solicitud de recuperacion de contraseña");
        }

        public Usuario AuthenticateUser(Authenticate userCredentials)
        {
            userCredentials.Password = StringExtension.HashPassword(userCredentials.Password);
            return _userRepository.Query().Include(x => x.Rol).FirstOrDefault(x => x.Correo == userCredentials.Email && x.Clave == userCredentials.Password);
        }

        public AuthenticatedUserInfo GenerateJwt(Usuario user)
        {
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

            AuthenticatedUserInfo authenticatedUserInfo = new AuthenticatedUserInfo
            {
                Id = user.Id,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Correo = user.Correo,
                IdTienda = user.IdTienda,
                rol = user.Rol.Nombre,
                Token = tokenString,
            };

            return authenticatedUserInfo;
        }
    }
}