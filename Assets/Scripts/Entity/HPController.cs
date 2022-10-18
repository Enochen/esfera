
using System;
using UnityEngine;

public class HPController : MonoBehaviour {
  [SerializeField] private int hp;
  [SerializeField] private int maxHP;
  public event Action HPChangeEvent;
  public event Action MaxHPChangeEvent;
  public event Action DeathEvent;
  public event Action<GameObject, Vector2> HitEvent;
  public event Action HealEvent;
  public bool isInvincible = false;
  public bool IsDead => HP == 0;
  EntityCombat combat;

  void Start() {
    combat = GetComponent<EntityCombat>();
  }

  public bool GetHit(GameObject source, Vector2 knockbackForce, int damage, int lines = 1) {
    if (isInvincible || IsDead) return false;
    var finalDamage = Math.Max(damage - combat.FinalDR(), 1);
    HP -= lines * finalDamage;
    HitEvent?.Invoke(source, knockbackForce);
    return true;
  }

  public void Heal(int amount, bool revive = false) {
    if (IsDead && !revive) return;
    HP += amount;
    HealEvent?.Invoke();
  }

  public int HP {
    get => hp;
    set {
      hp = Math.Clamp(value, 0, MaxHP);
      HPChangeEvent?.Invoke();
      if (IsDead) DeathEvent?.Invoke();
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
