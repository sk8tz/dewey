#if !DNXCORE50
using Dewey.Types;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Dewey.Security
{
    public class Crypto
    {
        public static string Key { get; set; }
        public static string Salt { get; set; }
        private static byte[] SaltBytes => Encoding.ASCII.GetBytes(Salt);

        public static string Encrypt(string plaintext) => Encrypt(plaintext, Key, SaltBytes);

        private static string Encrypt(string plaintext, string key, byte[] saltBytes)
        {
            if (plaintext == null)
                return "";

            var result = string.Empty;
            RijndaelManaged aes = null;

            try {
                var derivedKey = new Rfc2898DeriveBytes(key, saltBytes);

                aes = new RijndaelManaged();
                aes.Key = derivedKey.GetBytes(aes.KeySize / 8);

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var msEncrypt = new MemoryStream()) {
                    msEncrypt.Write(BitConverter.GetBytes(aes.IV.Length), 0,
                        sizeof(int));
                    msEncrypt.Write(aes.IV, 0, aes.IV.Length);

                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (var swEncrypt = new StreamWriter(csEncrypt)) {
                        swEncrypt.Write(plaintext);
                    }

                    result = Convert.ToBase64String(msEncrypt.ToArray());
                }
            } finally {
                if (aes != null) {
                    aes.Clear();
                }
            }

            return result;
        }

        public static string Decrypt(string cipher) => Decrypt(cipher, Key, SaltBytes);

        private static string Decrypt(string cipher, string key, byte[] saltBytes)
        {
            if (cipher == null || cipher.IsEmpty()) {
                return "";
            }

            var plaintext = string.Empty;
            RijndaelManaged aesAlg = null;

            try {
                var derivedKey = new Rfc2898DeriveBytes(key, saltBytes);

                byte[] bytes = Convert.FromBase64String(cipher);
                using (MemoryStream msDecrypt = new MemoryStream(bytes)) {
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = derivedKey.GetBytes(aesAlg.KeySize / 8);

                    aesAlg.IV = ReadByteArray(msDecrypt);

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key,
                        aesAlg.IV);

                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor,
                        CryptoStreamMode.Read))
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt)) {
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            } finally {
                if (aesAlg != null) {
                    aesAlg.Clear();
                }
            }

            return plaintext;
        }

        public static string Rekey(string cipher, string newKey, string newSalt)
        {
            byte[] saltBytes = Encoding.ASCII.GetBytes(newSalt);
            string plaintext = Decrypt(cipher);

            return Encrypt(plaintext, newKey, saltBytes);
        }

        private static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length) {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length) {
                throw new SystemException("Did not read byte array properly");
            }

            return buffer;
        }

        public static string Md5(string message)
        {
            byte[] digest = null;

            using (var md5 = MD5.Create()) {
                digest = md5.ComputeHash(Encoding.UTF8.GetBytes(message));
            }

            return BitConverter.ToString(digest).Replace("-", "").ToLower();
        }
        
        /// <summary>
        /// Generates a cryptographically secure random number of the specified 
        /// length. 
        /// </summary>
        /// <param name="length">The length, in bytes, of the secure random number
        /// generated</param>
        /// <returns>Returns the secure random number as a base 64 encoded string.</returns>
        public static string GenerateSecureRandomString(int length = 24)
        {
            var bytes = new byte[length];

            using (var rng = new RNGCryptoServiceProvider()) {
                rng.GetBytes(bytes);
            }

            return Convert.ToBase64String(bytes);
        }

#region Copyright notice for hashing code

        /* 
         * Password Hashing With PBKDF2 (http://crackstation.net/hashing-security.htm).
         * Copyright (c) 2013, Taylor Hornby
         * All rights reserved.
         *
         * Redistribution and use in source and binary forms, with or without 
         * modification, are permitted provided that the following conditions are met:
         *
         * 1. Redistributions of source code must retain the above copyright notice, 
         * this list of conditions and the following disclaimer.
         *
         * 2. Redistributions in binary form must reproduce the above copyright notice,
         * this list of conditions and the following disclaimer in the documentation 
         * and/or other materials provided with the distribution.
         *
         * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
         * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
         * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
         * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE 
         * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
         * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
         * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
         * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
         * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
         * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
         * POSSIBILITY OF SUCH DAMAGE.
         */

#endregion

#region Hashing Constants

        public const int SaltByteSize = 24;
        public const int HashByteSize = 32;
        public const int Pbkdf2Iterations = 1000;

        public const int IterationIndex = 0;
        public const int SaltIndex = 1;
        public const int Pbkdf2Index = 2;

#endregion

        /// <summary>
        /// Creates a salted PBKDF2 hash of the password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>The hash of the password.</returns>
        public static string CreateHash(string password)
        {
            var salt = new byte[SaltByteSize];

            using (var rng = new RNGCryptoServiceProvider()) {
                rng.GetBytes(salt);
            }

            byte[] hash = PBKDF2(password, salt, Pbkdf2Iterations, HashByteSize);

            return string.Format("{0}:{1}:{2}", Pbkdf2Iterations,
                Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        /// <summary>
        /// Validates a password given a hash of the correct one.
        /// </summary>
        /// <param name="password">The password to check.</param>
        /// <param name="correctHash">A hash of the correct password.</param>
        /// <returns>True if the password is correct. False otherwise.</returns>
        public static bool ValidatePassword(string password, string correctHash)
        {
            if (password.IsEmpty() || correctHash.IsEmpty()) {
                return false;
            }

            // Extract the parameters from the hash
            string[] split = correctHash.Split(':');
            var iterations = int.Parse(split[IterationIndex]);
            byte[] salt = Convert.FromBase64String(split[SaltIndex]);
            byte[] hash = Convert.FromBase64String(split[Pbkdf2Index]);

            byte[] testHash = PBKDF2(password, salt, iterations, hash.Length);
            return SlowEquals(hash, testHash);
        }

        /// <summary>
        /// Compares two byte arrays in length-constant time. This comparison
        /// method is used so that password hashes cannot be extracted from
        /// on-line systems using a timing attack and then attacked off-line.
        /// </summary>
        /// <param name="a">The first byte array.</param>
        /// <param name="b">The second byte array.</param>
        /// <returns>True if both byte arrays are equal. False otherwise.</returns>
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            var diff = (uint)a.Length ^ (uint)b.Length;

            for (int i = 0; i < a.Length && i < b.Length; i++) {
                diff |= (uint)(a[i] ^ b[i]);
            }

            return diff == 0;
        }

        /// <summary>
        /// Computes the PBKDF2-SHA1 hash of a password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="iter">The PBKDF2 iteration count.</param>
        /// <param name="outputBytes">The length of the hash to generate, in bytes.</param>
        /// <returns>A hash of the password.</returns>
        private static byte[] PBKDF2(string password, byte[] salt, int iter, int outputBytes)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt)) {
                pbkdf2.IterationCount = iter;

                return pbkdf2.GetBytes(outputBytes);
            }
        }
    }
}
#endif