using UnityEngine;
using CompiledTypes;

[ExecuteInEditMode]
public class CastleDBTest : MonoBehaviour
{
    public TextAsset CastleDBAsset;
    public bool test;
    void Update()
    {
        if(test)
        {
            CastleDB DB = new CastleDB(CastleDBAsset);
            unityTest testObject = DB.unityTest.unityTestsampleRow;

            Debug.Log("textValue: " + testObject.textValue);
            Debug.Log("booleanValue: " + testObject.booleanValue);
            Debug.Log("colorValue: " + testObject.colorValue);
            Debug.Log("enumValue: " + testObject.enumValue);
            Debug.Log("flagValue: " + testObject.flagValue);
            Debug.Log("floatValue: " + testObject.floatValue);
            foreach (var item in testObject.itemsList)
            {
                Debug.Log($"item {item.id} has attack {item.itemAttack}");
                foreach (var ability in item.itemAbilitesList)
                {
                    Debug.Log($"item has ability {ability.id} with effect {ability.effect}");
                }
            }
            
            test = false;
        }
    }
}
