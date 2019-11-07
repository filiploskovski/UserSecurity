using System.Collections.Generic;
using JwtTokenAuthentication.Models;

namespace JwtTokenAuthentication.Interfaces
{
    public interface IUserService
    {
        UserModel Authenticate(string username, string password);
        IEnumerable<UserModel> GetAll();
    }
}