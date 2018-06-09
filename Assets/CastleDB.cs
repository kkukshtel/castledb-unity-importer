using UnityEngine;
using UnityEditor.Experimental.AssetImporters;
using System.IO;
using System.Collections.Generic;

namespace CastleDBImporter
{
    public class CastleDB : ScriptableObject
    {
        //each sheet is it's own type and needs its own assembly (for now)
        //create the type from the columns
        //create the objects from the lines
        public void Init(string raw)
        {
            Root rootNode = JsonUtility.FromJson<Root>(raw);
            foreach (Sheet item in rootNode.sheets)
            {
                Debug.Log(item.name);
            }
            CastleAssemblyGenerator generator = new CastleAssemblyGenerator();
            generator.GenerateAssemblies(rootNode);
        }

        [System.Serializable]
        public class Root
        {
            public List<Sheet> sheets;
            public List<CustomType> customTypes;
            public bool compress;
        }

        [System.Serializable]
        public class Sheet
        {
            public string name;
            public List<Column> columns;
            public List<Line> lines;
            public List<Seperator> seperators;
            public List<Property> props;
        }

        [System.Serializable]
        public class Column
        {
            public string typeStr;
            public string name;
            public string display;
        }

        [System.Serializable]
        public class Line
        {
            public string rawLine;
        }

        [System.Serializable]
        public class Seperator
        {
        }

        [System.Serializable]
        public class Property
        {
        }

        [System.Serializable]
        public class CustomType
        {
        }
    }

}