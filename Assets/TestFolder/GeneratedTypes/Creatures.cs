
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
[FlagsAttribute] public enum SpawnAreasFlag { Forest = 1,Mountains = 2,Lake = 4,Plains = 8 }public Color Color;

        public enum RowValues { 
Squid, 
Bear, 
Dragon
 } 
        public Creatures (CastleDBParser.RootNode root, RowValues line) 
        {
            SimpleJSON.JSONNode node = root.GetSheetWithName("Creatures").Rows[(int)line];
id = node["id"];
Name = node["Name"];
attacksPlayer = node["attacksPlayer"].AsBool;
BaseDamage = node["BaseDamage"].AsInt;
DamageModifier = node["DamageModifier"].AsFloat;
foreach(var item in node["Drops"]) { DropsList.Add(new Drops(root, item));}
DeathSound = (DeathSoundEnum)node["DeathSound"].AsInt;
SpawnAreas = (SpawnAreasFlag)node["SpawnAreas"].AsInt;
Color = CastleDB.GetColorFromString( node["Color"]);

        }  
        
public static Creatures.RowValues GetRowValue(string name)
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