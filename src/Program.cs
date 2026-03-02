using System;
using System.Text.Json;
using Microsoft.Win32.SafeHandles;
using System.Security.Cryptography;

namespace Kod
{
    class Program
    {
        static void Main(string[] args)
        {
           
            var enc = new Encryption();
 
            Secret secret = new Secret(enc.CreateSecret());
            


            User afga = new User(); //Skaffa användare, hämta clientPath, serverPath och master password.
            afga.getData();

            var client = new Client(afga.clientPath); //Sätta paths
            var server = new Server(afga.serverPath);   

           

            client.Write(enc.Serialize(secret)); //serialisera secret key och skriv till client.json
            
            using (Aes aes = Aes.Create())
                {
                     byte[] iv = aes.IV; // 

                }





        }
    }
}
