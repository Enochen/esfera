using UnityEngine;

public class BoxerSkill0 : Skill {
  public BoxerSkill0(
    SkillMeta meta,
    GameObject source,
    int cooldown = 0) : base(meta, source, cooldown) { }

  public override void Activate() {
    base.Activate();
  }
}
