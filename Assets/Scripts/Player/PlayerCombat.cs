using System.Linq;
using System.Threading;
using UnityEngine;

public class PlayerCombat : EntityCombat {
  public int immunityLength = 1000;
  protected SpriteRenderer spriteRenderer;
  protected PlayerMovement movement;
  protected BuffsController buffsController;

  public override int FinalAP() {
    var buffs = buffsController.buffs.Where((buff) => buff is Rager).Count() * 0.3f;
    return (int)(baseAP * (1 + buffs));
  }

  public override int FinalDR() {
    var buffs = buffsController.buffs.Where((buff) => buff is Carbonado).Count() * 10;
    return buffs;
  }

  protected override void Start() {
    base.Start();
    spriteRenderer = GetComponent<SpriteRenderer>();
    movement = GetComponent<PlayerMovement>();
    buffsController = GetComponent<BuffsController>();
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
