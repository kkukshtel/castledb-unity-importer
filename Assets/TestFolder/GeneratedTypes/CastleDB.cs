
using UnityEngine;
using CastleDBImporter;
using System.Collections.Generic;
using System;

namespace CompiledTypes
{
    public class CastleDB
    {
        static CastleDBParser parsedDB;
        public Dictionary<string, Creatures> Creatures;
        public Dictionary<string, Items> Items;
        public Dictionary<string, Modifiers> Modifiers;

        public CastleDB(TextAsset castleDBAsset, TextAsset casteDBImagesAsset = null)
        {
            parsedDB = new CastleDBParser(castleDBAsset, casteDBImagesAsset);
            CastleDBParser.RootNode root = parsedDB.Root;
            Creatures = new Dictionary<string, Creatures>();
			foreach( var row in root.GetSheetWithName("Creatures").Rows ) {
				Creatures[row["id"]] = new Creatures(root, row);
			}Items = new Dictionary<string, Items>();
			foreach( var row in root.GetSheetWithName("Items").Rows ) {
				Items[row["id"]] = new Items(root, row);
			}Modifiers = new Dictionary<string, Modifiers>();
			foreach( var row in root.GetSheetWithName("Modifiers").Rows ) {
				Modifiers[row["id"]] = new Modifiers(root, row);
			}
        }
    }
}