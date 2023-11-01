using MarketServer.WebApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MarketServer.WebApi.Services;

public static class JwtService
{
    public static string CreatToken(User user)
    {
        var claims = new Claim[]
        {
            //user bilgileri token içerisine eklendi
            new("UserId", user.Id.ToString()),
            new("UserName", user.Name + " " + user.Lastname)
        };

        JwtSecurityToken handler = new(
            issuer: "Issuer",
            audience: "Audience",
            claims: claims,
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("My secret key My secret key My secret key My secret key")), SecurityAlgorithms.HmacSha256));

        string token = new JwtSecurityTokenHandler().WriteToken(handler);
        return token;
    }    
}
