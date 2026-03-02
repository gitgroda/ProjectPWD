using System;
using System.Text.Json;
using Microsoft.Win32.SafeHandles;

namespace Project
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Users\alfre\OneDrive - Uppsala universitet\SKOLA\Information och IT-säkerhet\Kod\client.json";
            var enc = new Encryption();
            var client = new Client(path);


            Secret secret = new Secret(enc.CreateSecret());
            
            
            client.Write(enc.Serialize(secret));

    
        }
    }
}
