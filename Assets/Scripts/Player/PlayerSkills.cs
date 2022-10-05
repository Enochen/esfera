using UnityEngine;

public class PlayerSkills : EntitySkills {
  public LayerMask terrainMask;
  public LayerMask enemyMask;
  protected InputManager controls;
  protected override void Start() {
    controls = GetComponent<InputManager>();
    base.Start();
  }
  protected override void Update() {
    base.Update();
  }

  protected override Skill[] GenerateSkills() {
    return new Skill[] {
        new BoxerSkill0 (
          new SkillMeta {
            input = controls.skill0,
            animTag = "Skill0",
            state = AttackState.SKILL_0,
            source = gameObject,
          }
        ),
        new BoxerSkill1 (
          new SkillMeta {
            input = controls.skill1,
            animTag = "Skill1",
            state = AttackState.SKILL_1,
            source = gameObject,
          },
          cooldown: 3
        ),
        new BoxerSkill2 (
          new SkillMeta {
            input = controls.skill2,
            animTag = "Skill2",
            state = AttackState.SKILL_2,
            source = gameObject,
          },
          dashDistance: 6,
          terrainMask,
          enemyMask,
          cooldown: 2
        ),
    };
  }
}