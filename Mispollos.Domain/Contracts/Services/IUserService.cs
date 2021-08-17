using Mispollos.Domain.Entities;
using Mispollos.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mispollos.Domain.Contracts.Services
{
    public interface IUserService
    {
        Task<List<Usuario>> GetUsers();

        Usuario GetUserById(Guid id);

        Usuario GetUserByToken(Guid token);

        Task<PagedResult<Usuario>> GetUsersPaged(int page, string search);

        Task CreateUser(Usuario user);

        Task CreateEmployee(Usuario user);

        Task UpdateUser(Usuario user);

        Task DeleteUser(Guid id);

        Boolean ValidateEmailExists(string email);

        Usuario AuthenticateUser(Authenticate userCredentials);

        AuthenticatedUserInfo GenerateJwt(Usuario user);

        Task RecoverAccount(RecoverPasswordEmail email);
    }
}