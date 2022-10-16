using System;
using System.Threading;
using UnityEngine;

public class EntityCombat : MonoBehaviour {
  public AudioSource hitSound;
  [Range(0, 1)] public float stance = 0;
  public int hitstunLength = 600;
  public CancellationTokenSource hitstunExtender = new();
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
  }

  protected virtual void OnHit(GameObject origin, Vector2 knockback) {
    if (hitSound != null) {
      hitSound.Play();
    }
    if (stance < 1) {
      entity.hitstunned = true;
      hitstunExtender.Cancel();
      hitstunExtender = new();
      _ = Util.ExecuteAfterTime(() => entity.hitstunned = false,
                             hitstunLength,
                             cancellationToken: hitstunExtender.Token);
      animator.SetTrigger("getHit");
      var knockbackEffect = 1 - stance;
      var direction = new Vector2(Math.Sign(transform.position.x - origin.transform.position.x), 1);
      rb.velocity = new(0, Math.Min(rb.velocity.y, 0));
      rb.AddForce(Util.HadamardProduct(knockback, direction) * knockbackEffect, ForceMode2D.Impulse);
    }
  }
}
