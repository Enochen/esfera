using System;
using System.Threading;
using UnityEngine;

public class EntityLifecycle : MonoBehaviour {
  [SerializeField] private bool initWithSpawnAnim = true;
  protected Animator animator;
  protected Rigidbody2D rb;
  protected EntityController entity;
  protected HPController hp;
  public event Action DeathAnimEndEvent;

  protected virtual void Start() {
    animator = GetComponent<Animator>();
    rb = GetComponent<Rigidbody2D>();
    entity = GetComponent<EntityController>();
    hp = GetComponent<HPController>();

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

  public void Spawn() {
    animator.SetTrigger("getSpawned");
  }

  protected virtual void OnSpawnAnimBegin() {
    entity.SetAnimLocked(true);
    rb.velocity = Vector2.zero;
  }

  protected virtual void OnSpawnAnimEnd() {
    entity.SetAnimLocked(false);
  }

  protected virtual void OnDeathAnimBegin() {
    entity.SetAnimLocked(true);
    rb.velocity = Vector2.zero;
  }

  protected virtual void OnDeathAnimEnd() {
    DeathAnimEndEvent?.Invoke();
  }
}
