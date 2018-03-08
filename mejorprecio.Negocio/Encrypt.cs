using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace mejorprecio.Negocio
{
    public static class Encrypt
    {
     
        public static string Encriptar(string cadena)
        {
            byte[] tmpSource; //almacenaran los bytes de origen
            byte[] tmpHash;   //almacenaran el valor de hash resultante

            cadena = cadena + "Lagash2017";
            //Crea una matriz de bytes de los datos de origen
            tmpSource = ASCIIEncoding.ASCII.GetBytes(cadena);
        
            //Calcula hash a partir de los datos de origen
            tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);

            string encriptado = ByteArrayToString(tmpHash);
            //Console.WriteLine(encriptado);
            return encriptado;
        }

        static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);

            for (i = 0; i < arrInput.Length-1;i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }
    }
}