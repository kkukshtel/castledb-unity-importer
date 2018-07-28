
using UnityEngine;
using CastleDBImporter;
using System.Collections.Generic;
using System;

namespace CompiledTypes
{
    public class CastleDB
    {
        static CastleDBParser parsedDB;
        public unityTestType unityTest;
public abilitiesType abilities;

        public CastleDB(TextAsset castleDBAsset)
        {
            parsedDB = new CastleDBParser(castleDBAsset);
            unityTest = new unityTestType();abilities = new abilitiesType();
        }
        public class unityTestType 
 {public unityTest unityTestsampleRow { get { return Get(CompiledTypes.unityTest.RowValues.unityTestsampleRow); } } 
public unityTest sampleRow2 { get { return Get(CompiledTypes.unityTest.RowValues.sampleRow2); } } 
public unityTest myNewRow { get { return Get(CompiledTypes.unityTest.RowValues.myNewRow); } } 
public unityTest doesthisbreak { get { return Get(CompiledTypes.unityTest.RowValues.doesthisbreak); } } 
public unityTest anotherbreakingtest { get { return Get(CompiledTypes.unityTest.RowValues.anotherbreakingtest); } } 
public unityTest rebecca { get { return Get(CompiledTypes.unityTest.RowValues.rebecca); } } 
public unityTest asmdeffiles { get { return Get(CompiledTypes.unityTest.RowValues.asmdeffiles); } } 
private unityTest Get(CompiledTypes.unityTest.RowValues line) { return new unityTest(parsedDB.Root, line); }

                public unityTest[] GetAll() 
                {
                    var values = (CompiledTypes.unityTest.RowValues[])Enum.GetValues(typeof(CompiledTypes.unityTest.RowValues));
                    unityTest[] returnList = new unityTest[values.Length];
                    for (int i = 0; i < values.Length; i++)
                    {
                        returnList[i] = Get(values[i]);
                    }
                    return returnList;
                }
 } //END OF unityTest 
public class abilitiesType 
 {public abilities ability1 { get { return Get(CompiledTypes.abilities.RowValues.ability1); } } 
public abilities abilit2 { get { return Get(CompiledTypes.abilities.RowValues.abilit2); } } 
private abilities Get(CompiledTypes.abilities.RowValues line) { return new abilities(parsedDB.Root, line); }

                public abilities[] GetAll() 
                {
                    var values = (CompiledTypes.abilities.RowValues[])Enum.GetValues(typeof(CompiledTypes.abilities.RowValues));
                    abilities[] returnList = new abilities[values.Length];
                    for (int i = 0; i < values.Length; i++)
                    {
                        returnList[i] = Get(values[i]);
                    }
                    return returnList;
                }
 } //END OF abilities 

    }
}