using System.Security.Cryptography;
using System.Text;

namespace Utilidades
{
    //Clase para encriptar 
    public class Encriptar
    {
        public static string ConvertirSHA256(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder stBldr = new StringBuilder();

            stream = sha256.ComputeHash(encoding.GetBytes(str));

            for (int i = 0; i < stream.Length; i++) stBldr.AppendFormat("{0:x2}", stream[i]);

            return stBldr.ToString();
        }
    }
}