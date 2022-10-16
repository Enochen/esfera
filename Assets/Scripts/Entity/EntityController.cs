using UnityEngine;

public class EntityController : MonoBehaviour {
  public FacingDirection facing = FacingDirection.RIGHT;
  public bool hitstunned = false;
  protected Animator animator;
  protected HPController hp;
  public bool CannotMove => hitstunned || AnimLocked || hp.IsDead;
  protected virtual void Start() {
    animator = GetComponent<Animator>();
    hp = GetComponent<HPController>();
  }
  public bool AnimLocked { get; private set; }

  public void SetAnimLocked(bool value) {
    if (animator != null) {
      animator.SetBool("isAnimLocked", value);
    }
    AnimLocked = value;
  }
}

public enum FacingDirection { LEFT, RIGHT }
