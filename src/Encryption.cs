using System;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;

public class Encryption
{

   
    public string Serialize<T>(T data) //serialiserar objekt
    {
        return(JsonSerializer.Serialize(data));

    }

    public void Encrypt()
    {
        

        
    }
  
}
