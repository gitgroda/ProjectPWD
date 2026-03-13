using System;
using System.IO;
using System.Text.Json;
using Microsoft.VisualBasic;

namespace Kod
{
    public class Server
    {

        public byte [] iv {get; set;}

        public string vault {get; set;}
        
    
    public Server(string vault, byte[] iv)
    {
        this.vault = vault;
        this.iv = iv;
    }    


    }
}