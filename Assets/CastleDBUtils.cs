using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

namespace CastleDBImporter
{
    public static class CastleDBUtils
    {
        public static Type GetTypeFromCastleDBType(string typeString)
        {
            switch (typeString)
            {
                case "1":
                    return typeof(string);
                case "2":
                    return typeof(bool);
                case "3":
                    return typeof(int);
                case "4":
                    return typeof(float);
                default:
                    return typeof(string);
            }
        }

        public static Type GetTypeForDBColumnName(CastleDB.RootNode root, string sheetName, string columnName)
        {
            return GetTypeFromCastleDBType(GetTypeString(root, sheetName, columnName));
        }

        public static string GetTypeString(CastleDB.RootNode root, string sheetName, string columnName)
        {
            CastleDB.SheetNode sheet = root.Sheets.FirstOrDefault(x => x.Name == sheetName);
            CastleDB.ColumnNode column = sheet.Columns.FirstOrDefault(x => x.Name == columnName);
            return column.TypeStr;
        }
    }
}