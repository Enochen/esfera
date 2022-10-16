using UnityEngine;

public class BoxerSkill1 : Skill {
  public BoxerSkill1(
    SkillMeta meta,
    GameObject source,
    int cooldown = 0) : base(meta, source, cooldown) { }

  public override void Activate() {
    base.Activate();
  }
}
