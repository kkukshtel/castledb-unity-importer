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
                            fieldText += ($"public List<{fieldType}> {column.Name}List = new List<{fieldType}>();\n");
                        }
                        else
                        {
                            fieldText += ($"public {fieldType} {column.Name};\n");
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
                if(!sheet.NestedType)
                {
                    constructorText += $"SimpleJSON.JSONNode node = root.GetSheetWithName(\"{sheet.Name}\").Rows[(int)line];\n";
                }
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
                        constructorText += $"{column.Name} = {enumCast}node[\"{column.Name}\"]{castText};\n";
                    }
                }

                //need to construct an enum of possible types
                string possibleValuesText = "";
                if(!sheet.NestedType)
                {
                    possibleValuesText += $"public enum RowValues {{ \n";

                    for (int i = 0; i < sheet.Rows.Count; i++)
                    {
                        string rowName = sheet.Rows[i][config.GUIDColumnName];
                        possibleValuesText += rowName;
                        if(i + 1 < sheet.Rows.Count){ possibleValuesText += ", \n";}
                    }
                    possibleValuesText += "\n }";
                }

                string getMethodText = "";
                if(!sheet.NestedType)
                {
                    getMethodText += $@"
public static {sheet.Name}.RowValues GetRowValue(string name)
{{
    var values = (RowValues[])Enum.GetValues(typeof(RowValues));
    for (int i = 0; i < values.Length; i++)
    {{
        if(values[i].ToString() == name)
        {{
            return values[i];
        }}
    }}
    return values[0];
}}";
                }

                string ctor = "";
                if(!sheet.NestedType)
                {
                    ctor = $"public {sheet.Name} (CastleDBParser.RootNode root, RowValues line)";
                }
                else
                {
                    ctor = $"public {sheet.Name} (CastleDBParser.RootNode root, SimpleJSON.JSONNode node)";
                }
                // string usings = "using UnityEngine;\n using System;\n using System.Collections.Generic;\n using SimpleJSON;\n using CastleDBImporter;\n";
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
        {possibleValuesText} 
        {ctor} 
        {{
            {constructorText}
        }}  
        {getMethodText}
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
            string classTexts = "";
            foreach (CastleDBParser.SheetNode sheet in root.Sheets)
            {
                if(sheet.NestedType){continue;} //only write main types to CastleDB
                cdbfields += $"public {sheet.Name}Type {sheet.Name};\n";
                cdbconstructorBody += $"{sheet.Name} = new {sheet.Name}Type();";

                //get a list of all the row names
                classTexts += $"public class {sheet.Name}Type \n {{";
                for (int i = 0; i < sheet.Rows.Count; i++)
                {
                    string rowName = sheet.Rows[i][config.GUIDColumnName];
                    classTexts += $"public {sheet.Name} {rowName} {{ get {{ return Get({config.GeneratedTypesNamespace}.{sheet.Name}.RowValues.{rowName}); }} }} \n";
                }
                classTexts += $"private {sheet.Name} Get({config.GeneratedTypesNamespace}.{sheet.Name}.RowValues line) {{ return new {sheet.Name}(parsedDB.Root, line); }}\n";
                classTexts += $@"
                public {sheet.Name}[] GetAll() 
                {{
                    var values = ({config.GeneratedTypesNamespace}.{sheet.Name}.RowValues[])Enum.GetValues(typeof({config.GeneratedTypesNamespace}.{sheet.Name}.RowValues));
                    {sheet.Name}[] returnList = new {sheet.Name}[values.Length];
                    for (int i = 0; i < values.Length; i++)
                    {{
                        returnList[i] = Get(values[i]);
                    }}
                    return returnList;
                }}";
                classTexts += $"\n }} //END OF {sheet.Name} \n";
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
            {cdbconstructorBody}
        }}
        {classTexts}
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