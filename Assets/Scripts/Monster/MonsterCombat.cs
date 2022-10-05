using System;
using UnityEngine;

public class MonsterCombat : EntityCombat {
  protected override void Start() {
    base.Start();
  }

  protected override void OnHit(GameObject origin, Vector2 knockback) {
    base.OnHit(origin, knockback);
  }

  protected override void OnSpawnAnimBegin() {
    base.OnSpawnAnimBegin();
  }

  protected override void OnSpawnAnimEnd() {
    base.OnSpawnAnimEnd();
  }

  protected override void OnDeathAnimBegin() {
    base.OnDeathAnimBegin();
  }

  protected override void OnDeathAnimEnd() {
    base.OnDeathAnimEnd();
    Destroy(gameObject);
  }
}
