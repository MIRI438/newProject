using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace NEWPROJECT.Services
{
    public class TokenService
    {
        private static readonly string secretKey = "SXkSqsKyNUyvGbnHs7ke2NCq8zQzNLW7mPmHbnZZ";
        private static readonly string issuer = "http://localhost:5190";

        public static string GenerateToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: issuer,  // ודא שה-Audience מוגדר כראוי
                claims: claims,
                expires: DateTime.Now.AddDays(30), // הגדרת תוקף של 30 יום
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static bool IsTokenExpired(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                return validatedToken.ValidTo <= DateTime.UtcNow;
            }
            catch (SecurityTokenExpiredException)
            {
                return true;
            }
            catch (Exception)
            {
                return true;
            }
        }

        public static TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidIssuer = issuer,
                ValidAudience = issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            };
        }

        public static string WriteToken(SecurityToken token) =>
            new JwtSecurityTokenHandler().WriteToken(token);
    }
}
