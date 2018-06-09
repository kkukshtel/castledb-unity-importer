using UnityEngine;
using UnityEditor.Experimental.AssetImporters;
using System.IO;
using System.Collections.Generic;
using SimpleJSON;

namespace CastleDBImporter
{
    public class CastleDB : ScriptableObject
    {
        //each sheet is it's own type and needs its own assembly (for now)
        //create the type from the columns
        //create the objects from the lines
        public RootNode Root { get; private set;}
        public void Init(string raw)
        {
            // Root rootNode = JsonUtility.FromJson<Root>(raw);
            Root = new RootNode(JSON.Parse(raw));
            foreach (SheetNode sheet in Root.Sheets)
            {
                Debug.Log(sheet.Name);
            }
            CastleAssemblyGenerator generator = new CastleAssemblyGenerator();
            generator.GenerateAssemblies(Root);
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