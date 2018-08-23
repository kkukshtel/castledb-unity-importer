
using UnityEngine;
using System;
using System.Collections.Generic;
using SimpleJSON;
using CastleDBImporter;
namespace CompiledTypes
{ 
    public class Drops
    {
        public Items item;
		public int DropChance;
		public List<PossibleEffects> PossibleEffectsList = new List<PossibleEffects>();
		
        public Drops (CastleDBParser.RootNode root, SimpleJSON.JSONNode node, Dictionary<string, Texture> DatabaseImages) 
        {
            item = new CompiledTypes.Items(root, root.GetSheetWithName("Items").Rows.Find( pred => pred["id"] == node["item"]), DatabaseImages);
			DropChance = node["DropChance"].AsInt;
			foreach(var item in node["PossibleEffects"]) { PossibleEffectsList.Add(new PossibleEffects(root, item));}

        } 
    }
}