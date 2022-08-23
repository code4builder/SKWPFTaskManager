using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKWPFTaskManager.Api.Models
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // issuer of token
        public const string AUDIENCE = "MyAuthClient"; // consumer of token
        const string KEY = "mysupersecret_secretkey!123";   // key for crypting
        public const int LIFETIME = 1; // Lifetime of this token = 1 minute
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
