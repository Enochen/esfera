using System;
using UnityEngine;

public class TrashMonsterAI : MonsterAI {
  Collider2D monsterCollider;
  Collider2D playerCollider;
  protected override void Start() {
    base.Start();
    monsterCollider = GetComponent<Collider2D>();
    playerCollider = player.GetComponent<Collider2D>();
  }

  protected override void Update() {
    base.Update();
  }
  protected override bool ShouldAttack() {
    return base.ShouldAttack() || monsterCollider.IsTouching(playerCollider);
  }
}
