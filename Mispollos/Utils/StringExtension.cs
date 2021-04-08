using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using Mispollos.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Mispollos
{
    public static class StringExtension
    {
        private readonly static string _salt = "rVhywrhJ4b2Q/Yicxt2jHg==";

        public static string HashPassword(string password)
        {
            // generate a 128-bit salt using a secure PRNG
            var salt = Convert.FromBase64String(_salt);

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hashed;
        }
    }
}