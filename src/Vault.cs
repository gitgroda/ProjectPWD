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

    public Dictionary<string, string> Set(string key, string pass) //Skriv över value om key existerar, annars skapas ny key med pass
    {
        vault[key] = pass;
        return vault;
    }


}