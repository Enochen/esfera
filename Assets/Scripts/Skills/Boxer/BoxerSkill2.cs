using UnityEngine;

public class BoxerSkill2 : Skill {
  readonly float dashDistance;
  readonly LayerMask terrainMask;
  readonly LayerMask enemyMask;
  float originalGravity;
  Rigidbody2D rb;
  TrailRenderer trail;
  PlayerController player;
  PlayerMovement movement;

  public BoxerSkill2(
    SkillMeta meta,
    float dashDistance,
    LayerMask terrainMask,
    LayerMask enemyMask,
    int cooldown = 0) : base(meta, cooldown) {
    this.dashDistance = dashDistance;
    this.terrainMask = terrainMask;
    this.enemyMask = enemyMask;
  }

  protected override void Setup() {
    rb = meta.source.GetComponent<Rigidbody2D>();
    trail = meta.source.GetComponent<TrailRenderer>();
    player = meta.source.GetComponent<PlayerController>();
    movement = meta.source.GetComponent<PlayerMovement>();
  }

  public override void Activate() {
    base.Activate();
    // originalGravity = rb.gravityScale;
    // movement.isDashing = true;
    // rb.gravityScale = 0;
    // rb.velocity = new Vector2(player.facing.X() * dashDistance, 0);
    // trail.emitting = true;

    var destination = rb.position + dashDistance * player.facing.X() * Vector2.right;
    rb.MovePosition(destination);
  }

  public override void AnimationEnd() {
    base.AnimationEnd();
    movement.ResetAirborneVelX();
    // movement.isDashing = false;
    // rb.gravityScale = originalGravity;
    rb.velocity = Vector2.zero;
    // trail.emitting = false;
  }
}
