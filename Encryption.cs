using System;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;

public class Encryption
{

    public byte[] CreateSecret()
    { 
        byte [] secret = new byte[16];
        return(RandomNumberGenerator.GetBytes(16));
    }
    public string Serialize(Secret secret)
        {
            return(JsonSerializer.Serialize(secret));

        }
}
