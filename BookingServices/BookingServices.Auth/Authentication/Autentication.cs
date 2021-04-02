using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Helpers.Authentication
{
    public class Authentication
    {
        public const string TokenAudience = "Myself";
        public const string TokenIssuer = "MyProject";
        private readonly SymmetricSecurityKey key;

        public Authentication(SymmetricSecurityKey symmetricKey)
        {
            this.key = symmetricKey;
        }

        public string BuildJwt(System.Security.Claims.ClaimsPrincipal principal)
        {
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: TokenIssuer,
                audience: TokenAudience,
                claims: principal.Claims,
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
