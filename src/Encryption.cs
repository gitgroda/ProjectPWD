using System;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text;
using Kod;

public class Encryption
{
    public byte[] vaultKey {get; set;}

   
    public string Serialize<T>(T data) //serialiserar objekt
    {
        return(JsonSerializer.Serialize(data));

    }

    public byte[] Derive(string pwd, byte[] secretKey)
    {   
        using(var vaultKey = new Rfc2898DeriveBytes(pwd, secretKey, 600000, HashAlgorithmName.SHA256))
        {
            return vaultKey.GetBytes(32);
        }
    }

    public void Write(string value, string path) //skriver value till path
        {
            if (File.Exists(path))
            {
                File.WriteAllText(path, value);
                System.Console.WriteLine("File created successfully");
            }
            else if (!File.Exists(path))
            {
                System.Console.WriteLine("File does not exist, creating new file...");

                File.WriteAllText(path, value);
            }
            else
            {
                System.Console.WriteLine("ERROR: Unknown error");
            }
        }

    public Server ServerRead(string path)
    {

        Server server = JsonSerializer.Deserialize<Server>(File.ReadAllText(path));
        return server;
        
    }
    public Client ClientRead(string path)
    {
        Client client = JsonSerializer.Deserialize<Client>(File.ReadAllText(path));
        return client;
    }
    public string EncryptVault(string plainVault, byte[] key, byte[] iv)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            ICryptoTransform encryptor = aes.CreateEncryptor();

            try
            {
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainVault);
                byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                return Convert.ToBase64String(encryptedBytes);

            }
            catch (CryptographicException)
            {
                System.Console.WriteLine("ERROR: Wrong master password or secret key");
                return null;
            }
            catch (Exception)
            {
                System.Console.WriteLine("ERROR: Unknown error");
                return null;
            }
            
        } 
    }
    public string DecryptVault(string encryptedString, byte[] key, byte[] iv)
    { 
        try
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedString);

            using (Aes aes = Aes.Create()){

            aes.Key = key;
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor();

                byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                return System.Text.Encoding.UTF8.GetString(decryptedBytes);
            }
        }
            catch 
            {
                return null;
            }
           
   
    }
    public string GeneratePassword()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    
    using (Aes aes = Aes.Create())
    {
        aes.GenerateKey(); // Genererar slumpmässiga bytes
        byte[] randomBytes = aes.Key;
        
        char[] password = new char[20];
        for (int i = 0; i < 20; i++)
        {
            int index = RandomNumberGenerator.GetInt32(chars.Length);
            password[i] = chars[index];
        }
        
        string result = new string(password);
        Console.WriteLine("Generated password: " + result);
        return result;
        
    }
    }
}

