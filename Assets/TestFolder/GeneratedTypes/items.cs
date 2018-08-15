
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
		public Texture image;
		
        public Items (CastleDBParser.RootNode root, SimpleJSON.JSONNode node, Dictionary<string, Texture> DatabaseImages) 
        {
            			id = node["id"];
			name = node["name"];
			Weight = node["Weight"].AsInt;
			image = DatabaseImages[node["image"]];

        } 
    }
}