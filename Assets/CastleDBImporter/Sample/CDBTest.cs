using UnityEngine;
using UnityEditor.Experimental.AssetImporters;
using System.IO;
using SimpleJSON;

namespace CastleDBImporter
{
    [ExecuteInEditMode]
    public class CDBTest : MonoBehaviour
    {
        public TextAsset CastleDB;
        public bool test;
        void Update()
        {
            if(test)
            {
                // Debug.Log("asdasda--------------------------------------------------");
                // Debug.Log("wwwwwoowowoow--------------------------------------------------");
                CastleDB castle = new CastleDB(CastleDB);
                CastleDB.RootNode root = castle.GenerateDB();
                // unityTest mytest = new unityTest(root.Sheets[0].Lines[0]);
                // Debug.Log("anotherenumcolumn: " + mytest.anotherenumcolumn);
                // Debug.Log("booleanValue: " + mytest.booleanValue);
                // Debug.Log("colorValue: " + mytest.colorValue);
                // Debug.Log("customTypeValue: " + mytest.customTypeValue);
                // Debug.Log("enumValue: " + mytest.enumValue);
                // Debug.Log("fileValue: " + mytest.fileValue);
                // Debug.Log("flagValue: " + mytest.flagValue);
                // Debug.Log("floatValue: " + mytest.floatValue);
                // Debug.Log("imageValue: " + mytest.imageValue);
                // Debug.Log("integerValue: " + mytest.integerValue);
                // Debug.Log("textValue: " + mytest.textValue);
                test = false;
            }
        }
    }

}