using System;
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
          
    }
    
}
