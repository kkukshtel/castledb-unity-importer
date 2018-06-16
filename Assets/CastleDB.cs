using UnityEngine;
using UnityEditor.Experimental.AssetImporters;
using System.IO;
using System.Collections.Generic;
using SimpleJSON;
using System;
using System.Reflection;



namespace CastleDBImporter
{
    public class CastleDB : ScriptableObject
    {
        //each sheet is it's own type and needs its own assembly (for now)
        //create the type from the columns
        //create the objects from the lines
        [SerializeField]
        private string rawDB;
        public RootNode Root
        {
            get 
            {
                return new RootNode(JSON.Parse(rawDB));
            }
        }
        public void Init(string raw)
        {
            // Root rootNode = JsonUtility.FromJson<Root>(raw);
            rawDB = raw;
            CastleAssemblyGenerator generator = new CastleAssemblyGenerator();
            generator.GenerateAssemblies(Root);
        }

        public T CreateObject<T>(JSONNode line) //need to make generated assemblies extend from a base CDBType class
        {
            T newObject = (T)Activator.CreateInstance(typeof(T));
            foreach (FieldInfo f in typeof(T).GetFields())
            {
                //get the type of the field
                //public int textValue;  testTextValue
                //cast as the field type
                // Type t = f.GetType(); // this may work too?
                //switch on the typestr of the field name class
                string typeString = CastleDBUtils.GetTypeString(Root, typeof(T).ToString(), f.Name);
                dynamic value;
                switch (typeString)
                {
                    case "1":
                        value = line[f.Name];
                        break;
                    case "2":
                        value = line[f.Name].AsBool;
                        break;
                    case "3":
                        value = line[f.Name].AsInt;
                        break;
                    case "4":
                        value = line[f.Name].AsFloat;
                        break;
                    default:
                        value = line[f.Name].AsBool;
                        break;
                }
                f.SetValue(newObject,value);
                //is it possible in here to assign a value to a field
                //based on the line key name match?
                Debug.Log(f.Name);
            } 
            return newObject;
        }

        public class RootNode
        {
            JSONNode value;
            public List<SheetNode> Sheets { get; protected set;}
            public RootNode (JSONNode root)
            {
                value = root;
                Sheets = new List<SheetNode>();
                foreach (KeyValuePair<string, SimpleJSON.JSONNode> item in value["sheets"])
                {
                    Sheets.Add(new SheetNode(item.Value));
                }
            }
        }

        public class SheetNode
        {
            JSONNode value;
            public string Name { get; protected set; }
            public List<ColumnNode> Columns { get; protected set; }
            public List<SimpleJSON.JSONNode> Lines { get; protected set; }
            // public List<SeperatorNode> Seperators { get; protected set; }
            // public List<PropertyNode> Properties { get; protected set; }
            public SheetNode(JSONNode sheetValue)
            {
                value = sheetValue;
                Name = value["name"];
                Columns = new List<ColumnNode>();
                Lines = new List<SimpleJSON.JSONNode>();

                foreach (KeyValuePair<string, SimpleJSON.JSONNode> item in value["columns"])
                {
                    Columns.Add(new ColumnNode(item.Value));
                }

                foreach (KeyValuePair<string, SimpleJSON.JSONNode> item in value["lines"])
                {
                    Lines.Add(item.Value);
                }
            }
        }

        public class ColumnNode
        {
            JSONNode value;
            public string TypeStr { get; protected set;}
            public string Name { get; protected set;}
            public string Display { get; protected set;}
            public ColumnNode(JSONNode sheetValue)
            {
                value = sheetValue;
                Name = value["name"];
                Display = value["display"];
                TypeStr = value["typeStr"];
            }
        }
    }

}