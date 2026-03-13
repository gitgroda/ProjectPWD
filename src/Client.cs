using System;
using System.IO;
using System.Text.Json;
using Microsoft.VisualBasic;

namespace Kod
{
    public class Client
    {
        public string secretKey {get; set;}

        public Client()
        {
            secretKey = string.Empty;
        }

        public Client(string secretKey)
        {
            this.secretKey = secretKey;

        }
    }
}
