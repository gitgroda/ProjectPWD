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
        secretKey = RandomNumberGenerator.GetBytes(16);
    
    }
}