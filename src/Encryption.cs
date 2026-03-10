using System;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text;

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
            aes.Key = key;
            aes.IV = iv;

            ICryptoTransform encryptor = aes.CreateEncryptor();

            byte[] plainBytes = Encoding.UTF8.GetBytes(plainVault);

            return encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        
        } 
    }
    public string DecryptVault(string encryptedString, byte[] key, byte[] iv)
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


}
