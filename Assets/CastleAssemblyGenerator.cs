using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using SimpleJSON;

namespace CastleDBImporter
{
    public class CastleAssemblyGenerator
    {
        //DONE
        public void GenerateAssemblies(CastleDB.RootNode root)
        {
            AssemblyName aName = new AssemblyName("CastleDBTypesAssembly");
            aName.CodeBase = Application.dataPath;
            AssemblyBuilder ab = 
                AppDomain.CurrentDomain.DefineDynamicAssembly(
                    aName, 
                    AssemblyBuilderAccess.RunAndSave,
                    Application.dataPath);

            // For a single-module assembly, the module name is usually
            // the assembly name plus an extension.
            ModuleBuilder mb = 
                ab.DefineDynamicModule(aName.Name, aName.Name + ".dll");

            foreach (CastleDB.SheetNode sheet in root.Sheets)
            {
                // Each sheet generates a class

                TypeBuilder tb = mb.DefineType(
                    sheet.Name, 
                    TypeAttributes.Public);

                tb.SetParent(typeof(CastleDBImporter.CastleDBType));


                tb.DefineDefaultConstructor(MethodAttributes.Public);

                // Each column defines the fields of its given type 
                // This block generates all the public fields of the generate type
                int typeCount = sheet.Columns.Count;
                // Type[] parameterTypes = new Type[typeCount];
                // FieldBuilder[] fields = new FieldBuilder[typeCount];
                // string[] fieldNames = new string[typeCount];
                for (int i = 0; i < typeCount; i++)
                {
                    CastleDB.ColumnNode column = sheet.Columns[i];
                    Type fieldType = CastleDBUtils.GetTypeFromCastleDBTypeStr(column.TypeStr);
                    if(fieldType != typeof(Enum))
                    {
                        FieldBuilder fb = tb.DefineField(
                        column.Name, 
                        fieldType, 
                        FieldAttributes.Public);
                        // fields[i] = fb;
                        // fieldNames[i] = column.Name;
                        // Add a public field of the specific type
                    }
                    else
                    {
                        EnumBuilder newEnum = mb.DefineEnum(column.Name, TypeAttributes.Public, typeof(int));
                        string[] enumValues = CastleDBUtils.GetEnumValuesFromTypeString(column.TypeStr);

                        if(CastleDBUtils.GetTypeNumFromCastleDBTypeString(column.TypeStr) == "10") //flag
                        {
                            ConstructorInfo cinfo = typeof(FlagsAttribute).GetConstructor(Type.EmptyTypes);
                            CustomAttributeBuilder flagsAttribute = new CustomAttributeBuilder(cinfo, new object[] { });
                            newEnum.SetCustomAttribute(flagsAttribute);

                            
                            for (int val = 0; i < enumValues.Length; i++)
                            {
                                newEnum.DefineLiteral(enumValues[val], (int)Math.Pow(2, val));
                            }
                        }

                        else
                        {
                            for (int val = 0; i < enumValues.Length; i++)
                            {
                                newEnum.DefineLiteral(enumValues[val], val);
                            }
                        }

                        newEnum.CreateType();
                    }
                }


                // Define a constructor that takes a JSON argument and 
                // stores it in the private field. 
                ConstructorBuilder ctor1 = tb.DefineConstructor(
                    MethodAttributes.Public, 
                    CallingConventions.Standard, 
                    new Type[]{typeof(JSONNode)});

                ILGenerator ctor1IL = ctor1.GetILGenerator();
                // For a constructor, argument zero is a reference to the new
                // instance. Push it on the stack before calling the base
                // class constructor. Specify the default constructor of the 
                // base class (System.Object) by passing an empty array of 
                // types (Type.EmptyTypes) to GetConstructor.
                ctor1IL.Emit(OpCodes.Ldarg_0);
                ctor1IL.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
                // Push the instance on the stack before pushing the argument
                // that is to be assigned to the private field m_number.
                // ctor1IL.Emit(OpCodes.Ldarg_0);

                // for (int i = 0; i < typeCount; i++)
                // {
                //     ctor1IL.Emit(OpCodes.Ldarg_0);                    
                //     ctor1IL.Emit(OpCodes.Ldarg_S, i+1);
                //     ctor1IL.Emit(OpCodes.Stfld, fields[i]);
                // }

                //tries to define a constuctor that can assign a line to a field for use later but idk
                // FieldBuilder constructorField = tb.DefineField(
                //     "lineNode", 
                //     typeof(JSONNode), 
                //     FieldAttributes.Public);

                // ctor1IL.Emit(OpCodes.Ldarg_0);                    
                // ctor1IL.Emit(OpCodes.Ldarg_1);
                // ctor1IL.Emit(OpCodes.Stfld, constructorField);

                ctor1IL.Emit(OpCodes.Ret);

                // Finish the type.
                Type t = tb.CreateType();

                // The following line saves the single-module assembly. This
                // requires AssemblyBuilderAccess to include Save. You can now
                // type "ildasm MyDynamicAsm.dll" at the command prompt, and 
                // examine the assembly. You can also write a program that has
                // a reference to the assembly, and use the MyDynamicType type.
                // 

                // foreach (JSONNode line in sheet.Lines)
                // {
                //     if(sheet.Name != "unityTest3") { continue;}
                //     FieldInfo fi = t.GetField("testStringColumn");
                //     //need to get all the generate fields
                //     FieldInfo[] typeFields = t.GetFields();
                //     //convert the raw line stream to json using our new type
                //     object generated = Activator.CreateInstance(t);

                //     for (int i = 0; i < sheet.Columns.Count; i++)
                //     {
                //         //cast the line value to the type specificed by the typeString that is the name of the field
                //         switch (sheet.Columns[i].TypeStr)
                //         {
                //             case "1":
                //                 typeFields[i].SetValue(generated, line[typeFields[i].Name].AsInt);
                //                 break;
                //             default:
                //                 break;
                //         }
                //     }
                               
                //     // object generated = Activator.CreateInstance(t,
                //     //     new object[]{
                //     //         "THISVALUEISNTRIGHT"
                //     //     });
                //     // JsonUtility.FromJsonOverwrite(line.rawLine, generated);
                //     // Debug.Log($"o2.Number: {fi.GetValue(generated)}");
                // }
            }
            
            ab.Save(aName.Name + ".dll");

        }
    }
}