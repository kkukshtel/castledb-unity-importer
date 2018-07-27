
using UnityEngine;
using System;
using System.Collections.Generic;
using SimpleJSON;
using CastleDBImporter;
namespace CastleDBCompiledTypes
{ 
    public class items
    {
        public string id;
public int itemAttack;
public List<itemAbilites> itemAbilitesList = new List<itemAbilites>();

         
        public items (SimpleJSON.JSONNode node) 
        {
            id = node["id"];
itemAttack = node["itemAttack"].AsInt;
foreach(var item in node["itemAbilites"]) { itemAbilitesList.Add(new itemAbilites(item));}

        }  
    }
}