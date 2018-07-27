
using UnityEngine;
using System;
using System.Collections.Generic;
using SimpleJSON;
using CastleDBImporter;
namespace CastleDBCompiledTypes
{ 
    public class itemAbilites
    {
        public string id;
public string effect;

         
        public itemAbilites (SimpleJSON.JSONNode node) 
        {
            id = node["id"];
effect = node["effect"];

        }  
    }
}