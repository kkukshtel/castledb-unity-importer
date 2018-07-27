
using UnityEngine;
using CastleDBImporter;
using System.Collections.Generic;
using System;

namespace CastleDBCompiledTypes
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
 {public unityTest unityTestsampleRow { get { return Get(CastleDBCompiledTypes.unityTest.RowValues.unityTestsampleRow); } } 
public unityTest sampleRow2 { get { return Get(CastleDBCompiledTypes.unityTest.RowValues.sampleRow2); } } 
private unityTest Get(CastleDBCompiledTypes.unityTest.RowValues line) { return new unityTest(parsedDB.Root, line); }

                public unityTest[] GetAll() 
                {
                    var values = (CastleDBCompiledTypes.unityTest.RowValues[])Enum.GetValues(typeof(CastleDBCompiledTypes.unityTest.RowValues));
                    unityTest[] returnList = new unityTest[values.Length];
                    for (int i = 0; i < values.Length; i++)
                    {
                        returnList[i] = Get(values[i]);
                    }
                    return returnList;
                }
 } //END OF unityTest 
public class abilitiesType 
 {public abilities ability1 { get { return Get(CastleDBCompiledTypes.abilities.RowValues.ability1); } } 
public abilities abilit2 { get { return Get(CastleDBCompiledTypes.abilities.RowValues.abilit2); } } 
private abilities Get(CastleDBCompiledTypes.abilities.RowValues line) { return new abilities(parsedDB.Root, line); }

                public abilities[] GetAll() 
                {
                    var values = (CastleDBCompiledTypes.abilities.RowValues[])Enum.GetValues(typeof(CastleDBCompiledTypes.abilities.RowValues));
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