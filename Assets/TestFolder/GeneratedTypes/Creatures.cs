
using UnityEngine;
using System;
using System.Collections.Generic;
using SimpleJSON;
using CastleDBImporter;
namespace CompiledTypes
{ 
    public class Creatures
    {
        public string id;
		public string Name;
		public bool attacksPlayer;
		public int BaseDamage;
		public float DamageModifier;
		public List<Drops> DropsList = new List<Drops>();
		public DeathSoundEnum DeathSound;
public enum DeathSoundEnum {  Sound1 = 0,Sound2 = 1 }public SpawnAreasFlag SpawnAreas;
[FlagsAttribute] public enum SpawnAreasFlag { Forest = 1,Mountains = 2,Lake = 4,Plains = 8 }
        public Creatures (CastleDBParser.RootNode root, SimpleJSON.JSONNode node) 
        {
            			id = node["id"];
			Name = node["Name"];
			attacksPlayer = node["attacksPlayer"].AsBool;
			BaseDamage = node["BaseDamage"].AsInt;
			DamageModifier = node["DamageModifier"].AsFloat;
			foreach(var item in node["Drops"]) { DropsList.Add(new Drops(root, item));}
			DeathSound = (DeathSoundEnum)node["DeathSound"].AsInt;
			SpawnAreas = (SpawnAreasFlag)node["SpawnAreas"].AsInt;

        } 
    }
}