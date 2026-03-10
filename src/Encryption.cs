using System;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

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


    public byte[] EncryptVault(string plainVault, byte[] key, byte[] iv)
    {
        using (Aes aes = Aes.Create())
        {
            ICryptoTransform encryptor = aes.CreateEncryptor();

            using (MemoryStream ms = new MemoryStream())
        {
            using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                using (StreamWriter sw = new StreamWriter(cs))
                {
                    sw.Write(plainVault); 
                }
            }
            
            return ms.ToArray();
        }

        
        } 
    
       
    

    }


}
