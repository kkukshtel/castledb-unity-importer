
using UnityEngine;
using System;
using System.Collections.Generic;
using SimpleJSON;
using CastleDBImporter;
namespace CompiledTypes
{ 
    public class Items
    {
        public string id;
		public string name;
		public int Weight;
		public string image;
		
        public Items (CastleDBParser.RootNode root, SimpleJSON.JSONNode node) 
        {
            			id = node["id"];
			name = node["name"];
			Weight = node["Weight"].AsInt;
			image = node["image"];

        } 
    }
}