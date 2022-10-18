using System;

public class MonsterMovement : EntityMovement {
  public event Action MovementEvent;

  protected override void UpdateAnimator() {
    animator.SetBool("isGrounded", IsGrounded);
    animator.SetBool("isCrouching", State == MoveState.CROUCHING);
    animator.SetBool("isClimbing", State == MoveState.CLIMBING);
    animator.SetFloat("speedX", Math.Abs(rb.velocity.x));
    animator.SetFloat("speedY", Math.Abs(rb.velocity.y));
  }

  protected override void UpdateMovement() {
    MovementEvent?.Invoke();
    if (rb.velocity.x < -0.01) {
      entity.facing = FacingDirection.LEFT;
    } else if (rb.velocity.x > 0.01) {
      entity.facing = FacingDirection.RIGHT;
    }

    var hpBar = GetComponentInChildren<FloatingHPBar>();
    if (hpBar != null) {
      hpBar.transform.localScale = hpBar.transform.localScale.SetFacing(entity.facing);
    }
  }
}
