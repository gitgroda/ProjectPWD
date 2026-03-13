using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Kod
{
    public class User
    {
        public string clientPath;
        public string serverPath;


        public string getData()
        {
            System.Console.WriteLine("Input master password: \n");
            return Console.ReadLine();
   
        }
     
        public Vault CreateVault()
        {
            return new Vault();
        }
        public void Create()
        {
            System.Console.WriteLine("Enter new file path:");
            string newPath = Console.ReadLine();
        }
          
    }
   
    
}
