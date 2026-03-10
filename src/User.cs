using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Kod
{
    public class User
    {


        

        public string getData()
        {
            System.Console.WriteLine("Input master password: \n");
            return Console.ReadLine();
   
        }
        public void Data()
        {
            //clientPath = @"C:\Users\alfre\OneDrive - Uppsala universitet\SKOLA\Information och IT-säkerhet\Kod\Client.json";
            
            //serverPath = @"C:\Users\alfre\OneDrive - Uppsala universitet\SKOLA\Information och IT-säkerhet\Kod\server.json";
 

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
