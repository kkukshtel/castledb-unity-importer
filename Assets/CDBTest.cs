using UnityEngine;
using UnityEditor.Experimental.AssetImporters;
using System.IO;

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
                // Debug.Log("testing");
                CastleDB castle = new CastleDB(CastleDB);
                CastleDB.RootNode root = castle.GenerateDB();
                unityTest testObject =  CastleDBUtils.CreateObject<unityTest>(root, root.Sheets[0].Lines[0]);
                // Debug.Log($"boolean value: {testObject.booleanValue}");
                // Debug.Log($"colorValue: {testObject.colorValue}");
                // Debug.Log($"enumValue: {testObject.enumValue}");
                // Debug.Log($"fileValue: {testObject.fileValue}");
                // Debug.Log($"flagValue: {testObject.flagValue}");
                // Debug.Log($"floatValue: {testObject.floatValue}");
                // Debug.Log($"imageValue: {testObject.imageValue}");
                // Debug.Log($"integerValue: {testObject.integerValue}");
                // Debug.Log($"listValue: {testObject.listValue}");
                // Debug.Log($"textValue: {testObject.textValue}");
                
                // Debug.Log(testObject.lineNode); if the constructor works eventually
                
                test = false;
            }
        }
    }

}