using System.Security.Claims;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ToDoApi.Infrastructure.Settings;

namespace ToDoApi.Infrastructure.Helper
{
    public class JwtHelper
    {
        private readonly JwtSettings jwtSettings;
        public JwtHelper(IOptions<JwtSettings> options)
        {
            jwtSettings=options.Value;
        }

        public string GenerateJwtToken(string username,string userId)
        {
            var key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SignInKey));
            var credentials=new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var userclaims=new []{
                new Claim(JwtRegisteredClaimNames.Sub,userId),
                new Claim(JwtRegisteredClaimNames.UniqueName,username)
            };

            var issuer=jwtSettings.Issuer;

            var audeince=jwtSettings.Audience;

            var expireTimeSpan=DateTime.Now.AddHours(2);

            var token=new JwtSecurityToken(issuer,audeince,
            expires:expireTimeSpan,
            notBefore:DateTime.Now,
            claims:userclaims,
            signingCredentials:credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}