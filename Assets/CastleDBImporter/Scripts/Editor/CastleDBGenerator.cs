using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;


namespace CastleDBImporter
{
    public class CastleDBGenerator
    {

        public static void GenerateTypes(CastleDBParser.RootNode root, CastleDBConfig configFile)
        {
            // Create scripts
            List<string> scripts = new List<string>();
            CastleDBConfig config = configFile;

            InitTypePath(config);

            foreach (CastleDBParser.SheetNode sheet in root.Sheets)
            {
                string scriptPath = $"Assets/{config.GeneratedTypesLocation}/{sheet.Name}.cs";
                scripts.Add(scriptPath);

                //generate fields
                string fieldText = "";
                for (int i = 0; i < sheet.Columns.Count; i++)
                {
                    CastleDBParser.ColumnNode column = sheet.Columns[i];
                    string fieldType = CastleDBUtils.GetTypeFromCastleDBColumn(column);
                    if(fieldType != "Enum") //non-enum, normal field
                    {
                        if(CastleDBUtils.GetTypeNumFromCastleDBTypeString(column.TypeStr) == "8")
                        {
                            fieldText += ($"public List<{fieldType}> {column.Name}List = new List<{fieldType}>();\n\t\t");
                        }
                        else
                        {
                            fieldText += ($"public {fieldType} {column.Name};\n\t\t");
                        }
                    }
                    else //enum type
                    {
                        string[] enumValueNames = CastleDBUtils.GetEnumValuesFromTypeString(column.TypeStr);
                        string enumEntries = "";
                        if(CastleDBUtils.GetTypeNumFromCastleDBTypeString(column.TypeStr) == "10") //flag
                        {
                            fieldText += ($"public {column.Name}Flag {column.Name};\n");
                            for (int val = 0; val < enumValueNames.Length; val++)
                            {
                                enumEntries += (enumValueNames[val] + " = " + (int)Math.Pow(2, val));
                                if(val + 1 < enumValueNames.Length) {enumEntries += ",";};
                            }
                            fieldText += ($"[FlagsAttribute] public enum {column.Name}Flag {{ {enumEntries} }}");
                        }
                        else
                        {
                            fieldText += ($"public {column.Name}Enum {column.Name};\n");
                            for (int val = 0; val < enumValueNames.Length; val++)
                            {
                                enumEntries += (enumValueNames[val] + " = " + val);
                                if(val + 1 < enumValueNames.Length) {enumEntries += ",";};
                            }
                            fieldText += ($"public enum {column.Name}Enum {{  {enumEntries} }}");
                        }
                    }
                }

                //generate the constructor that sets the fields based on the passed in value
                string constructorText = "";

                for (int i = 0; i < sheet.Columns.Count; i++)
                {
                    CastleDBParser.ColumnNode column = sheet.Columns[i];
                    string castText = CastleDBUtils.GetCastStringFromCastleDBTypeStr(column.TypeStr);
                    string enumCast = "";
                    string typeNum = CastleDBUtils.GetTypeNumFromCastleDBTypeString(column.TypeStr);
                    if(typeNum == "8")
                    {
                        //list type
                        constructorText += $"foreach(var item in node[\"{column.Name}\"]) {{ {column.Name}List.Add(new {column.Name}(root, item));}}\n";
                    }
                    else if(typeNum == "6")
                    {
                        //working area:
                        //ref type
                        string refType = CastleDBUtils.GetTypeFromCastleDBColumn(column);
                        //look up the line based on the passed in row
                        constructorText += $"{column.Name} = new {config.GeneratedTypesNamespace}.{refType}(root,{config.GeneratedTypesNamespace}.{refType}.GetRowValue(node[\"{column.Name}\"]));\n";
                    }
                    else
                    {
                        if(typeNum == "10")
                        {
                            enumCast = $"({column.Name}Flag)";
                        }
                        else if(typeNum == "5")
                        {
                            enumCast = $"({column.Name}Enum)";
                        }
                        constructorText += $"\t\t\t{column.Name} = {enumCast}node[\"{column.Name}\"]{castText};\n";
                    }
                }
                             
                string ctor = "";
                ctor = $"public {sheet.Name} (CastleDBParser.RootNode root, SimpleJSON.JSONNode node)";
                string fullClassText = $@"
using UnityEngine;
using System;
using System.Collections.Generic;
using SimpleJSON;
using CastleDBImporter;
namespace {config.GeneratedTypesNamespace}
{{ 
    public class {sheet.Name}
    {{
        {fieldText}
        {ctor} 
        {{
            {constructorText}
        }} 
    }}
}}";
                Debug.Log("Generating CDB Class: " + sheet.Name);
                File.WriteAllText(scriptPath, fullClassText);
            }

            //build the CastleDB file
            string cdbscriptPath = $"Assets/{config.GeneratedTypesLocation}/CastleDB.cs";
            scripts.Add(cdbscriptPath);
            //fields
            string cdbfields = "";
            string cdbconstructorBody = "";
            foreach (CastleDBParser.SheetNode sheet in root.Sheets)
            {
                if(sheet.NestedType){continue;} //only write main types to CastleDB
                cdbfields += $"public Dictionary<string, {sheet.Name}> {sheet.Name}s;\n";
                cdbconstructorBody += $"{sheet.Name}s = new Dictionary<string, {sheet.Name}>();\n";                

                //get a list of all the row names
                cdbconstructorBody += $"\t\t\tforeach( var row in root.GetSheetWithName(\"{sheet.Name}\").Rows ) {{\n";
                cdbconstructorBody += $"\t\t\t\t{sheet.Name}s[row[\"id\"]] = new {sheet.Name}(root, row);\n\t\t\t}}";          
            }

            string fullCastle = $@"
using UnityEngine;
using CastleDBImporter;
using System.Collections.Generic;
using System;

namespace {config.GeneratedTypesNamespace}
{{
    public class CastleDB
    {{
        static CastleDBParser parsedDB;
        {cdbfields}
        public CastleDB(TextAsset castleDBAsset)
        {{
            parsedDB = new CastleDBParser(castleDBAsset);
            CastleDBParser.RootNode root = parsedDB.Root;
            { cdbconstructorBody }
        }}
    }}
}}";

            Debug.Log("Generating CastleDB class");
            File.WriteAllText(cdbscriptPath, fullCastle);
            AssetDatabase.Refresh();

        }

        public static void InitTypePath(CastleDBConfig config)
        {
            var path = $"{Application.dataPath}/{config.GeneratedTypesLocation}";
            if (Directory.Exists (path))
            {
                //we've generated this before, so delete the assets in the folder and refresh the DB
                var files = Directory.GetFiles(path);
                foreach (var file in files)
                {
                    FileUtil.DeleteFileOrDirectory(file);
                }
                AssetDatabase.Refresh();
            }
            else
            {
                Directory.CreateDirectory(path);
                AssetDatabase.Refresh();
            }
        }
    }
}
