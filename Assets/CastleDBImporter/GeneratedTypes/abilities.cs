using UnityEngine;
 using System;
 using System.Collections.Generic;
 using SimpleJSON;
 using CastleDBImporter;
 public class abilities 
 { 
 public string id;
public string realname;
public bool boolValueSheet2;
 
 public enum abilitiesvalues { 
ability1, 
abilit2
 } public abilities (CastleDB.RootNode root, abilitiesvalues line) { 
 SimpleJSON.JSONNode node = root.GetSheetWithName("abilities").Rows[(int)line];
id = node["id"];
realname = node["realname"];
boolValueSheet2 = node["boolValueSheet2"].AsBool;
 
 }  }