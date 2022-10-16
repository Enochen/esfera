using UnityEngine;

public class BoxerSkill3 : Skill
{
  public BoxerSkill3(
    SkillMeta meta,
    GameObject source,
    int cooldown = 0) : base(meta, source, cooldown) { }

  public override void Activate() {
    base.Activate();
  }
}
