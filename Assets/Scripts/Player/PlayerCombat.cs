using System.Threading;
using UnityEngine;

public class PlayerCombat : EntityCombat {
  public int immunityLength = 1000;
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
    _ = Util.ExecuteEveryTime(FlashSprite, 100, source.Token);
    _ = Util.ExecuteAfterTime(() => {
      source.Cancel();
      SetSpriteAlpha(1);
      hp.isInvincible = false;
    }, immunityLength);
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
