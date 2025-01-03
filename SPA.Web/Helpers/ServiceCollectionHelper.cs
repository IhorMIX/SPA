using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SPA.Web.Options;

namespace SPA.Web.Helpers;

public static class ServiceCollectionHelper
{
    public static void AddJwtAuth(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,

                    ValidIssuer = AuthOption.AuthOptions.ISSUER,

                    ValidateAudience = true,

                    ValidAudience = AuthOption.AuthOptions.AUDIENCE,

                    ValidateLifetime = true,


                    IssuerSigningKey = AuthOption.AuthOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                };
            });
    }
}