using UnityEngine;
 using System;
 using SimpleJSON;
 using CastleDBImporter;
 public class unityTest2 
 { 
 public System.String id;
public System.String stringValueSheet2;
public System.Boolean boolValueSheet2;
 
 public enum unityTest2values { 
unitytest2SampleRow1, 
unitytest2SampleRow2
 } public unityTest2 (CastleDB.RootNode root, unityTest2values line) { 
 SimpleJSON.JSONNode node = root.GetSheetWithName("unityTest2").Rows[(int)line];
id = node["id"];
stringValueSheet2 = node["stringValueSheet2"];
boolValueSheet2 = node["boolValueSheet2"].AsBool;
 
 }  }