using System;
using UnityEngine;

public class MonsterAI : MonoBehaviour {
  public GameObject player;
  public AIMovementState state = AIMovementState.WANDER;
  public Transform groundCheckPoint;
  public float groundCheckRayLength = 1;
  public LayerMask groundLayers;
  public float stayRange = 0.5f;
  public float aggroRange = 7f;
  protected Rigidbody2D rb;
  protected MonsterController entity;
  protected MonsterSkills attack;
  protected MonsterMovement movement;
  protected bool moveLock;

  Vector2 DistanceToPlayer => new(
    player.transform.position.x - entity.transform.position.x,
    player.transform.position.y - entity.transform.position.y
  );

  protected virtual void Start() {
    rb = GetComponent<Rigidbody2D>();
    entity = GetComponent<MonsterController>();
    attack = GetComponent<MonsterSkills>();
    movement = GetComponent<MonsterMovement>();
    movement.MovementEvent += TryAttack;
    movement.MovementEvent += GroundCheck;
    movement.MovementEvent += TryMove;
  }

  protected void TryAttack() {
    if (!ShouldAttack()) return;
    foreach (var skill in attack.skills) {
      if (attack.TryActivateSkill(skill)) {
        break;
      }
    }
  }

  protected void GroundCheck() {
    if (entity.CannotMove || rb.velocity.y < -0.01) return;
    var hit = Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckRayLength, groundLayers);
    if (hit.collider && !hit.collider.GetComponent<DamagingCollider>()) return;
    rb.velocity = new(0, rb.velocity.y);
    if (!moveLock) {
      moveLock = true;
      state = AIMovementState.NO_MOVEMENT;
      _ = Util.ExecuteAfterTime(FlipFacing, 500);
      _ = Util.ExecuteAfterTime(() => state = AIMovementState.WANDER, 750);
      _ = Util.ExecuteAfterTime(() => moveLock = false, 2000);
    }
  }

  protected virtual void TryMove() {
    if (entity.CannotMove || rb.velocity.y < -0.01) return;
    var dist = DistanceToPlayer;
    if (!moveLock) {
      state = Math.Abs(dist.x) switch {
        var d when d < stayRange => AIMovementState.NO_MOVEMENT,
        var d when d < aggroRange => AIMovementState.AGGRO,
        _ => AIMovementState.WANDER,
      };
    }
    rb.velocity = GetVelocity();
  }

  protected virtual Vector2 GetVelocity() => state switch {
    AIMovementState.NO_MOVEMENT => new(0, rb.velocity.y),
    AIMovementState.AGGRO => new(Math.Sign(DistanceToPlayer.x) * movement.moveSpeed, rb.velocity.y),
    AIMovementState.WANDER => new(entity.facing.X() * movement.moveSpeed, rb.velocity.y),
  };

  protected void FlipFacing() {
    entity.facing = entity.facing switch {
      FacingDirection.LEFT => FacingDirection.RIGHT,
      FacingDirection.RIGHT => FacingDirection.LEFT,
    };
  }

  protected virtual bool ShouldAttack() {
    return Math.Abs(DistanceToPlayer.x) <= 0.8;
  }

  public enum AIMovementState {
    NO_MOVEMENT,
    AGGRO,
    WANDER,
  }
}
