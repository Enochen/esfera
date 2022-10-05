using UnityEngine;

public abstract class Skill {
  public SkillMeta meta;
  public int cooldown;
  float lastUse = float.MinValue;
  bool initialized;

  public Skill(SkillMeta meta, int cooldown = 0) {
    this.meta = meta;
    this.cooldown = cooldown;
  }

  public float RemainingCooldown => cooldown - (Time.time - lastUse);
  public bool IsAvailable => RemainingCooldown < 0;

  protected virtual void Setup() { }

  public virtual void Activate() {
    if (!initialized) {
      Setup();
      initialized = true;
    }
    lastUse = Time.time;
  }

  public virtual void AnimationEnd() { }
}

public enum AttackState {
  IDLE = -1,
  SKILL_0,
  SKILL_1,
  SKILL_2,
  SKILL_3,
}

public struct SkillMeta {
  public InputButton input;
  public string animTag;
  public AttackState state;
  public GameObject source;
}