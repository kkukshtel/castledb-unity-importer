
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
		
        public Modifiers (CastleDBParser.RootNode root, SimpleJSON.JSONNode node) 
        {
            			id = node["id"];
			name = node["name"];
			change = node["change"].AsInt;

        } 
    }
}