using System;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
public class Vault
{
    public Dictionary<string, string> vault {get; set;}

    public Vault()
    {
        vault = new Dictionary<string, string>();
    }


}