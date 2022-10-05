
using System;
using UnityEngine;

public class HPController : MonoBehaviour {
  [SerializeField] private int hp;
  [SerializeField] private int maxHP;
  public event Action HPChangeEvent;
  public event Action MaxHPChangeEvent;
  public event Action DeathEvent;
  public event Action<GameObject, Vector2> HitEvent;
  public bool isInvincible = false;

  public bool GetHit(GameObject source, Vector2 knockbackForce, int damage, int lines = 1, bool respectInvincibility = true) {
    if (isInvincible && respectInvincibility || HP == 0) return false;
    HP = Math.Max(0, HP - lines * damage);
    DeathCheck();
    HitEvent?.Invoke(source, knockbackForce);
    return true;
  }

  private void DeathCheck() {
    if (HP > 0) return;
    DeathEvent?.Invoke();
  }

  public int HP {
    get => hp;
    set {
      hp = value;
      HPChangeEvent?.Invoke();
    }
  }

  public int MaxHP {
    get => maxHP;
    set {
      maxHP = value;
      MaxHPChangeEvent?.Invoke();
    }
  }
}
