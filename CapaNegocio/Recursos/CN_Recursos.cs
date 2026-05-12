using System;
using System.Security.Cryptography;
using System.Text;

namespace CapaNegocio.Recursos
{
    public class CN_Recursos
    {
        public static string ConvertirSha256(string texto)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(texto));

                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public static string GenerarClaveTemporal()
        {
            const string caracteres = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789";
            Random random = new Random();

            char[] clave = new char[8];

            for (int i = 0; i < clave.Length; i++)
            {
                clave[i] = caracteres[random.Next(caracteres.Length)];
            }

            return "LG-" + new string(clave);
        }
    }
}