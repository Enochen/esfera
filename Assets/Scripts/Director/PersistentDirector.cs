using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PersistentDirector : Director {
  public GameObject[] commonMonsters;
  protected override IEnumerable<Card> GenerateCards() {
    var commonCards = commonMonsters.Select((prefab) => new Card {
      entityPrefab = prefab,
      BaseCost = 1,
      weight = 4
    });
    return commonCards;
  }

  protected override bool IsCardValid(Card card) {
    return true;
  }
}
