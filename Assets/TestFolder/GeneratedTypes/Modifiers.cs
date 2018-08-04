
using UnityEngine;
using System;
using System.Collections.Generic;
using SimpleJSON;
using CastleDBImporter;
namespace CompiledTypes
{ 
    public class Modifiers
    {
        public string id;
public string name;
public int change;

        public enum RowValues { 
poison, 
enchanted
 } 
        public Modifiers (CastleDBParser.RootNode root, RowValues line) 
        {
            SimpleJSON.JSONNode node = root.GetSheetWithName("Modifiers").Rows[(int)line];
id = node["id"];
name = node["name"];
change = node["change"].AsInt;

        }  
        
public static Modifiers.RowValues GetRowValue(string name)
{
    var values = (RowValues[])Enum.GetValues(typeof(RowValues));
    for (int i = 0; i < values.Length; i++)
    {
        if(values[i].ToString() == name)
        {
            return values[i];
        }
    }
    return values[0];
}
    }
}