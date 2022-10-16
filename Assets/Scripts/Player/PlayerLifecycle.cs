using UnityEngine;

public class PlayerLifecycle : EntityLifecycle {
  PlayerMovement movement;

  protected override void Start() {
    base.Start();
    movement = GetComponent<PlayerMovement>();
  }

  protected override void OnSpawnAnimBegin() {
    base.OnSpawnAnimBegin();
    entity.SetAnimLocked(true);
    rb.constraints = RigidbodyConstraints2D.FreezeAll;
    movement.ResetAirborneVelX();
  }

  protected override void OnSpawnAnimEnd() {
    base.OnSpawnAnimEnd();
    entity.SetAnimLocked(false);
    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
  }

  protected override void OnDeathAnimBegin() {
    base.OnDeathAnimBegin();
    entity.SetAnimLocked(true);
  }

  protected override void OnDeathAnimEnd() {
    base.OnDeathAnimEnd();
    entity.SetAnimLocked(false);
  }
}
