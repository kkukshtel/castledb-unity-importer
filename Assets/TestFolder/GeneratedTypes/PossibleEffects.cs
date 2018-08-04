
using UnityEngine;
using System;
using System.Collections.Generic;
using SimpleJSON;
using CastleDBImporter;
namespace CompiledTypes
{ 
    public class PossibleEffects
    {
        public Modifiers effect;
public int EffectChance;

         
        public PossibleEffects (CastleDBParser.RootNode root, SimpleJSON.JSONNode node) 
        {
            effect = new CompiledTypes.Modifiers(root,CompiledTypes.Modifiers.GetRowValue(node["effect"]));
EffectChance = node["EffectChance"].AsInt;

        }  
        
    }
}