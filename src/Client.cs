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
            }
            else if (!File.Exists(clientPath))
            {
                System.Console.WriteLine("ERROR: File not found");
            }
            else
            {
                System.Console.WriteLine("ERROR: Unknown error");
            }
        }
    }
}
