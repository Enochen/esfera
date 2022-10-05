using UnityEngine;

public class EntityController : MonoBehaviour {
  public FacingDirection facing = FacingDirection.RIGHT;
  public bool hitstunned = false;
  public bool isDead = false;
  protected Animator animator;
  public bool CannotMove => hitstunned || AnimLocked || isDead;
  protected virtual void Start() {
    animator = GetComponent<Animator>();
  }
  public bool AnimLocked { get; private set; }

  public void SetAnimLocked(bool value) {
    if (animator != null) {
      animator.SetBool("isAnimLocked", value);
    }
    AnimLocked = value;
  }

  public enum FacingDirection { LEFT, RIGHT }
}
