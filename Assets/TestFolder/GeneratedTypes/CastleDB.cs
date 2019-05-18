
using UnityEngine;
using CastleDBImporter;
using System.Collections.Generic;
using System;

namespace CompiledTypes
{
    public class CastleDB
    {
        static CastleDBParser parsedDB;
        public CreaturesType Creatures;
public ItemsType Items;
public ModifiersType Modifiers;

        public CastleDB(TextAsset castleDBAsset)
        {
            parsedDB = new CastleDBParser(castleDBAsset);
            Creatures = new CreaturesType();Items = new ItemsType();Modifiers = new ModifiersType();
        }
        public class CreaturesType 
 {public Creatures Squid { get { return Get(CompiledTypes.Creatures.RowValues.Squid); } } 
public Creatures Bear { get { return Get(CompiledTypes.Creatures.RowValues.Bear); } } 
public Creatures Dragon { get { return Get(CompiledTypes.Creatures.RowValues.Dragon); } } 
private Creatures Get(CompiledTypes.Creatures.RowValues line) { return new Creatures(parsedDB.Root, line); }

                public Creatures[] GetAll() 
                {
                    var values = (CompiledTypes.Creatures.RowValues[])Enum.GetValues(typeof(CompiledTypes.Creatures.RowValues));
                    Creatures[] returnList = new Creatures[values.Length];
                    for (int i = 0; i < values.Length; i++)
                    {
                        returnList[i] = Get(values[i]);
                    }
                    return returnList;
                }
 } //END OF Creatures 
public class ItemsType 
 {public Items HealingPotion { get { return Get(CompiledTypes.Items.RowValues.HealingPotion); } } 
public Items PoisonPotion { get { return Get(CompiledTypes.Items.RowValues.PoisonPotion); } } 
public Items UltraSword { get { return Get(CompiledTypes.Items.RowValues.UltraSword); } } 
private Items Get(CompiledTypes.Items.RowValues line) { return new Items(parsedDB.Root, line); }

                public Items[] GetAll() 
                {
                    var values = (CompiledTypes.Items.RowValues[])Enum.GetValues(typeof(CompiledTypes.Items.RowValues));
                    Items[] returnList = new Items[values.Length];
                    for (int i = 0; i < values.Length; i++)
                    {
                        returnList[i] = Get(values[i]);
                    }
                    return returnList;
                }
 } //END OF Items 
public class ModifiersType 
 {public Modifiers poison { get { return Get(CompiledTypes.Modifiers.RowValues.poison); } } 
public Modifiers enchanted { get { return Get(CompiledTypes.Modifiers.RowValues.enchanted); } } 
private Modifiers Get(CompiledTypes.Modifiers.RowValues line) { return new Modifiers(parsedDB.Root, line); }

                public Modifiers[] GetAll() 
                {
                    var values = (CompiledTypes.Modifiers.RowValues[])Enum.GetValues(typeof(CompiledTypes.Modifiers.RowValues));
                    Modifiers[] returnList = new Modifiers[values.Length];
                    for (int i = 0; i < values.Length; i++)
                    {
                        returnList[i] = Get(values[i]);
                    }
                    return returnList;
                }
 } //END OF Modifiers 


        // Convert CastleDB color string to Unity Color type.
        public static Color GetColorFromString( string color)
        {
            int.TryParse(color, out int icolor);
            float blue = ((icolor >> 0) & 255) / 255.0f;
            float green = ((icolor >> 8) & 255) / 255.0f;
            float red = ((icolor >> 16) & 255) / 255.0f;
            return new Color(red, green, blue);
        }
    }
}