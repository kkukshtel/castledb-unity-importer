using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;
using System;

namespace CastleDBImporter
{
    public class CastleDB
    {
        //each sheet is its own type and needs its own assembly (for now)
        //create the type from the columns
        //create the objects from the lines

        TextAsset DBTextAsset;

        public CastleDB(TextAsset db)
        {
            DBTextAsset = db;
        }

        public RootNode GenerateDB()
        {
            return new RootNode(JSON.Parse(DBTextAsset.text));
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
            public SheetNode GetSheetWithName(string name)
            {
                foreach (var item in Sheets)
                {
                    if(item.Name == name)
                    {
                        return item;
                    }
                }
                return null;
            }
        }

        public class SheetNode
        {
            JSONNode value;
            public bool NestedType { get; protected set;}
            public string Name { get; protected set; }
            public List<ColumnNode> Columns { get; protected set; }
            public List<SimpleJSON.JSONNode> Rows { get; protected set; }
            // public List<SeperatorNode> Seperators { get; protected set; }
            // public List<PropertyNode> Properties { get; protected set; }
            public SheetNode(JSONNode sheetValue)
            {
                value = sheetValue;
                string rawName = value["name"];
                //for list types the name can come in as foo@bar@boo
                Char delimit = '@';
                var splitString = rawName.Split(delimit);
                if(splitString.Length <= 1)
                {
                    Name = value["name"];
                    NestedType = false;
                }
                else
                {
                    Name = splitString[splitString.Length - 1];
                    NestedType = true;
                }
                Columns = new List<ColumnNode>();
                Rows = new List<SimpleJSON.JSONNode>();

                foreach (KeyValuePair<string, SimpleJSON.JSONNode> item in value["columns"])
                {
                    Columns.Add(new ColumnNode(item.Value));
                }

                foreach (KeyValuePair<string, SimpleJSON.JSONNode> item in value["lines"])
                {
                    Rows.Add(item.Value);
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