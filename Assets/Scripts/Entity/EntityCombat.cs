using System;
using UnityEngine;

public class EntityCombat : MonoBehaviour {
  [Range(0, 1)] public float stance = 0;
  public int hitstunLength = 600;
  [SerializeField] private bool initWithSpawnAnim = true;
  protected Animator animator;
  protected Rigidbody2D rb;
  protected EntityController entity;
  protected HPController hp;

  protected virtual void Start() {
    animator = GetComponent<Animator>();
    rb = GetComponent<Rigidbody2D>();
    entity = GetComponent<EntityController>();
    hp = GetComponent<HPController>();

    hp.HitEvent += OnHit;
    hp.DeathEvent += () => animator.SetTrigger("getKilled");

    foreach (var behavior in animator.GetBehaviours<HandleOnTransition>()) {
      behavior.OnStateEnterEvent += (state) => {
        if (state.IsTag("Die")) OnDeathAnimBegin();
        if (state.IsTag("Spawn")) OnSpawnAnimBegin();
      };
      behavior.OnStateExitEvent += (state) => {
        if (state.IsTag("Die")) OnDeathAnimEnd();
        if (state.IsTag("Spawn")) OnSpawnAnimEnd();
      };
    }

    if (initWithSpawnAnim) {
      Spawn();
    } else {
      OnSpawnAnimEnd();
    }
  }

  protected virtual void OnHit(GameObject origin, Vector2 knockback) {
    if (stance < 1) {
      if (!entity.hitstunned) {
        entity.hitstunned = true;
        Util.ExecuteAfterTime(() => entity.hitstunned = false, hitstunLength);
      }
      animator.SetTrigger("getHit");
      var knockbackEffect = 1 - stance;
      var direction = new Vector2(Math.Sign(transform.position.x - origin.transform.position.x), 1);
      rb.velocity = new(0, Math.Min(rb.velocity.y, 0));
      rb.AddForce(Util.HadamardProduct(knockback, direction) * knockbackEffect, ForceMode2D.Impulse);
    }
  }

  public void Spawn() {
    animator.SetTrigger("getSpawned");
  }

  protected virtual void OnSpawnAnimBegin() {
    entity.isDead = true;
    rb.velocity = Vector2.zero;
  }

  protected virtual void OnSpawnAnimEnd() {
    entity.isDead = false;
  }

  protected virtual void OnDeathAnimBegin() {
    entity.isDead = true;
    rb.velocity = Vector2.zero;
  }

  protected virtual void OnDeathAnimEnd() { }
}
