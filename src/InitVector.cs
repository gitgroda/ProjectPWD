using System;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;

public class InitVector
{
    public byte[] iv {get; set;} 


    public InitVector()
    {
        using (Aes aes = Aes.Create())
        {
            iv = aes.IV;
                    
        }
    }
}
