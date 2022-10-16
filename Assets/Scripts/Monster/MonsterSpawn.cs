using UnityEngine;
using static Director;

public class MonsterSpawn : MonoBehaviour {
  public MonsterRarity rarity;
  public int cardCost;
  public int cardWeight;

  public Card ToCard() {
    return new Card {
      entityPrefab = gameObject,
      baseCost = cardCost,
      baseWeight = cardWeight,
      rarity = rarity
    };
  }
}
