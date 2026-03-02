using System;
using System.Text.Json;
using Microsoft.Win32.SafeHandles;
using System.Security.Cryptography;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics;

namespace Kod
{
    class Program
    {
        static void Main(string[] args)
        {
           
            var enc = new Encryption();
 
            User afga = new User();                         //Skaffa användare, hämta clientPath, serverPath och master password.
            afga.getData();

            var client = new Client(afga.clientPath);            //Sätta paths
            var server = new Server(afga.serverPath);   


            Secret secret = new Secret();
            InitVector vector = new InitVector();


            string serSecret = enc.Serialize(secret);     //serialisera secret key och skriv till client.json
            client.Write(serSecret); 

            string serIV = enc.Serialize(vector);              //serialisera IV och skriv till server.json
            server.Write(serIV);

            
            Vault testVault = new Vault();

            string serVault = enc.Serialize(testVault);

            
            

        }
    }
}
