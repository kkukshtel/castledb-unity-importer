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
        public static string GetTypeFromCastleDBColumn(CastleDBParser.ColumnNode column)
        {
            string typeString = GetTypeNumFromCastleDBTypeString(column.TypeStr);
            switch (typeString)
            {
                case "1":
                    return "string";
                case "2":
                    return "bool";
                case "3":
                    return "int";
                case "4":
                    return "float";
                case "5": //enum
                    return "Enum";
                case "10": //enum flag
                    return "Enum";
                case "6": //ref type
                    return GetRefTypeFromTypeString(column.TypeStr);
                case "8": //nested list type
                    return column.Name;
                case "11": //color
                     //TODO: fix color encoding  https://docs.unity3d.com/ScriptReference/ColorUtility.TryParseHtmlString.html
                    return "string";
                    // return typeof(Color); 
                default:
                    return "string";
            }
        }

        public static string GetCastStringFromCastleDBTypeStr(string inputString)
        {
            //taken from the possibilites of casting in SimpleJSON
            string typeString = GetTypeNumFromCastleDBTypeString(inputString);
            switch (typeString)
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

        public static string GetRefTypeFromTypeString(string inputString)
        {
            Char delimiter = ':';
            String[] typeString = inputString.Split(delimiter);
            return typeString[1];
        }

        public static string[] GetEnumValuesFromTypeString(string inputString)
        {
            Char delimiter1 = ':';
            Char delimiter2 = ',';
            String[] init = inputString.Split(delimiter1);
            String[] enumvalues = init[1].Split(delimiter2);
            return enumvalues;
        }

        /* Unused but maybe useful in the future
        public static Type GetTypeForDBColumnName(CastleDB.RootNode root, string sheetName, string columnName)
        {
            return GetTypeFromCastleDBTypeStr(GetRawTypeStringFromColumnName(root, sheetName, columnName));
        }

        public static string GetTypeNumFromRawTypeString(CastleDBParser.RootNode root, string sheetName, string columnName)
        {
            return GetTypeNumFromCastleDBTypeString(GetRawTypeStringFromColumnName(root,sheetName,columnName));
        }

        public static string GetRawTypeStringFromColumnName(CastleDBParser.RootNode root, string sheetName, string columnName)
        {
            CastleDBParser.SheetNode sheet = root.Sheets.FirstOrDefault(x => x.Name == sheetName);
            CastleDBParser.ColumnNode column = sheet.Columns.FirstOrDefault(x => x.Name == columnName);
            Debug.Log(column.TypeStr);
            return column.TypeStr;
        }
        */
    }
}