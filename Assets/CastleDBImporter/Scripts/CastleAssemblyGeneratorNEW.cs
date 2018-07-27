using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;


namespace CastleDBImporter
{
    public class AssembyBuilderExample
    {
        public static void BuildAssembly(CastleDBParser.RootNode root)
        {
            //TODO: need to delete previous dll, then build this one
            //TODO: need to do path management and create a path if it doesnt exsit and allow for a custom path
            var outputAssembly = "Temp/CastleDBAssembly/CastleDBAssembly.dll";
            var assemblyProjectPath = "Assets/CastleDBImporter/CompiledTypes/CastleDBAssembly.dll";

            Directory.CreateDirectory("Temp/CastleDBAssembly");

            // Create scripts
            List<string> scripts = new List<string>();

            InitTypePath();

            foreach (CastleDBParser.SheetNode sheet in root.Sheets)
            {
                // string scriptPath = "Temp/CastleDBAssembly/"+sheet.Name+".cs";
                string scriptPath = "Assets/CastleDBImporter/GeneratedTypes/"+sheet.Name+".cs";
                scripts.Add(scriptPath);
                // string scriptName = Path.GetFileNameWithoutExtension(scriptPath);

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
                    if(typeNum != "8")
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
                    else
                    {
                        constructorText += $"foreach(var item in node[\"{column.Name}\"]) {{ {column.Name}List.Add(new {column.Name}(item));}}\n";
                        // constructorText += $"{column.Name} = new {column.Name}(node[\"{column.Name}\"]);\n";
                    }
                }

                //need to construct an enum of possible types
                //also need to have some utility functions like Type.CreateAllObjects returns list of all objects
                
                string possibleValuesText = "";
                if(!sheet.NestedType)
                {
                    possibleValuesText += $"public enum RowValues {{ \n";
                    // for (int i = 0; i < sheet.Rows.Count; i++)
                    // {
                    //     possibleValuesText += sheet.Rows[i]["id"];
                    //     if(i+1 < sheet.Rows.Count){ possibleValuesText += ", \n";}
                    // }
                    foreach (var name in sheet.RowNames)
                    {
                        possibleValuesText += name;
                        if(sheet.RowNames.IndexOf(name) + 1 < sheet.RowNames.Count){ possibleValuesText += ", \n";}
                    }
                    possibleValuesText += "\n }";
                }

                string ctor = "";
                if(!sheet.NestedType)
                {
                    ctor = $"public {sheet.Name} (CastleDBParser.RootNode root, RowValues line)";
                }
                else
                {
                    ctor = $"public {sheet.Name} (SimpleJSON.JSONNode node)";
                }
                // string usings = "using UnityEngine;\n using System;\n using System.Collections.Generic;\n using SimpleJSON;\n using CastleDBImporter;\n";
                string fullClassText = $@"
using UnityEngine;
using System;
using System.Collections.Generic;
using SimpleJSON;
using CastleDBImporter;
namespace CastleDBCompiledTypes
{{ 
    public class {sheet.Name}
    {{
        {fieldText}
        {possibleValuesText} 
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
            string cdbscriptPath = "Assets/CastleDBImporter/GeneratedTypes/CastleDB.cs";
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
                foreach (var name in sheet.RowNames)
                {
                    // public unityTest sampleRow2 { get { return Get(CastleDBCompiledTypes.unityTest.RowValues.sampleRow2); } }
                    classTexts += $"public {sheet.Name} {name} {{ get {{ return Get(CastleDBCompiledTypes.{sheet.Name}.RowValues.{name}); }} }} \n";
                }

                classTexts += $"private {sheet.Name} Get(CastleDBCompiledTypes.{sheet.Name}.RowValues line) {{ return new {sheet.Name}(parsedDB.Root, line); }}\n";
                classTexts += $@"
                public {sheet.Name}[] GetAll() 
                {{
                    var values = (CastleDBCompiledTypes.{sheet.Name}.RowValues[])Enum.GetValues(typeof(CastleDBCompiledTypes.{sheet.Name}.RowValues));
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

namespace CastleDBCompiledTypes
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



            var assemblyBuilder = new AssemblyBuilder(outputAssembly, scripts.ToArray());

            // Exclude a reference to the copy of the assembly in the Assets folder, if any.
            assemblyBuilder.excludeReferences = new string[] { assemblyProjectPath };
            assemblyBuilder.additionalReferences = new string [] {"Assets/CastleDBImporter/CompiledTypes/SimpleJSON.dll","Assets/CastleDBImporter/CompiledTypes/CastleDB.dll"}; //TODO: need to have this path set dynamically

            //first see if a DLL already exists
            // if(AssetDatabase.DeleteAsset(assemblyProjectPath))
            // {
            //     Debug.Log("deleting asset");
            //     System.Threading.Thread.Sleep(20);
            // }

            // Called on main thread
            assemblyBuilder.buildStarted += delegate(string assemblyPath)
            {
                Debug.LogFormat("Assembly build started for {0}", assemblyPath);
            };

            // Called on main thread
            assemblyBuilder.buildFinished += delegate(string assemblyPath, CompilerMessage[] compilerMessages)
            {
                var errorCount = compilerMessages.Count(m => m.type == CompilerMessageType.Error);
                var warningCount = compilerMessages.Count(m => m.type == CompilerMessageType.Warning);

                Debug.LogFormat("Assembly build finished for {0}", assemblyPath);
                Debug.LogFormat("Warnings: {0} - Errors: {0}", errorCount, warningCount);

                foreach (CompilerMessage message in compilerMessages)
                {
                    if(message.type == CompilerMessageType.Error)
                    {
                        Debug.Log("ERROR: " + message.message);
                    }
                }

                if(errorCount == 0)
                {
                    File.Copy(outputAssembly, assemblyProjectPath, true);
                    AssetDatabase.ImportAsset(assemblyProjectPath);
                }
            };

            // AssemblyReloadEvents.afterAssemblyReload += delegate()
            // {
            //     Debug.Log("assembly reloaded"); 
            // };

            // if(AssetDatabase.LoadMainAssetAtPath(assemblyProjectPath) != null)
            // {
            //     if(AssetDatabase.DeleteAsset(assemblyProjectPath))
            //     {
                    
            //         // Start build of assembly
            //         // if(!assemblyBuilder.Build())
            //         // {
            //         //     Debug.LogErrorFormat("Failed to start build of assembly {0}!", assemblyBuilder.assemblyPath);
            //         //     return;
            //         // }
            //     }
            // }
            // else
            // {
            //     // Start build of assembly
            //     if(!assemblyBuilder.Build())
            //     {
            //         Debug.LogErrorFormat("Failed to start build of assembly {0}!", assemblyBuilder.assemblyPath);
            //         return;
            //     }
            // }

            // Start build of assembly
            // if(!assemblyBuilder.Build())
            // {
            //     Debug.LogErrorFormat("Failed to start build of assembly {0}!", assemblyBuilder.assemblyPath);
            //     return;
            // }

            // while(assemblyBuilder.status != AssemblyBuilderStatus.Finished)
            // {
            //     System.Threading.Thread.Sleep(10);
            // }
            AssetDatabase.Refresh();
        }

        // [MenuItem("CastleDB Importer/Delete Type Folder")]
        // public static void DeleteFolderAssets()
        // {
        //     var path = Application.dataPath + "/CastleDBImporter/GeneratedTypes";
        //     // Verify that the folder exists (may have been already removed).
        //     if (Directory.Exists (path))
        //     {
        //         Debug.Log ("Deleting : " + path);
        //         // Remove dir (recursively)
        //         Directory.Delete(path, true);

        //         // Sync AssetDatabase with the delete operation.
        //         AssetDatabase.DeleteAsset("/Assets/CastleDBImporter/GeneratedTypes");
        //     }

        //     // Refresh the asset database once we're done.
        //     AssetDatabase.Refresh();
        // }

        public static void InitTypePath()
        {
            var path = Application.dataPath + "/CastleDBImporter/GeneratedTypes";
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