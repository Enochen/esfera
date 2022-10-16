using System;
using UnityEngine;
using static EntityController;

public class PlayerMovement : EntityMovement {
  [Serializable]
  public class JumpBoosts {
    public Vector2 basic = new(0, 6);
    public Vector2 flash = new(7, 4);
    public Vector2 up = new(3, 8);
    public Vector2 crouch = new(0, 1);
    public Vector2 climb = new(3, 2);
  }
  public JumpBoosts jumpBoosts;
  public bool isDashing;
  [SerializeField] private LayerMask allPlatformMask;
  [SerializeField] private LayerMask floorPlatformMask;
  Collider2D hitbox;
  PlayerEffects effects;
  InputManager input;
  Collider2D currPlatform;
  CompositeCollider2D currLadder;
  bool climbCooldown;
  float originalGravity;
  float airborneVelX = 0;
  public override MoveState State {
    get => _state; set {
      if (_state == value) return;
      if (value == MoveState.CLIMBING) StartClimbing();
      if (State == MoveState.CLIMBING) StopClimbing();
      _state = value;
    }
  }

  protected override void Start() {
    base.Start();
    hitbox = GetComponent<Collider2D>();
    effects = GetComponentInChildren<PlayerEffects>();
    input = GetComponent<InputManager>();
  }

  protected override void UpdateAnimator() {
    animator.SetBool("isGrounded", IsGrounded);
    animator.SetBool("isCrouching", State == MoveState.CROUCHING);
    animator.SetBool("isClimbing", State == MoveState.CLIMBING);
    animator.SetFloat("speedX", Math.Abs(rb.velocity.x));
    animator.SetFloat("speedY", Math.Abs(rb.velocity.y));
  }

  private bool TouchingGround => hitbox.IsTouchingLayers(allPlatformMask);
  private bool TouchingFloor => hitbox.IsTouchingLayers(floorPlatformMask);

  protected override void UpdateMovement() {
    float velX = rb.velocity.x;
    float velY = rb.velocity.y;

    var onGround = rb.velocity.y <= 0.01f && TouchingGround;
    if (!onGround) {
      if (IsGrounded) {
        State = MoveState.AIRBORNE;
      }
    } else {
      if (State != MoveState.CLIMBING) {
        State = input.down.isPressed
                  ? MoveState.CROUCHING
                  : MoveState.STANDING;
      }
    }

    if (isDashing) return;

    if (entity.CannotMove) {
      if (State == MoveState.STANDING) {
        rb.velocity = new(0, velY);
      }
      return;
    }

    // Set facing direction
    if (input.moveX < 0) {
      entity.facing = FacingDirection.LEFT;
    } else if (input.moveX > 0) {
      entity.facing = FacingDirection.RIGHT;
    }

    // Horizontal movement
    if (IsGrounded) {
      ResetAirborneVelX();
      velX = input.moveX * moveSpeed;
      if (State == MoveState.CROUCHING) {
        velX /= 2f;
      }
    } else if (State == MoveState.FLASH_JUMP) {
      velX = airborneVelX;
    }

    if (currLadder != null) {
      if (!climbCooldown && (input.up.isPressed || input.down.isPressed)) {
        State = MoveState.CLIMBING;
      }
      if (State == MoveState.CLIMBING) {
        velX = 0;
        velY = 0;
        if (input.up.isPressed) velY = 3;
        if (input.down.isPressed) velY = -3;
      }
    } else {
      if (State == MoveState.CLIMBING) {
        State = MoveState.AIRBORNE;
      }
    }

    // Jumping
    if (input.jump.isPressed) {
      (velX, velY) = Jump(velX, velY, input.jump.ConsumeTrigger());
      airborneVelX = velX;
    }

    rb.velocity = new(velX, velY);
  }

  public (float velX, float velY) Jump(float velX, float velY, bool pressedThisFrame) {
    switch (State) {
      case MoveState.STANDING:
        effects.TriggerDust();
        return (velX + jumpBoosts.basic.x * entity.facing.X(), jumpBoosts.basic.y);
      case MoveState.CROUCHING when pressedThisFrame && !TouchingFloor:
        DisableCollision(currPlatform);
       _ = Util.ExecuteAfterTime(() => EnableCollision(currPlatform), 500);
        return (jumpBoosts.crouch.x * entity.facing.X(), jumpBoosts.crouch.y);
      case MoveState.AIRBORNE when pressedThisFrame:
        State = MoveState.FLASH_JUMP;
        if (input.up.isPressed) {
          effects.TriggerUpJump();
          var newX = input.moveX != 0
                  ? jumpBoosts.up.x * entity.facing.X()
                  : velX;
          return (newX, jumpBoosts.up.y);
        } else {
          effects.TriggerFlashJump();
          return (jumpBoosts.flash.x * entity.facing.X(),
                  Math.Clamp(velY * 2f, jumpBoosts.flash.y / 2f, jumpBoosts.flash.y));
        }
      case MoveState.CLIMBING when pressedThisFrame:
        State = MoveState.AIRBORNE;
        return (jumpBoosts.climb.x * entity.facing.X(), jumpBoosts.climb.y);
      default:
        return (velX, velY);
    }
  }

  private void StartClimbing() {
    DisableCollision(currPlatform);
   _ = Util.ExecuteAfterTime(() => EnableCollision(currPlatform), 500);
    SetClimbingPosition(currLadder.bounds);
    originalGravity = rb.gravityScale;
    rb.gravityScale = 0;
  }

  private void StopClimbing() {
    rb.gravityScale = originalGravity;
    climbCooldown = true;
   _ = Util.ExecuteAfterTime(() => climbCooldown = false, 500);
  }

  public void ResetAirborneVelX() {
    airborneVelX = 0;
  }

  void SetClimbingPosition(Bounds ladder) {
    var pos = rb.position;
    pos.x = ladder.center.x;
    pos.y = Math.Min(ladder.max.y - 0.1f, pos.y);
    pos.y = Math.Max(ladder.min.y + 0.1f, pos.y);
    rb.position = pos;
  }

  void OnCollisionEnter2D(Collision2D col) {
    if (!col.gameObject.IsInLayerMask(allPlatformMask)) return;
    EnableCollision(currPlatform);

    if (col.gameObject.IsInLayerMask(floorPlatformMask)) return;
    currPlatform = col.collider;
  }

  private void OnTriggerStay2D(Collider2D col) {
    if (col.gameObject.CompareTag("Ladder")) {
      currLadder = col.GetComponent<CompositeCollider2D>();
    }
  }

  private void OnTriggerExit2D(Collider2D col) {
    if (col.gameObject.CompareTag("Ladder")) {
      if (currLadder != null && rb.position.y >= currLadder.bounds.max.y) {
        EnableCollision(currPlatform);
      }
      currLadder = null;
      climbCooldown = false;
    }
  }

  void EnableCollision(Collider2D col) {
    if (col == null) return;
    Physics2D.IgnoreCollision(hitbox, col, false);
  }

  void DisableCollision(Collider2D col) {
    if (col == null) return;
    Physics2D.IgnoreCollision(hitbox, col);
  }
}
