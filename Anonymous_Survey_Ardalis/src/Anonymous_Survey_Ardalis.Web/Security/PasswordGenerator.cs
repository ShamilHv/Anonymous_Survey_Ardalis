using System.Security.Cryptography;
using System.Text;

namespace Anonymous_Survey_Ardalis.Web.Security;
 public static class PasswordGenerator
    {
        // Define character sets
        private static readonly string LowerCase = "abcdefghijklmnopqrstuvwxyz";
        private static readonly string UpperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static readonly string Numbers = "0123456789";
        private static readonly string Special = "!@#$%^&*()-_=+[]{}|;:,.<>?";

        /// <summary>
        /// Generates a cryptographically secure random password
        /// </summary>
        /// <param name="length">Length of the password (default 12)</param>
        /// <param name="includeSpecial">Include special characters (default true)</param>
        /// <returns>A secure random password</returns>
        public static string GenerateSecurePassword(int length = 12, bool includeSpecial = true)
        {
            // Define character pool based on parameters
            string characterPool = LowerCase + UpperCase + Numbers;
            if (includeSpecial)
            {
                characterPool += Special;
            }

            // Create a byte array to store random values
            byte[] randomBytes = new byte[length * 4];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            // Create password builder
            StringBuilder password = new StringBuilder(length);
            
            // Ensure we have at least one character from each required set
            password.Append(LowerCase[randomBytes[0] % LowerCase.Length]);
            password.Append(UpperCase[randomBytes[1] % UpperCase.Length]);
            password.Append(Numbers[randomBytes[2] % Numbers.Length]);
            
            if (includeSpecial)
            {
                password.Append(Special[randomBytes[3] % Special.Length]);
            }

            // Fill the remaining length with characters from the pool
            for (int i = (includeSpecial ? 4 : 3); i < length; i++)
            {
                int index = randomBytes[i] % characterPool.Length;
                password.Append(characterPool[index]);
            }

            // Shuffle the password
            return ShuffleString(password.ToString(), randomBytes);
        }

        /// <summary>
        /// Shuffles the characters in a string using Fisher-Yates algorithm
        /// </summary>
        private static string ShuffleString(string input, byte[] randomSource)
        {
            char[] chars = input.ToCharArray();
            
            for (int i = chars.Length - 1; i > 0; i--)
            {
                int swapIndex = randomSource[i] % (i + 1);
                if (swapIndex != i)
                {
                    (chars[i], chars[swapIndex]) = (chars[swapIndex], chars[i]);
                }
            }
            
            return new string(chars);
        }
    }

