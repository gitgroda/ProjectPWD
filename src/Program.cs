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
    class Program
    {
        static void Main(string[] args)
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

                    var initVault = user.CreateVault();

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
                            System.Console.WriteLine("ERROR: 'create' requires path to client and server file");
                            return;
                        }
                        
                        var createEnc = new Encryption();
                        
                        System.Console.WriteLine("Input secret key:");
                        
                        try
                        {
                            byte[] createSecretKey = Convert.FromBase64String(Console.ReadLine());
                            System.Console.WriteLine("Input master password");
                            createEnc.vaultKey = createEnc.Derive(Console.ReadLine(), createSecretKey); 
                        
                            Server createServer = createEnc.ServerRead(args[2]);

                            string temp = createEnc.DecryptVault(createServer.vault, createEnc.vaultKey, createServer.iv);
                            if(temp == null)
                            {
                                System.Console.WriteLine("ERROR: Wrong password or secret key");
                                return;
                            }                        
                            var createSecret = new Secret(createSecretKey);

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
                            Client getClient = getEnc.ClientRead(args[1]);
                            Server getServer = getEnc.ServerRead(args[2]);

                            System.Console.WriteLine("Input master password");
                            getEnc.vaultKey = getEnc.Derive(Console.ReadLine(), Convert.FromBase64String(getClient.secretKey)); 

                            string getPlainText = getEnc.DecryptVault(getServer.vault, getEnc.vaultKey, getServer.iv);

                            Vault getVault = JsonSerializer.Deserialize<Vault>(getPlainText);

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
                        catch (Exception ex)
                        {
                            System.Console.WriteLine("error" + ex.Message);
                        }
                    }
                break;

                case "set":
                    {

                        if(args.Length < 3)
                        {
                            System.Console.WriteLine("ERROR: 'set' requires path to client and server file");
                        }
                        string setPropKey;
                        string generatedkey = "";
                        string inputKey = "";

                        var setEnc = new Encryption();

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
                            Client setClient = setEnc.ClientRead(args[1]);
                            Server setServer = setEnc.ServerRead(args[2]);

                            System.Console.WriteLine("Enter master password");
                            setEnc.vaultKey = setEnc.Derive(Console.ReadLine(), Convert.FromBase64String(setClient.secretKey)); 

                            string setPlainText = setEnc.DecryptVault(setServer.vault, setEnc.vaultKey, setServer.iv);

                            Vault setVault = JsonSerializer.Deserialize<Vault>(setPlainText);

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
                            Client deleteClient = deleteEnc.ClientRead(args[1]);
                            Server deleteServer = deleteEnc.ServerRead(args[2]);

                            System.Console.WriteLine("Input master password:");
                            deleteEnc.vaultKey = deleteEnc.Derive(Console.ReadLine(), Convert.FromBase64String(deleteClient.secretKey));

                            string deletePlainText = deleteEnc.DecryptVault(deleteServer.vault, deleteEnc.vaultKey, deleteServer.iv);

                            Vault deleteVault = JsonSerializer.Deserialize<Vault>(deletePlainText);

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
                            System.Console.WriteLine("Secret key: " + secretClient.secretKey);

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
