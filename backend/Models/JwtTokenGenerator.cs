using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace backend.Models
{
    public class JwtTokenGenerator
    {
        /// <summary>
        /// Criar um token JWT para o utilizador
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GenerateToken(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = GenerateRandomKey(); // Gerar uma chave aleatória de 256 bits
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userId)
                }),
                Expires = DateTime.UtcNow.AddHours(1), // O token expira em 1 hora
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Gerar uma chave aleatória de 256 bits
        /// </summary>
        /// <returns></returns>
        private byte[] GenerateRandomKey()
        {
            var key = new byte[32]; // 256 bits = 32 bytes
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }
            return key;
        }
    }
}
