using System;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.VisualBasic;

public class Secret
{
    public byte[] secretKey { get; set; }

    public Secret()

    {
        secretKey = new byte[16];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(secretKey);

        }
    }
    public Secret(byte[] secretkey)
    {
        this.secretKey = secretkey;
    }
    
}