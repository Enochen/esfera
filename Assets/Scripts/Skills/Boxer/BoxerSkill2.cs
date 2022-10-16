using UnityEngine;

public class BoxerSkill2 : Skill {
  readonly float dashDistance;
  readonly LayerMask enemyMask;
  Rigidbody2D rb;
  TrailRenderer trail;
  PlayerController player;
  PlayerMovement movement;
  Vector2 prevPos;

  public BoxerSkill2(
    SkillMeta meta,
    GameObject source,
    float dashDistance,
    LayerMask enemyMask,
    int cooldown = 0) : base(meta, source, cooldown) {
    this.dashDistance = dashDistance;
    this.enemyMask = enemyMask;
  }

  protected override void Setup() {
    rb = source.GetComponent<Rigidbody2D>();
    trail = source.GetComponent<TrailRenderer>();
    player = source.GetComponent<PlayerController>();
    movement = source.GetComponent<PlayerMovement>();
  }

  public override void Activate() {
    base.Activate();
    trail.emitting = true;
    prevPos = rb.position;
    var destination = rb.position + dashDistance * player.facing.X() * Vector2.right;
    rb.MovePosition(destination);
  }

  public override void AnimationEnd() {
    base.AnimationEnd();
    trail.emitting = false;
    movement.ResetAirborneVelX();
    rb.velocity = Vector2.zero;
    foreach (var hit in Physics2D.LinecastAll(prevPos, rb.position, enemyMask)) {
      hit.collider.GetComponent<HPController>().GetHit(source, Vector2.zero, 5, 3);
    }
  }
}
