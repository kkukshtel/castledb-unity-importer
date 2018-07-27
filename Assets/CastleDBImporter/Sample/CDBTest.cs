using UnityEngine;
using CastleDBCompiledTypes;

[ExecuteInEditMode]
public class CDBTest : MonoBehaviour
{
    public TextAsset CastleDBAsset;
    public bool test;
    void Update()
    {
        if(test)
        {
            CastleDB DB = new CastleDB(CastleDBAsset);
            unityTest testObject = DB.unityTest.unityTestsampleRow;
            
            log("textValue: " + testObject.textValue);
            log("booleanValue: " + testObject.booleanValue);
            log("colorValue: " + testObject.colorValue);
            log("enumValue: " + testObject.enumValue);
            log("fileValue: " + testObject.fileValue);
            log("flagValue: " + testObject.flagValue);
            log("floatValue: " + testObject.floatValue);
            log("imageValue: " + testObject.imageValue);
            foreach (var item in testObject.itemsList)
            {
                log($"item {item.id} has attack {item.itemAttack}");
                foreach (var ability in item.itemAbilitesList)
                {
                    log($"item has ability {ability.id} with effect {ability.effect}");
                }
            }
            
            test = false;
        }
    }

    void log(string text)
    {
        Debug.Log(text);
    }
}
