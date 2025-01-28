using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SPA.Web.Options;

public class AuthOption
{
    public class AuthOptions
    {
        public const string ISSUER = "FreeSPA"; // token publisher
        public const string AUDIENCE = "SPA"; // token customer
        const string KEY = "YourSuperSecretKeyThatIs32BytesLong";   // encryption key
        public const int LIFETIME = 1440; // token liftime
        public const string UserIdCalmName = "UserId";
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}