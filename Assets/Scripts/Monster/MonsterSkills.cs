public class MonsterSkills : EntitySkills {
  protected override void Start() {
    base.Start();
  }
  protected override void Update() {
    base.Update();
  }
  protected override Skill[] GenerateSkills() {
    return new Skill[] {
        new TrashMonsterSkill0(
          new SkillMeta {
            animTag = "Skill0",
            state = AttackState.SKILL_0,
          },
          source: gameObject,
          cooldown: 2
        ),
    };
  }

}
