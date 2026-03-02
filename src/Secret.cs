using System;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.VisualBasic;

public class Secret
{
    public byte[] secret { get; set; }


    public Secret(byte [] secret)
    {
        this.secret = secret;
        
    }


}