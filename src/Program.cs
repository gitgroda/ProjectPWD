using System;
using System.Text.Json;
using Microsoft.Win32.SafeHandles;
using System.Security.Cryptography;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics;
using System.IO;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace Kod
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
                {
                System.Console.WriteLine("ERROR: No commands entered. See manual for available commands or run program with 'help'.");
                return;
                }

           switch (args[0])
            {
                case "init":
                

                if (args.Length < 3)
                    {
                        System.Console.WriteLine("ERROR: 'init' requires path to client and server file");
                        return;
                    }

                var client = new Client(args[1]);            //Sätta paths
                var server = new Server(args[2]); 
                
                var user = new User();                           //ta in password
                

                var secret = new Secret();
                var enc = new Encryption(); 
                var vector = new InitVector();

                System.Console.WriteLine("Secret key: " + secret.secretKey);

                client.Write(enc.Serialize(secret));
                server.Write(enc.Serialize(vector));

                enc.vaultKey = enc.Derive(user.getData(), secret.secretKey);



                break;

                case "create":

                break;

                case "get":

                break;

                case "set":

                break;

                case "delete":

                break;

                case "secret":

                break;

                case "change":

                break;
            }
        
        
            
 
            User afga = new User();                         //Skaffa användare, hämta clientPath, serverPath och master password.
            afga.Data();

             




            string serSecret = enc.Serialize(secret);     //serialisera secret key och skriv till client.json
            client.Write(serSecret); 

            string serIV = enc.Serialize(vector);              //serialisera IV och skriv till server.json
            server.Write(serIV);

            
            Vault testVault = afga.CreateVault();
            string serVault = enc.Serialize(testVault);


            

            string encryptVault = enc.Serialize(enc.EncryptVault(serVault, secret.secretKey, vector.iv)); //kryptera och serialisera vault

            System.Console.WriteLine(encryptVault);

        }
    }
}
