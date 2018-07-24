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
                CastleDB castle = new CastleDB(CastleDB);
                CastleDB.RootNode root = castle.GenerateDB();
                unityTest mytest = new unityTest(root, unityTest.unityTestvalues.unityTestsampleRow);
                log("booleanValue: " + mytest.booleanValue);
                log("colorValue: " + mytest.colorValue);
                log("enumValue: " + mytest.enumValue);
                log("fileValue: " + mytest.fileValue);
                log("flagValue: " + mytest.flagValue);
                log("floatValue: " + mytest.floatValue);
                log("imageValue: " + mytest.imageValue);
                foreach (var item in mytest.itemsList)
                {
                    log("attack " + item.itemAttack);
                    foreach (var ability in item.itemAbilitesList)
                    {
                        log(ability.effect);
                    }
                }
                
                log("textValue: " + mytest.textValue);
                test = false;
            }
        }

        void log(string text)
        {
            Debug.Log(text);
        }
    }

}