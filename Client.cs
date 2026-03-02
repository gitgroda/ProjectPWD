using System;
using System.IO;
using System.Text.Json;
using Microsoft.VisualBasic;

namespace Kod
{
    public class Client
    {
        string path;

        public Client(string path)
        {
            this.path = path;

        }
       

        public void Write(string secret)
        {
            if (File.Exists(path))
            {
                File.WriteAllText(path, secret);
            }
            else if (!File.Exists(path))
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
