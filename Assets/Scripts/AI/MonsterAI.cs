using System;
using UnityEngine;

public class MonsterAI : MonoBehaviour {
  public GameObject player;
  protected Rigidbody2D rb;
  protected MonsterController entity;
  protected MonsterSkills attack;
  protected MonsterMovement movement;

  Vector2 DistanceToPlayer => new(
    player.transform.position.x - entity.transform.position.x,
    player.transform.position.y - entity.transform.position.y
  );

  protected virtual void Start() {
    rb = GetComponent<Rigidbody2D>();
    entity = GetComponent<MonsterController>();
    attack = GetComponent<MonsterSkills>();
    movement = GetComponent<MonsterMovement>();
  }

  protected virtual void Update() {
    var velX = 0f;
    if (!entity.CannotMove && rb.velocity.y >= -0.01) {
      if (ShouldAttack()) {
        attack.TryActivateSkill(attack.skills[0]);
      } else {
        var dist = DistanceToPlayer;
        if (Math.Abs(dist.x) > 0.5) {
          velX = Math.Sign(dist.x) * movement.moveSpeed;
        }
      }
    }
    movement.Velocity = new(velX, rb.velocity.y);
  }

  protected virtual bool ShouldAttack() {
    return Math.Abs(DistanceToPlayer.x) <= 0.8;
  }
}
