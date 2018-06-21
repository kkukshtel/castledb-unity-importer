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
        public enumff val;
        public enum enumff
        {
            myval
        }
        [MenuItem("CastleDB Importer/Rebuild Assembly")]
        public static void BuildCastleDBAssembly()
        {
            //maybe do something here to load the TextAsset
            // BuildAssembly();
        }

        public static void BuildAssembly(CastleDB.RootNode root)
        {
            var outputAssembly = "Temp/CastleDBAssembly/CastleDBAssembly.dll";
            var assemblyProjectPath = "Assets/CastleDBAssembly.dll";

            Directory.CreateDirectory("Temp/CastleDBAssembly");

            // Create scripts
            List<string> scripts = new List<string>();

            foreach (CastleDB.SheetNode sheet in root.Sheets)
            {
                string scriptPath = "Temp/CastleDBAssembly/"+sheet.Name+".cs";
                scripts.Add(scriptPath);
                string scriptName = Path.GetFileNameWithoutExtension(scriptPath);
                //geneate the fields with type
                string fieldText = "";
                int typeCount = sheet.Columns.Count;
                for (int i = 0; i < typeCount; i++)
                {
                    CastleDB.ColumnNode column = sheet.Columns[i];
                    Type fieldType = CastleDBUtils.GetTypeFromCastleDBTypeStr(column.TypeStr);
                    if(fieldType != typeof(Enum)) //non-enum, normal field
                    {
                        fieldText += ("public " + fieldType.ToString() + " " + column.Name + ";");
                    }
                    else //enum type
                    {
                        string[] enumValueNames = CastleDBUtils.GetEnumValuesFromTypeString(column.TypeStr);
                        string enumEntries = "";
                        string attribute = "";
                        string newEnum = "";
                        if(CastleDBUtils.GetTypeNumFromCastleDBTypeString(column.TypeStr) == "10") //flag
                        {
                            attribute = "[FlagsAttribute]";
                            fieldText += ("public " + column.Name + "Flag" + " " + column.Name + ";");
                            for (int val = 0; val < enumValueNames.Length; val++)
                            {
                                enumEntries += (enumValueNames[val] + " = " + (int)Math.Pow(2, val));
                                if(val + 1 < enumValueNames.Length) {enumEntries += ",";};
                            }
                            newEnum = attribute + " public enum " + column.Name + "Flag" + " { " + enumEntries + " }";
                        }
                        else
                        {
                            fieldText += ("public " + column.Name + "Enum" + " " + column.Name + ";");
                            for (int val = 0; val < enumValueNames.Length; val++)
                            {
                                enumEntries += (enumValueNames[val] + " = " + val);
                                if(val + 1 < enumValueNames.Length) {enumEntries += ",";};
                            }
                            newEnum = attribute + " public enum " + column.Name + "Enum" + " { " + enumEntries + " }";
                        }
                        // string newEnum = string.Format(
                        // @"{0} public enum {1} 
                        // {
                        //     {2}
                        // }"
                        // ,attribute,column.Name,enumEntries);
                        fieldText += newEnum;
                    }
                }

                //generate the constructor that sets the fields based on the passed in value
                string constructorText = "";



                // string fullClassText = string.Format(
                //  @"using UnityEngine;
                //  using SimpleJSON;
                //  {
                //      public class {0}
                //      {
                //         {1}

                //         public {0}(SimpleJSON.JSONNode lineNode)
                //         {
                //             {2}
                //         }
                //      }
                //  }
                //  ",sheet.Name,fieldText,constructorText);
                string fullClassText = "using UnityEngine; using System; public class " + sheet.Name + " { " + fieldText + " public " + sheet.Name + "() { " + constructorText + " }  }";
                // string fullClassText = "using UnityEngine; using SimpleJSON; public class " + sheet.Name + " { " + fieldText + " public " + sheet.Name + "(SimpleJSON.JSONNode lineNode) { " + constructorText + " }  }";
                File.WriteAllText(scriptPath, fullClassText);
            }









            var assemblyBuilder = new AssemblyBuilder(outputAssembly, scripts.ToArray());

            // Exclude a reference to the copy of the assembly in the Assets folder, if any.
            assemblyBuilder.excludeReferences = new string[] { assemblyProjectPath };

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

            // Start build of assembly
            if(!assemblyBuilder.Build())
            {
                Debug.LogErrorFormat("Failed to start build of assembly {0}!", assemblyBuilder.assemblyPath);
                return;
            }

            while(assemblyBuilder.status != AssemblyBuilderStatus.Finished)
            {
                System.Threading.Thread.Sleep(10);
            }
        }
    }
}