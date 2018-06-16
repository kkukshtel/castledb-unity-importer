using UnityEngine;
using UnityEditor.Experimental.AssetImporters;
using System.IO;

namespace CastleDBImporter
{
    [ExecuteInEditMode]
    public class CDBTest : MonoBehaviour
    {
        public CastleDB DB;
        public bool test;
        void Update()
        {
            if(test)
            {
                // unityTest sample = JsonUtility.FromJson<unityTest>(DB.Root.Sheets[0].Lines[0]);
                unityTest testObject =  DB.CreateObject<unityTest>(DB.Root.Sheets[0].Lines[0]);
                Debug.Log(testObject.booleanValue);
                Debug.Log(testObject.colorValue);
                Debug.Log(testObject.enumValue);
                Debug.Log(testObject.fileValue);
                Debug.Log(testObject.flagValue);
                Debug.Log(testObject.floatValue);
                Debug.Log(testObject.imageValue);
                Debug.Log(testObject.integerValue);
                Debug.Log(testObject.lineNode);
                Debug.Log(testObject.listValue);
                Debug.Log(testObject.textValue);
                test = false;
            }
        }
    }

}