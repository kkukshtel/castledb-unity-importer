using UnityEngine;
 using System;
 using SimpleJSON;
 using CastleDBImporter;
 public class unityTest 
 { 
 public System.String id;
public System.String textValue;
public System.Boolean booleanValue;
public System.Int32 integerValue;
public System.Int32 anotherInt;
public System.Int32 myInt;
public System.Int32 myNewInt;
public System.Single floatValue;
public enumValueEnum enumValue;
 public enum enumValueEnum { enum1 = 0,enum2 = 1 }public anotherenumcolumnEnum anotherenumcolumn;
 public enum anotherenumcolumnEnum { somevalue = 0,anothervalue = 1 }public flagValueFlag flagValue;
[FlagsAttribute] public enum flagValueFlag { oneFlag = 1,twoFlag = 2,threeFlag = 4,fourFlag = 8 }public System.String colorValue;
public System.String fileValue;
public System.String imageValue;
 
 public enum unityTestvalues { 
unityTestsampleRow
 } public unityTest (CastleDB.RootNode root, unityTestvalues line) { 
 SimpleJSON.JSONNode node = root.GetSheetWithName("unityTest").Rows[(int)line];
id = node["id"];
textValue = node["textValue"];
booleanValue = node["booleanValue"].AsBool;
integerValue = node["integerValue"].AsInt;
anotherInt = node["anotherInt"].AsInt;
myInt = node["myInt"].AsInt;
myNewInt = node["myNewInt"].AsInt;
floatValue = node["floatValue"].AsFloat;
enumValue = (enumValueEnum)node["enumValue"].AsInt;
anotherenumcolumn = (anotherenumcolumnEnum)node["anotherenumcolumn"].AsInt;
flagValue = (flagValueFlag)node["flagValue"].AsInt;
colorValue = node["colorValue"];
fileValue = node["fileValue"];
imageValue = node["imageValue"];
 
 }  }