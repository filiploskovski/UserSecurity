using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using JwtTokenAuthentication.Interfaces;
using JwtTokenAuthentication.JwtHelper;
using JwtTokenAuthentication.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JwtTokenAuthentication.Services
{
    public class UserService : IUserService
    {
        private readonly IOptions<AppSettings> _jwtTokenKey;

        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private readonly List<UserModel> _users = new List<UserModel>
        {
            new UserModel { Id = 1, FirstName = "Test", LastName = "User", Username = "test", Password = "test" }
        };

        public UserService(IOptions<AppSettings> jwtTokenKey)
        {
            _jwtTokenKey = jwtTokenKey;
        }


        public IEnumerable<UserModel> GetAll()
        {
            return _users;
        }

        public UserModel Authenticate(string username, string password)
        {
            var user = _users.SingleOrDefault(x => x.Username == username && x.Password == password);

            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_jwtTokenKey.Value.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user;
        }
    }
}
