using UnityEngine;
// using CompiledTypes;

[ExecuteInEditMode]
public class CastleDBTest : MonoBehaviour
{
    public TextAsset CastleDBAsset;
    public bool test;
    void Update()
    {
        if(test)
        {
            // CastleDB DB = new CastleDB(CastleDBAsset);
            // Creatures creature = DB.Creatures.Dragon;

            // DB.Creatures.James;
            // Items item = DB.Creatures.Bear.DropsList[0].item;
            // Debug.Log("[string] name: " + creature.Name);
            // Debug.Log("[bool] attacks player: " + creature.attacksPlayer);
            // Debug.Log("[int] base damage: " + creature.BaseDamage);
            // Debug.Log("[float] damage modifier: " + creature.DamageModifier);
            // Debug.Log("[enum] death sound: " + creature.DeathSound);
            // Debug.Log("[flag enum] spawn areas: " + creature.SpawnAreas);
            // foreach (var item in creature.DropsList)
            // {
            //     Debug.Log($"{creature.Name} drops item {item.item} at rate {item.dropChance}");
            //     foreach (var ability in item.itemAbilitesList)
            //     {
            //         Debug.Log($"item has ability {ability.id} with effect {ability.effect}");
            //     }
            // }
            
            test = false;
        }
    }
}
