using System;
using System.Collections.Generic;
using System.Text;

namespace LogixHealth.EnterpriseLibrary.Cryptography
{
    public class PasswordGenerator
    {
        private string CreateRandomPassword(int passwordLength, string allowedChars)
        {
            char[] chars = new char[passwordLength];

            Random rd = new Random();

            for (int i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        public string GenerateRandomPassword()
        {
            StringBuilder sb = new StringBuilder();
            string allowedChars;
            List<string> pwdchar = new List<string>();
            List<int> numbersPresent = new List<int>();

            Random randomNumber = new Random();
            string pwd = string.Empty;
            int number = 0;

            string allowedChars1 = "abcdefghijkmnopqrstuvwxyz";
            string allowedChars2 = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
            string allowedChars3 = "!@$?_-";
            string allowedChars4 = "0123456789";

            pwdchar.Add(allowedChars1);
            pwdchar.Add(allowedChars2);
            pwdchar.Add(allowedChars3);
            pwdchar.Add(allowedChars4);

            number = randomNumber.Next(0, 3);
            allowedChars = pwdchar[number];
            pwdchar.Remove(pwdchar[number]);

            sb.Append(CreateRandomPassword(3, allowedChars));

            foreach (var item in pwdchar)
            {
                for (int i = 0; i < 2; i++)
                {
                    number = randomNumber.Next(item.Length - 1);
                    sb.Append(item[number]);
                }
            }
            return sb.ToString();

        }

    }
}