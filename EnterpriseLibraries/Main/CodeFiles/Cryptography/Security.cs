using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace LogixHealth.EnterpriseLibrary.Cryptography
{
    public class Security
    {
        private const string _initialVector = "RFrna73m*az@01xY";
        private const string _hashAlgorith = "SHA1";
        private const string _salt = "LogixHealth2.0";
        private const int _passwordIterations = 2;
        private const int _keySize = 256;
        private const string _kryptoPassoword = "qX#na73m*az@01xY";

        #region static methods
        public static string AESEncrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return "";

            byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(_initialVector);
            byte[] SaltValueBytes = Encoding.ASCII.GetBytes(_salt);
            byte[] PlainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(_kryptoPassoword, SaltValueBytes, _hashAlgorith, _passwordIterations);
            byte[] KeyBytes = DerivedPassword.GetBytes(_keySize / 8);
            RijndaelManaged SymmetricKey = new RijndaelManaged
            {
                Mode = CipherMode.CBC
            };

            byte[] cipherTextBytes = null;

            using (ICryptoTransform Encryptor = SymmetricKey.CreateEncryptor(KeyBytes, InitialVectorBytes))
            {
                using (MemoryStream MemStream = new MemoryStream())
                {
                    using (CryptoStream CryptoStream = new CryptoStream(MemStream, Encryptor, CryptoStreamMode.Write))
                    {
                        CryptoStream.Write(PlainTextBytes, 0, PlainTextBytes.Length);
                        CryptoStream.FlushFinalBlock();
                        cipherTextBytes = MemStream.ToArray();
                        MemStream.Close();
                        CryptoStream.Close();
                    }
                }
            }

            SymmetricKey.Clear();

            return Convert.ToBase64String(cipherTextBytes);

        }

        public static string AESDecrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return "";

            byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(_initialVector);
            byte[] SaltValueBytes = Encoding.ASCII.GetBytes(_salt);
            byte[] CipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(_kryptoPassoword, SaltValueBytes, _hashAlgorith, _passwordIterations);
            byte[] KeyBytes = DerivedPassword.GetBytes(_keySize / 8);
            RijndaelManaged SymmetricKey = new RijndaelManaged
            {
                Mode = CipherMode.CBC
            };

            byte[] plainTextBytes = new byte[CipherTextBytes.Length];
            int ByteCount = 0;

            using (ICryptoTransform Decryptor = SymmetricKey.CreateDecryptor(KeyBytes, InitialVectorBytes))
            {
                using (MemoryStream MemStream = new MemoryStream(CipherTextBytes))
                {
                    using (CryptoStream CryptoStream = new CryptoStream(MemStream, Decryptor, CryptoStreamMode.Read))
                    {
                        ByteCount = CryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                        MemStream.Close();
                        CryptoStream.Close();
                    }
                }
            }

            SymmetricKey.Clear();

            return Encoding.UTF8.GetString(plainTextBytes, 0, ByteCount);
        }

        #endregion

        public string HashText(string plainText, string salt)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                return string.Empty;
            }

            var text = plainText + salt;
            byte[] textHash;

            using (var hashAlgorithm = new SHA1Managed())
            {
                var textBytes = Encoding.ASCII.GetBytes(text);
                textHash = hashAlgorithm.ComputeHash(textBytes);
            }

            return BitConverter.ToString(textHash);
        }

        public string GenerateSalt()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        public bool ValidatePasswordStrength(string password)
        {
            bool isValid = false;
            var spaceRegex = new Regex(@"[\s]");

            if (!string.IsNullOrEmpty(password) && password.Length >= 8 && !spaceRegex.IsMatch(password))
            {
                var upperRegex = new Regex("[A-Z]");
                var lowerRegex = new Regex("[a-z]");
                var digitRegex = new Regex("[0-9]");
                var specialChars = new char[] { '~', '!', '@', '#', '$', '%', '^', '&', '*', '_', '-', '+', '=', '`', '|', '\\',
                    '(', ')', '{', '}', '[', ']', ':', ';', '"', '\'', '<', '>', ',', '.', '?', '/'};
                var maxAsciiCode = 127;
                int score = 0;

                if (upperRegex.IsMatch(password))
                {
                    score++;
                }

                if (lowerRegex.IsMatch(password))
                {
                    score++;
                }

                if (digitRegex.IsMatch(password))
                {
                    score++;
                }

                if (password.IndexOfAny(specialChars) >= 0)
                {
                    score++;
                }

                if (password.Any(c => c > maxAsciiCode))
                {
                    score++;
                }

                if (score >= 3)
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        public string GeneratePassword()
        {
            return new PasswordGenerator().GenerateRandomPassword();
        }
    }
}
