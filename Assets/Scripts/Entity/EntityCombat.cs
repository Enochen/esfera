using System;
using System.Threading;
using UnityEngine;

public class EntityCombat : MonoBehaviour {
  [Range(0, 1)] public float stance = 0;
  public int hitstunLength = 600;
  public int baseAP = 10;
  protected CancellationTokenSource hitstunExtender = new();
  protected Animator animator;
  protected Rigidbody2D rb;
  protected EntityController entity;
  protected HPController hp;

  public virtual int FinalAP() => baseAP;
  public virtual int FinalDR() => 0;

  protected virtual void Start() {
    animator = GetComponent<Animator>();
    rb = GetComponent<Rigidbody2D>();
    entity = GetComponent<EntityController>();
    hp = GetComponent<HPController>();

    hp.HitEvent += OnHit;
  }

  protected virtual void OnHit(GameObject origin, Vector2 knockback) {
    if (stance < 1) {
      entity.hitstunned = true;
      hitstunExtender.Cancel();
      hitstunExtender = new();
      _ = Util.ExecuteAfterTime(() => entity.hitstunned = false,
                                hitstunLength,
                                cancellationToken: hitstunExtender.Token);
      animator.SetTrigger("getHit");
      var knockbackEffect = 1 - stance;
      rb.velocity = new(0, Math.Min(rb.velocity.y, 0));
      rb.AddForce(knockback * knockbackEffect, ForceMode2D.Impulse);
    }
  }
}
