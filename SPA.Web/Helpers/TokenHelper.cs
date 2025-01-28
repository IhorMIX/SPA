using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using SPA.Web.Options;

namespace SPA.Web.Helpers;

public class TokenHelper
{
    public string GetToken(int userId)
    {
        var identity = GetIdentity(userId);
        
        var now = DateTime.UtcNow;
        
        var jwt = new JwtSecurityToken(
            issuer: AuthOption.AuthOptions.ISSUER,
            audience: AuthOption.AuthOptions.AUDIENCE,
            notBefore: now,
            claims: identity.Claims,
            expires: now.Add(TimeSpan.FromMinutes(AuthOption.AuthOptions.LIFETIME)),
            signingCredentials: new SigningCredentials(AuthOption.AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return encodedJwt;
    }

    private ClaimsIdentity GetIdentity(int userId)
    {
        var claims = new List<Claim>
        {
            new (AuthOption.AuthOptions.UserIdCalmName, userId.ToString()),
        };
        ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
        
        return claimsIdentity;
    }

    public static string GenerateRefreshToken(string token)
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber) + token.Substring(1,10).GetHashCode();
        }
    }
}