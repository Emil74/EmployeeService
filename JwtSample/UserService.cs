using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JwtSample
{
    public class UserService
    {
        private const string SecretCode = "kYp3s6v9y/B?E(H+";


        private IDictionary<string, string> _users = new Dictionary<string, string>()
        {
            { "user1", "password" }, // 0
            { "user2", "password" }, // 1
            { "user3", "password" }, // 2
            { "user4", "password" }, // 3
            { "user5", "password" }, // 4
        };

        public string Authenticate(string user, string password)
        {
            if (string.IsNullOrWhiteSpace(user) ||
            string.IsNullOrWhiteSpace(password))
            {
                return string.Empty;

            }

            int i = 0;

            foreach (var usr in _users)
            {
                if (string.CompareOrdinal(usr.Key, user) == 0 &&
                string.CompareOrdinal(usr.Value, password) == 0)
                {
                    return GenerateJwtToken(i, usr.Key);
                }
                i++;
            }



            return string.Empty;
        }

        private string GenerateJwtToken(int id, string name)
        {
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            byte[] key = Encoding.ASCII.GetBytes(SecretCode);
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor();
            securityTokenDescriptor.SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
            securityTokenDescriptor.Expires = DateTime.Now.AddMinutes(15);
            securityTokenDescriptor.Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Name, name),

            });


            SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            return jwtSecurityTokenHandler.WriteToken(securityToken);
        }




    }
}
