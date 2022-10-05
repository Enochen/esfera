using System;
using UnityEngine;

public abstract class EntityMovement : MonoBehaviour {
  [SerializeField] protected MoveState _state;
  public virtual MoveState State { get => _state; set => _state = value; }
  public float moveSpeed = 3;
  protected Animator animator;
  protected Rigidbody2D rb;
  protected EntityController entity;

  protected virtual void Start() {
    animator = GetComponent<Animator>();
    rb = GetComponent<Rigidbody2D>();
    entity = GetComponent<EntityController>();
  }

  protected bool IsGrounded => State == MoveState.STANDING
                  || State == MoveState.CROUCHING;
  protected bool IsAirborne => State == MoveState.AIRBORNE
                  || State == MoveState.FLASH_JUMP;

  protected void Update() {
    UpdateMovement();
    UpdateAnimator();
    UpdateFacing();
  }

  protected abstract void UpdateAnimator();
  protected abstract void UpdateMovement();
  protected virtual void UpdateFacing() {
    transform.localScale = entity.transform.localScale.SetFacing(entity.facing);
  }

  public enum MoveState {
    STANDING,
    CROUCHING,
    AIRBORNE,
    FLASH_JUMP,
    CLIMBING,
  }
}
