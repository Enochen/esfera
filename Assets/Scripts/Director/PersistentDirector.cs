using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PersistentDirector : Director {
  public GameObject[] monsters;

  protected override IEnumerable<Card> GenerateCards() {
    return monsters.Select((prefab) => prefab.GetComponent<MonsterSpawn>().ToCard());
  }

  protected override bool IsCardValid(Card card) {
    return true;
  }
}
