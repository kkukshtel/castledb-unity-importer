using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;
using System;

namespace CastleDBImporter
{
    public class CastleDBParser
    {
        TextAsset DBTextAsset;
        TextAsset DBImagesTextAsset;

        public RootNode Root {get; private set;}
        public Dictionary<string, Texture> DatabaseImages { get; private set; }

        public CastleDBParser(TextAsset db, TextAsset dbImages = null)
        {
            DBTextAsset = db;
            DBImagesTextAsset = dbImages;

            RegenerateDB();
        }

        public void RegenerateDB()
        {
            if (DBImagesTextAsset != null)
            {
                DatabaseImages = new Dictionary<string, Texture>();

                var dbImagesJSON = JSON.Parse(DBImagesTextAsset.text);
                foreach (var dbImage in dbImagesJSON.AsObject)
                {                    
                    string base64 = dbImage.Value.Value.Split(',')[1];

                    // We must add padding to the base64 string because I guess CastleDB doesn't add it
                    switch (base64.Length % 4)
                    {
                        case 2: base64 += "=="; break;
                        case 3: base64 += "="; break;
                    }
                   
                    byte[] bytes = Convert.FromBase64String(base64);

                    Texture2D tex = new Texture2D(2, 2);

                    if (!tex.LoadImage(bytes))
                        Debug.LogError("Error loading image from database: " + dbImage.Key);

                    DatabaseImages[dbImage.Key] = tex;
                }

                Debug.Log("Images from database loaded: " + DatabaseImages.Keys.Count);
            }

            Root = new RootNode(JSON.Parse(DBTextAsset.text));
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
