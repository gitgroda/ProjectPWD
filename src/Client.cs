using System;
using System.IO;
using System.Text.Json;
using Microsoft.VisualBasic;

namespace Kod
{
    public class Client
    {
        public string clientPath;

        public Client(string path)
        {
            this.clientPath = path;

        }
       

        public void Write(string secret)
        {
            if (File.Exists(clientPath))
            {
                File.WriteAllText(clientPath, secret);
                System.Console.WriteLine(clientPath);
            }
            else if (!File.Exists(clientPath))
            {
                System.Console.WriteLine("File does not exist, creating new file...");

                File.WriteAllText(clientPath, secret);
            }
            else
            {
                System.Console.WriteLine("ERROR: Unknown error");
            }
        }
                public static Client create(string path)  //skapa ny clientfil i ett vault som redan finns
        {
            

            if (File.Exists(path))
            {
                Console.WriteLine("Connecting to existing vault...");
            }
            else
            {
                Console.WriteLine("Vault not found. A new file will be created upon first write");
            }
            return new Client(path);
        }
    }
}
