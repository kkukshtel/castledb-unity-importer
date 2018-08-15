using UnityEngine;
using System.Collections.Generic;
using CompiledTypes;

[ExecuteInEditMode]
public class CastleDBTest : MonoBehaviour
{
    public TextAsset CastleDBAsset;
    public TextAsset CastleDBImagesAsset;
    public bool test;
    void Update()
    {
        if(test)
        {
            CastleDB DB = new CastleDB(CastleDBAsset, CastleDBImagesAsset);
            Creatures creature = DB.Creatures["Dragon"];
            Debug.Log("[string] name: " + creature.Name);
            Debug.Log("[bool] attacks player: " + creature.attacksPlayer);
            Debug.Log("[int] base damage: " + creature.BaseDamage);
            Debug.Log("[float] damage modifier: " + creature.DamageModifier);
            Debug.Log("[enum] death sound: " + creature.DeathSound);
            Debug.Log("[flag enum] spawn areas: " + creature.SpawnAreas);
            foreach (var item in creature.DropsList)
            {
                Debug.Log($"{creature.Name} drops item {item.item} at rate {item.DropChance}");
                foreach (var effect in item.PossibleEffectsList)
                {
                    Debug.Log($"item has effect {effect.effect} with chase {effect.EffectChance}");
                }
            }

            test = false;
        }
    }
}
