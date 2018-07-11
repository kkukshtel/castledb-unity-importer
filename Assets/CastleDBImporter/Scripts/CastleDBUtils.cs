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
            switch (GetTypeNumFromCastleDBTypeString(inputString))
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
                    return typeof(string); //TODO: fix color encoding  https://docs.unity3d.com/ScriptReference/ColorUtility.TryParseHtmlString.html
                    // return typeof(Color); 
                default:
                    return typeof(string);
            }
        }

        public static string GetCastStringFromCastleDBTypeStr(string inputString)
        {
            JSONNode value;
            //taken from the possibilites of casting in SimpleJSON
            switch (GetTypeNumFromCastleDBTypeString(inputString))
            {
                case "1":
                    return "";
                case "2":
                    return ".AsBool";
                case "3":
                    return ".AsInt";
                case "4":
                    return ".AsFloat";
                case "5": //enum
                    return ".AsInt";
                case "10": //enum flag
                    return ".AsInt";
                case "11":
                    return "";  //https://docs.unity3d.com/ScriptReference/ColorUtility.TryParseHtmlString.html
                default:
                    return "";
            }
        }

        public static string GetTypeNumFromCastleDBTypeString(string inputString)
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
            return GetTypeFromCastleDBTypeStr(GetRawTypeStringFromColumnName(root, sheetName, columnName));
        }

        public static string GetTypeNumFromRawTypeString(CastleDB.RootNode root, string sheetName, string columnName)
        {
            return GetTypeNumFromCastleDBTypeString(GetRawTypeStringFromColumnName(root,sheetName,columnName));
        }

        public static string GetRawTypeStringFromColumnName(CastleDB.RootNode root, string sheetName, string columnName)
        {
            CastleDB.SheetNode sheet = root.Sheets.FirstOrDefault(x => x.Name == sheetName);
            CastleDB.ColumnNode column = sheet.Columns.FirstOrDefault(x => x.Name == columnName);
            Debug.Log(column.TypeStr);
            return column.TypeStr;
        }
    }
}