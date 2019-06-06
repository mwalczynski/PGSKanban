using System.Security.Cryptography;
using System.Text;

namespace PgsKanban.BusinessLogic.Services
{
    public static class Md5Hasher
    {
        public static string HashString(string text)
        {
                MD5 md5Hash = MD5.Create();

                byte[] inputBytes = Encoding.UTF8.GetBytes(text);

                byte[] hash = md5Hash.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();

                foreach (byte t in hash)
                {
                    sb.Append(t.ToString("x2"));
                }

                return sb.ToString();
        }
    }
}
