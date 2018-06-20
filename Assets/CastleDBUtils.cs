using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;
using SimpleJSON;
using System.Reflection;


namespace CastleDBImporter
{
    public static class CastleDBUtils
    {
        public static Type GetTypeFromCastleDBTypeStr(string inputString)
        {
            switch (GetTypeNumFromTypeString(inputString))
            {
                case "1":
                    return typeof(string);
                case "2":
                    return typeof(bool);
                case "3":
                    return typeof(int);
                case "4":
                    return typeof(float);
                case "5": //enum
                    return typeof(Enum);
                case "10": //enum flag
                    return typeof(Enum);
                case "11":
                    return typeof(Color); 
                default:
                    return typeof(string);
            }
        }

        public static T CreateObject<T>(CastleDB.RootNode root, JSONNode line) where T : CastleDBType
        {
            T newObject = (T)Activator.CreateInstance(typeof(T));
            foreach (FieldInfo f in typeof(T).GetFields())
            {
                //get the type of the field
                //public int textValue;  testTextValue
                //cast as the field type
                // Type t = f.GetType(); // this may work too?
                //switch on the typestr of the field name class
                Debug.Log("getting field info for field: " + f.Name);
                string typeString = CastleDBUtils.GetTypeNumFromRawTypeString(root, typeof(T).ToString(), f.Name);
                Debug.Log($"typestr: {typeString}");
                dynamic value;
                switch (typeString)
                {
                    case "1":
                        value = line[f.Name].Value;
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
                    case "5":
                        value = line[f.Name].AsInt;
                        break;
                    case "10":
                        value = line[f.Name].AsInt;
                        break;
                    default:
                        value = line[f.Name].AsObject;
                        break;
                }
                f.SetValue(newObject,value);
            } 
            return newObject;
        }

        public static string GetTypeNumFromTypeString(string inputString)
        {
            Char delimiter = ':';
            String[] typeString = inputString.Split(delimiter);
            return typeString[0];
        }

        public static string[] GetEnumValuesFromTypeString(string inputString)
        {
            Char delimiter1 = ':';
            Char delimiter2 = ',';
            String[] init = inputString.Split(delimiter1);
            String[] enumvalues = init[1].Split(delimiter2);
            return enumvalues;
        }

        public static Type GetTypeForDBColumnName(CastleDB.RootNode root, string sheetName, string columnName)
        {
            return GetTypeFromCastleDBTypeStr(GetRawTypeString(root, sheetName, columnName));
        }

        public static string GetTypeNumFromRawTypeString(CastleDB.RootNode root, string sheetName, string columnName)
        {
            return GetTypeNumFromTypeString(GetRawTypeString(root,sheetName,columnName));
        }

        public static string GetRawTypeString(CastleDB.RootNode root, string sheetName, string columnName)
        {
            CastleDB.SheetNode sheet = root.Sheets.FirstOrDefault(x => x.Name == sheetName);
            CastleDB.ColumnNode column = sheet.Columns.FirstOrDefault(x => x.Name == columnName);
            Debug.Log(column.TypeStr);
            return column.TypeStr;
        }
    }
}