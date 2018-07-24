using UnityEngine;
 using System;
 using System.Collections.Generic;
 using SimpleJSON;
 using CastleDBImporter;
 public class unityTest 
 { 
 public string id;
public string textValue;
public bool booleanValue;
public int myInt;
public List<items> itemsList = new List<items>();
public float floatValue;
public enumValueEnum enumValue;
 public enum enumValueEnum { enum1 = 0,enum2 = 1 }public flagValueFlag flagValue;
[FlagsAttribute] public enum flagValueFlag { oneFlag = 1,twoFlag = 2,threeFlag = 4,fourFlag = 8 }public string colorValue;
public string ability;
public string fileValue;
public string imageValue;
 
 public enum unityTestvalues { 
unityTestsampleRow, 
sampleRow2
 } public unityTest (CastleDB.RootNode root, unityTestvalues line) { 
 SimpleJSON.JSONNode node = root.GetSheetWithName("unityTest").Rows[(int)line];
id = node["id"];
textValue = node["textValue"];
booleanValue = node["booleanValue"].AsBool;
myInt = node["myInt"].AsInt;
foreach(var item in node["items"]) { itemsList.Add(new items(item));}floatValue = node["floatValue"].AsFloat;
enumValue = (enumValueEnum)node["enumValue"].AsInt;
flagValue = (flagValueFlag)node["flagValue"].AsInt;
colorValue = node["colorValue"];
ability = node["ability"];
fileValue = node["fileValue"];
imageValue = node["imageValue"];
 
 }  }