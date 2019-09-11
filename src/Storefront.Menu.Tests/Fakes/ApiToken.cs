using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Storefront.Menu.API.Authorization;

namespace Storefront.Menu.Tests.Fakes
{
    public sealed class ApiToken
    {
        private JwtOptions _jwtOptions;

        public ApiToken(JwtOptions jwtOptions)
        {
            _jwtOptions = jwtOptions;
        }

        public long TenantId { get; set; } = 1;
        public long UserId { get; set; } = 1;

        public override string ToString()
        {
            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(
                    key: new SymmetricSecurityKey(
                        key: Encoding.ASCII.GetBytes(_jwtOptions.Secret)
                    ),
                    algorithm: SecurityAlgorithms.HmacSha256
                ),
                claims: new[]
                {
                    new Claim("TenantId", TenantId.ToString()),
                    new Claim("UserId", UserId.ToString())
                }
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
