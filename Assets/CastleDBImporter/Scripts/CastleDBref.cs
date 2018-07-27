// using UnityEngine;
// using CastleDBImporter;
// using System.Collections.Generic;
// using System;

// namespace CastleDBCompiledTypes
// { 
//     public class CastleDB 
//     {
//         // public CDBunityTest unityTest
//         static CastleDBParser parsedDB;
//         public unityTestType unityTest;

//         public CastleDB(TextAsset castleDBAsset)
//         {
//             parsedDB = new CastleDBParser(castleDBAsset);
//             unityTest = new unityTestType();
//         }
//         public class unityTestType
//         {
//             public unityTest sampleRow2 { get { return Get(CastleDBCompiledTypes.unityTest.RowValues.sampleRow2); } }
//             private unityTest Get(CastleDBCompiledTypes.unityTest.RowValues line)
//             {
//                 return new unityTest(parsedDB.Root, line);
//             }

//             public unityTest[] GetAll()
//             {
//                 var values = (CastleDBCompiledTypes.unityTest.RowValues[])Enum.GetValues(typeof(CastleDBCompiledTypes.unityTest.RowValues));
//                 unityTest[] returnList = new unityTest[values.Length];
//                 for (int i = 0; i < values.Length; i++)
//                 {
//                     returnList[i] = Get(values[i]);
//                 }
//                 return returnList;
//             }
//         }
//     }
// }