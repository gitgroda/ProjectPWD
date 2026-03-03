using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Kod
{
    public class User
    {

        public string serverPath;
        public string clientPath;
        public string masterPwd;

        public void getData()
        {
            System.Console.WriteLine("Input client path: \n");
            clientPath = Console.ReadLine();

            System.Console.WriteLine("Input server path: \n");
            serverPath = Console.ReadLine();

            System.Console.WriteLine("Input master password: \n");
            masterPwd = Console.ReadLine();
   
        }
        public Vault Init()
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
