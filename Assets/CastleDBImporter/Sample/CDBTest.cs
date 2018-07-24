using UnityEngine;
using UnityEditor.Experimental.AssetImporters;
using System.IO;
using SimpleJSON;

namespace CastleDBImporter
{
    [ExecuteInEditMode]
    public class CDBTest : MonoBehaviour
    {
        public TextAsset CastleDBAsset;
        public bool test;
        void Update()
        {
            if(test)
            {
                CastleDB castle = new CastleDB(CastleDBAsset);
                //List<unityTest> myListofValues = castle.Get<unityTest>();
                //unityTest test = myListofValues.Get(unityTest.unityTestvalues.unityTestsampleRow)
                unityTest mytest = new unityTest(castle.Root, unityTest.unityTestvalues.unityTestsampleRow);
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