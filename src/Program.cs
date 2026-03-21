using System;
using System.Text.Json;
using Microsoft.Win32.SafeHandles;
using System.Security.Cryptography;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics;
using System.IO;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics.CodeAnalysis;

namespace Kod
{
    public class Program
    {
        public static void Main(string[] args)      //alla funktioner följer liknande logik som i init, med avsaknad eller tillägg av diverese funktioner. 
                                                    //logiken följer främst; läs client & server, skapa vault key, dekryptera / kryptera vault, 
                                                    //spara vault-string till objekt, serialisera objekt och skriv till server. endast krypterade serverobjekt sparas till server.json
                                                    
        {
            if (args.Length == 0)
                {
                System.Console.WriteLine("ERROR: No commands entered. See manual for available commands or run program with 'help'.");
                return;
                }

           switch (args[0])
            {
                case "init":
                
                if (args.Length < 3)
                    {
                        System.Console.WriteLine("ERROR: 'init' requires path to client and server file");
                        return;
                    }
   
                var user = new User();                           
                var initSecret = new Secret();
                var initEnc = new Encryption(); 
                var initVector = new InitVector();

                System.Console.WriteLine(initEnc.Serialize(initSecret));
                try
                {
                    initEnc.Write(initEnc.Serialize(initSecret), args[1]); //skapa client.json 
                    initEnc.Write(initEnc.Serialize(initSecret), args[2]); //skapa server.json och skriv in IV (överflödigt, kommer skrivas över med server-objektet)

                    initEnc.vaultKey = initEnc.Derive(user.getData(), initSecret.secretKey);        //ta in password och skapa vault key

                    var initVault = user.CreateVault();                         //skapa vault

                    string initEncryptVault = initEnc.EncryptVault(initEnc.Serialize(initVault), initEnc.vaultKey, initVector.iv); //kryptera och serialisera vault

                    initEnc.Write(initEnc.Serialize(new Server(initEncryptVault, initVector.iv)), args[2]); //skapar objekt server, serialiserar och skriver vault till server.json                        
                }
                catch
                {
                    System.Console.WriteLine("ERROR: Unknown error"); 
                }
                



                break;

                case "create":
                    {
                        if (args.Length < 3)
                        {
                            System.Console.WriteLine("ERROR: 'create' requires path to client, server");
                            return;
                        }
                        
                        var createEnc = new Encryption();
                        
                        
                        try
                        {   
                            System.Console.WriteLine("Enter master password");
                            string pwd = Console.ReadLine();
                            System.Console.WriteLine("Enter secret key");
                            byte[] createSecretKey = Convert.FromBase64String(Console.ReadLine());

                            createEnc.vaultKey = createEnc.Derive(pwd, createSecretKey); 

                            var createServer = createEnc.ServerRead(args[2]);

                            string temp = createEnc.DecryptVault(createServer.vault, createEnc.vaultKey, createServer.iv);
                            if(temp == null)
                            {
                                System.Console.WriteLine("ERROR: Wrong password or secret key");
                                return;
                            }                        
                            var createSecret = new Client(Convert.ToBase64String(createSecretKey));

                            createEnc.Write(JsonSerializer.Serialize(createSecret), args[1]);
                            

                        }
                        catch
                        {
                            System.Console.WriteLine("ERROR: Unknown error");
                        }
                    }
                break;


                case "get":
                    {
                         
                        if(args.Length < 3)
                        {
                            System.Console.WriteLine("ERROR: 'get' requires path to client and server file");
                            return;
                        }
                        string getPropKey = "";

                        if(args.Length >= 4)
                        {
                            getPropKey = args[3];
                        }
                    
                             
                        try
                        {
                            var getEnc = new Encryption();
                            var getClient = getEnc.ClientRead(args[1]);
                            var getServer = getEnc.ServerRead(args[2]);

                            System.Console.WriteLine("Input master password");
                            getEnc.vaultKey = getEnc.Derive(Console.ReadLine(), Convert.FromBase64String(getClient.secretKey)); 

                            string getPlainText = getEnc.DecryptVault(getServer.vault, getEnc.vaultKey, getServer.iv);

                            var getVault = JsonSerializer.Deserialize<Vault>(getPlainText);

                            if (getVault.Get(getPropKey) == "notfound")
                            {
                                getVault.ListProperties();
                                return;
                            }
                            else
                            {
                                System.Console.WriteLine(getVault.Get(getPropKey));

                            }
                        }
                        catch (Exception)
                        {
                            System.Console.WriteLine("ERROR: Unknown error");
                        }
                    }
                break;

                case "set":
                    {

                        if(args.Length < 3)
                        {
                            System.Console.WriteLine("ERROR: 'set' requires path to client and server file");
                            return;
                        }
                        string setPropKey;
                        string generatedkey = "";
                        string inputKey = "";

                        var setEnc = new Encryption();
                        System.Console.WriteLine("Enter master password");
                        string pwd = Console.ReadLine();

                        if(args.Length == 5)
                        {
                            setPropKey = args[3];
                            if (args[4] == "-g" || args[4] == "--generate")
                            {
                                generatedkey = setEnc.GeneratePassword();
                            }
                            else
                            {
                                System.Console.WriteLine("ERROR: Wrong syntax, '-g' or '--generate' ");
                                return;
                            }

                        }
                        else if (args.Length == 4)
                        {
                            setPropKey = args[3];
                            System.Console.WriteLine("Enter password to set");
                            inputKey = Console.ReadLine();
                        }
                        else
                        {
                            System.Console.WriteLine("ERROR: Provide prop key");
                            return;
                        }

                        try
                        {
                            var setClient = setEnc.ClientRead(args[1]);
                            var setServer = setEnc.ServerRead(args[2]);

                            setEnc.vaultKey = setEnc.Derive(pwd, Convert.FromBase64String(setClient.secretKey)); 

                            string setPlainText = setEnc.DecryptVault(setServer.vault, setEnc.vaultKey, setServer.iv);

                            var setVault = JsonSerializer.Deserialize<Vault>(setPlainText);

                            if (args.Length == 5)
                            {
                                setVault.vault = setVault.Set(setPropKey, generatedkey);

                                setServer.vault = setEnc.EncryptVault(JsonSerializer.Serialize<Vault>(setVault), setEnc.vaultKey, setServer.iv);
                                 
                                setEnc.Write(JsonSerializer.Serialize<Server>(setServer), args[2]);

                                System.Console.WriteLine("Password generated and saved to vault");

                            }
                            else if (args.Length == 4)
                            {
                                setVault.vault = setVault.Set(setPropKey, inputKey);
                                                                
                                setServer.vault = setEnc.EncryptVault(JsonSerializer.Serialize<Vault>(setVault), setEnc.vaultKey, setServer.iv);

                                setEnc.Write(JsonSerializer.Serialize<Server>(setServer), args[2]);
                                System.Console.WriteLine("Password saved to vault");

                            }

                        }
                        catch
                        {
                            System.Console.WriteLine("ERROR: Unknown error");
                        }
                        
                    }
                break;

                case "delete":
                    {
                        if (args.Length < 3)
                        {
                            System.Console.WriteLine("ERROR: 'delete' requires path to client and server");
                            return;
                        }
                        if (args.Length < 4)
                        {
                            System.Console.WriteLine("ERROR: 'delete' requires a property and key");
                            return;
                        }
                        string deletePropKey = args[3];
                        var deleteEnc = new Encryption();
                        
                        try
                        {
                            var deleteClient = deleteEnc.ClientRead(args[1]);
                            var deleteServer = deleteEnc.ServerRead(args[2]);

                            System.Console.WriteLine("Input master password:");
                            deleteEnc.vaultKey = deleteEnc.Derive(Console.ReadLine(), Convert.FromBase64String(deleteClient.secretKey));

                            string deletePlainText = deleteEnc.DecryptVault(deleteServer.vault, deleteEnc.vaultKey, deleteServer.iv);

                            var deleteVault = JsonSerializer.Deserialize<Vault>(deletePlainText);

                            deleteVault.Delete(deletePropKey);

                            deleteServer.vault = deleteEnc.EncryptVault(JsonSerializer.Serialize<Vault>(deleteVault), deleteEnc.vaultKey, deleteServer.iv);

                            deleteEnc.Write(JsonSerializer.Serialize<Server>(deleteServer), args[2]);

                        }
                        catch
                        {
                            System.Console.WriteLine("ERROR: Decryption failed or propery not found");
                        }                    }
                break;

                case "secret":
                    {
                        if (args.Length < 2)
                        {
                            System.Console.WriteLine("ERROR: 'secret' requires path to client file");
                            return;
                        }

                        try
                        {
                            var secretEnc = new Encryption();
                            Client secretClient = secretEnc.ClientRead(args[1]);
                            System.Console.WriteLine(secretClient.secretKey);

                        }
                        catch
                        {
                            System.Console.WriteLine("ERROR: Could not read client file");
                        }
                    }
                break;

                case "change":
                    {
                        if (args.Length < 3)
                        {
                            System.Console.WriteLine("ERROR: 'change' requires path to client and server file");
                            return;

                        }
                        var changeEnc = new Encryption();
                        
                        try
                        {
                            Client changeClient =  changeEnc.ClientRead(args[1]);
                            Server changeServer = changeEnc.ServerRead(args[2]);

                            System.Console.WriteLine("Input current master password:");
                            changeEnc.vaultKey = changeEnc.Derive(Console.ReadLine(), Convert.FromBase64String(changeClient.secretKey));

                            string changePlainText = changeEnc.DecryptVault(changeServer.vault, changeEnc.vaultKey, changeServer.iv);
                            Vault changeVault = JsonSerializer.Deserialize<Vault>(changePlainText);

                            System.Console.WriteLine("Input new master password:");
                            changeEnc.vaultKey = changeEnc.Derive(Console.ReadLine(), Convert.FromBase64String(changeClient.secretKey));

                            changeServer.vault = changeEnc.EncryptVault(JsonSerializer.Serialize<Vault>(changeVault), changeEnc.vaultKey, changeServer.iv);

                            changeEnc.Write(JsonSerializer.Serialize<Server>(changeServer), args[2]);
                            
                            System.Console.WriteLine("Master password changed successfully");
                        }
                        catch
                        {
                            System.Console.WriteLine("ERROR: Incorrect master password");
                        }
                    }
                break;
            }
        

        }
    }
}
