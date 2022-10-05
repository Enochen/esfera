using UnityEngine;

public class TutorialMonsterAI : TrashMonsterAI {
  bool enraged = false;
  SpriteRenderer spriteRenderer;
  MonsterCombat combat;
  HPController hpController;
  protected override void Start() {
    base.Start();
    spriteRenderer = GetComponent<SpriteRenderer>();
    combat = GetComponent<MonsterCombat>();
    hpController = GetComponent<HPController>();
    hpController.HPChangeEvent += () => {
      if (!enraged && hpController.HP < hpController.MaxHP * 0.75f) {
        enraged = true;
        entity.SetAnimLocked(true);
        combat.stance = 0.6f;
        hpController.isInvincible = true;
        spriteRenderer.color = new Color(0, 0, 0);
        Util.ExecuteAfterTime(() => {
          entity.SetAnimLocked(false);
          movement.moveSpeed = 3;
          hpController.isInvincible = false;
          spriteRenderer.color = new Color(1, 0.26f, 0.26f);
        }, 1000);
      }
    };
  }
}
