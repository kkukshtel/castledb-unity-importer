using UnityEngine;
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
    }
}