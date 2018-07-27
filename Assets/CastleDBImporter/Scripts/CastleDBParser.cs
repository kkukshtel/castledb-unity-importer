using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;
using System;

namespace CastleDBImporter
{
    public class CastleDBParser
    {
        //each sheet is its own type and needs its own assembly (for now)
        //create the type from the columns
        //create the objects from the lines

        TextAsset DBTextAsset;
        public RootNode Root {get; private set;}

        public CastleDBParser(TextAsset db)
        {
            DBTextAsset = db;
            Root = new RootNode(JSON.Parse(DBTextAsset.text));
        }

        public void RegenerateDB()
        {
            Root = new RootNode(JSON.Parse(DBTextAsset.text));
        }

        // Dictionary<string,List<GeneratedType>> map;
        // public void CreateObjects()
        // {
        //     //TODO: try to do object creation once here
        //     //get all the generated types
        //     //typeof(genType)?
        //     foreach (var sheet in Root.Sheets)
        //     {
        //         Type genType = Type.GetType(sheet.Name);
        //         List<sheet.name> list = new List<sheet.name>();
        //         map.Add(sheet.Name, list);
        //         for (int i = 0; i < sheet.Rows.Count; i++)
        //         {
        //             list.Add(new sheet.name(i));   
        //         }
        //         //create all the objects
        //         //intit all the objects
        //     }
        // }


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
            public List<string> RowNames
            {
                get
                {
                    List<string> names = new List<string>();
                    for (int i = 0; i < Rows.Count; i++)
                    {
                        names.Add(Rows[i]["id"]); //TODO: need to have an identifying global name
                    }
                    return names;
                }
            }
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