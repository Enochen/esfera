using System;
using System.Threading;
using UnityEngine;
using static EntityController;

public class PlayerCombat : EntityCombat {
  public int immunityLength = 1000;
  public Vector3 spawnPoint;
  SpriteRenderer spriteRenderer;
  PlayerMovement movement;

  protected override void Start() {
    base.Start();
    spriteRenderer = GetComponent<SpriteRenderer>();
    movement = GetComponent<PlayerMovement>();
  }

  protected override void OnHit(GameObject origin, Vector2 knockback) {
    base.OnHit(origin, knockback);
    if (stance == 1 || knockback != Vector2.zero) {
      movement.ResetAirborneVelX();
    }
    CancellationTokenSource source = new();
    hp.isInvincible = true;
    Util.ExecuteEveryTime(FlashSprite, 100, source.Token);
    Util.ExecuteAfterTime(() => {
      source.Cancel();
      SetSpriteAlpha(1);
      hp.isInvincible = false;
    }, immunityLength);
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
    hp.HP = hp.MaxHP;
  }

  protected override void OnDeathAnimEnd() {
    base.OnDeathAnimEnd();
    entity.SetAnimLocked(false);
    transform.position = spawnPoint;
    entity.facing = FacingDirection.LEFT;
    OnSpawnAnimBegin();
  }

  void FlashSprite() {
    if (spriteRenderer == null) return;
    SetSpriteAlpha(spriteRenderer.color.a == 1 ? 0.3f : 1);
  }

  void SetSpriteAlpha(float a) {
    if (spriteRenderer == null) return;
    Color newColor = spriteRenderer.color;
    newColor.a = a;
    spriteRenderer.color = newColor;
  }
}
