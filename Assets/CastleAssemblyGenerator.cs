using System;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace CastleDBImporter
{
    public class CastleAssemblyGenerator
    {
        public void GenerateAssemblies(CastleDB.Root rootNode)
        {
            foreach (CastleDB.Sheet sheet in rootNode.sheets)
            {
                // Each sheet generates a class
                AssemblyName aName = new AssemblyName(sheet.name + "Assembly");
                AssemblyBuilder ab = 
                    AppDomain.CurrentDomain.DefineDynamicAssembly(
                        aName, 
                        AssemblyBuilderAccess.RunAndSave);

                // For a single-module assembly, the module name is usually
                // the assembly name plus an extension.
                ModuleBuilder mb = 
                    ab.DefineDynamicModule(aName.Name, aName.Name + ".dll");

                TypeBuilder tb = mb.DefineType(
                    sheet.name, 
                    TypeAttributes.Public);


                // Each column defines the fields of its given type 
                int typeCount = sheet.columns.Count;
                Type[] parameterTypes = new Type[typeCount];
                FieldBuilder[] fields = new FieldBuilder[typeCount];
                string[] fieldNames = new string[typeCount];
                for (int i = 0; i < typeCount; i++)
                {
                    CastleDB.Column column = sheet.columns[i];
                    Type fieldType = CastleDBUtils.GetTypeFromCastleDBType(column.typeStr);
                    parameterTypes[i] = fieldType;

                    // Add a public field of the specific type
                    FieldBuilder fb = tb.DefineField(
                    column.name, 
                    fieldType, 
                    FieldAttributes.Public);
                    fields[i] = fb;
                    fieldNames[i] = column.name;
                }


                // Define a constructor that takes an integer argument and 
                // stores it in the private field. 
                ConstructorBuilder ctor1 = tb.DefineConstructor(
                    MethodAttributes.Public, 
                    CallingConventions.Standard, 
                    parameterTypes);

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

                for (int i = 0; i < typeCount; i++)
                {
                    ctor1IL.Emit(OpCodes.Ldarg_0);                    
                    ctor1IL.Emit(OpCodes.Ldarg_S, i+1);
                    ctor1IL.Emit(OpCodes.Stfld, fields[i]);
                    // ctor1IL.Emit(OpCodes.Ldarg_1);
                    // ctor1IL.Emit(OpCodes.Stfld, fbNumber);
                }

                ctor1IL.Emit(OpCodes.Ret);

                // Finish the type.
                Type t = tb.CreateType();

                // The following line saves the single-module assembly. This
                // requires AssemblyBuilderAccess to include Save. You can now
                // type "ildasm MyDynamicAsm.dll" at the command prompt, and 
                // examine the assembly. You can also write a program that has
                // a reference to the assembly, and use the MyDynamicType type.
                // 
                ab.Save(aName.Name + ".dll");

                foreach (CastleDB.Line line in sheet.lines)
                {
                    if(sheet.name != "unityTest3") { continue;}
                    FieldInfo fi = t.GetField("testStringColumn");
                    //need to get all the generate fields
                    FieldInfo[] typeFields = t.GetFields();
                    //convert the raw line stream to json using our new type
                    // (t.GetType())newObj = JsonUtility.FromJson<t>(line.rawLine);


                    for (int i = 0; i < typeFields.Length; i++)
                    {
                    //then loop through generated fields and set their value to their value of the named key in json
                        
                    }

    //                   int propertiesCounter = 0;

    // // Loop over the values that we will assign to the properties
    // foreach (XmlNode node in xmlDoc.SelectSingleNode("root").ChildNodes)
    // {
    //     string value = node.InnerText;
    //     properties[propertiesCounter].SetValue(generetedObject, value, null);
    //     propertiesCounter++;
    // }
                    
                    
                    // object generated = Activator.CreateInstance(t);
                    object generated = Activator.CreateInstance(t,
                        new object[]{
                            "THISVALUEISNTRIGHT"
                        });
                    JsonUtility.FromJsonOverwrite(line.rawLine, generated);
                    // JsonUtility.FromJsonOverwrite(line.rawLine, generated);
                    Debug.Log($"o2.Number: {fi.GetValue(generated)}");
                }
            }
        }
    }
}