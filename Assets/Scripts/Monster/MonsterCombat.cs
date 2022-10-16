using UnityEngine;

public class MonsterCombat : EntityCombat {
  protected override void Start() {
    base.Start();
  }

  protected override void OnHit(GameObject origin, Vector2 knockback) {
    base.OnHit(origin, knockback);
  }
}
