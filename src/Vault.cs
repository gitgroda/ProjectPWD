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

    public string Get(string key)
    {
        if (vault.ContainsKey(key))
        {
            return vault[key];
        }
        return "notfound";
    }   
    public void ListProperties()
    {
        if (vault.Count == 0)
        {
            Console.WriteLine("The vault is empty");
            return;
        }
        Console.WriteLine("Stored properties in vault:");

        foreach (var key in vault.Keys)
        {
            Console.WriteLine($"- {key}");
        }

    }
    public bool Delete(string key)
    {
        if (vault.ContainsKey(key))
        {
            vault.Remove(key);
           Console.WriteLine($"Property '{key}' deleted.");
            return true;
        }
        Console.WriteLine($"ERROR: Property '{key}' not found");
        return false;

    }
}