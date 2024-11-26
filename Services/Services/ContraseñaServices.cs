using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Services
{
    public class ContraseñaServices
    {
        public static string HashearContraseñaConSalt(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convertir la contraseña con Salt en bytes
                byte[] bytes = Encoding.UTF8.GetBytes(password);

                // Calcular el hash
                byte[] hash = sha256.ComputeHash(bytes);

                // Convertir el hash en una cadena hexadecimal
                StringBuilder result = new StringBuilder();
                foreach (byte b in hash)
                {
                    result.Append(b.ToString("x2"));
                }

                return result.ToString(); // Devolver el hash
            }
        }



        private const string CRITERIOS_CARACTERES = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const string CRITERIOS_NUMERICOS = "0123456789";
        private const string CRITERIOS_ESPECIALES = "!\"#$%&'()*+,-./:;=?@[]^_`{|}~";

        public static bool SeguridadContraseña(string password)
        {
            if (password.Length >= 8)
            {
                bool tieneCaracterAlfabetico = false;
                bool tieneNumero = false;
                bool tieneCaracterEspecial = false;


                foreach (char c in password)
                {

                    if (CRITERIOS_CARACTERES.Contains(c))
                    {
                        tieneCaracterAlfabetico = true;
                    }

                    else if (CRITERIOS_NUMERICOS.Contains(c))
                    {
                        tieneNumero = true;
                    }

                    else if (CRITERIOS_ESPECIALES.Contains(c))
                    {
                        tieneCaracterEspecial = true;
                    }

                    if (tieneCaracterAlfabetico && tieneNumero && tieneCaracterEspecial)
                    {
                        return true;
                    }
                }


                return false;
            }

            return false;
        }
    }

}

