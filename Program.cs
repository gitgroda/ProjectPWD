using System;
using System.Text.Json;
using Microsoft.Win32.SafeHandles;

namespace Kod
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Users\alfre\OneDrive - Uppsala universitet\SKOLA\Information och IT-säkerhet\Kod\client.json";
            var enc = new Encryption();
            var client = new Client(path);

            
            //var secret = enc.Serialize();

            //client.Write(secret);

            Secret secret = new Secret(enc.CreateSecret());
            
            
            client.Write(enc.Serialize(secret));

    
        }
    }
}
