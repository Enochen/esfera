using System;
using UnityEngine;
using static EntityController;

public class MonsterMovement : EntityMovement {
  private Vector2 _velocity;

  public Vector2 Velocity { get => _velocity; set => _velocity = value; }

  protected override void UpdateAnimator() {
    animator.SetBool("isGrounded", IsGrounded);
    animator.SetBool("isCrouching", State == MoveState.CROUCHING);
    animator.SetBool("isClimbing", State == MoveState.CLIMBING);
    animator.SetFloat("speedX", Math.Abs(rb.velocity.x));
    animator.SetFloat("speedY", Math.Abs(rb.velocity.y));
  }

  protected override void UpdateMovement() {
    rb.velocity = Velocity;
    if (Velocity.x < -0.01) {
      entity.facing = FacingDirection.LEFT;
    } else if (Velocity.x > 0.01) {
      entity.facing = FacingDirection.RIGHT;
    }

    var hpBar = GetComponentInChildren<FloatingHPBar>();
    if (hpBar != null) {
      hpBar.transform.localScale = hpBar.transform.localScale.SetFacing(entity.facing);
    }
  }
}
