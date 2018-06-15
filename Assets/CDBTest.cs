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
                // DB.CreateObject<unityTest>(DB.Root.Sheets[0].Lines[0]);
                test = false;
            }
        }
    }

}