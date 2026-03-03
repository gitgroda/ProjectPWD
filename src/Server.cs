using System;
using System.IO;
using System.Text.Json;
using Microsoft.VisualBasic;

namespace Kod
{
    public class Server
    {
        public string serverPath;

        public string iv {get; set;}

        
    
    public Server(string path)
    {
        this.serverPath = path;

    }
    public void Write(string secret)
        {
            if (File.Exists(serverPath))
            {
                File.WriteAllText(serverPath, secret);
            }
            else if (!File.Exists(serverPath))
            {
                System.Console.WriteLine("File does not exist, creating new file...");
                File.WriteAllText(serverPath, secret);
            }
            else
            {
                System.Console.WriteLine("ERROR: Unknown error");
            }
        }

    }
}
