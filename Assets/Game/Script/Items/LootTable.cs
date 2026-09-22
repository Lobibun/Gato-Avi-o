using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Loot/Loot Table")]
public class LootTable : ScriptableObject
{
    public List<DropItem> randomDrops;


 public GameObject GetRandomDrop()
{
    int totalWeight = 0;

    foreach (DropItem item in randomDrops)
    {
     totalWeight += item.weight;
    }

    int randomNumber = Random.Range(0, totalWeight);

    foreach (DropItem item in randomDrops)
    {
      randomNumber -=item.weight;
      if (randomNumber < 0)
      {
        return item.prefab;
      }
    }
    return null;
}
}