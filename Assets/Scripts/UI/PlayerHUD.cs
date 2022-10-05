using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

public class PlayerHUD : MonoBehaviour {
  public SkillBox[] skillBoxes = new SkillBox[4];
  HPController hp;
  ProgressBar hpBar;
  PlayerSkills skillManager;

  void Start() {
    hp = GetComponent<HPController>();
    skillManager = GetComponent<PlayerSkills>();

    hpBar = GetComponent<UIDocument>().rootVisualElement.Q<ProgressBar>();
    hp.HPChangeEvent += UpdateHP;
    UpdateHP();
  }

  void Update() {
    for (var i = 0; i < skillManager.skills.Length; i++) {
      skillBoxes[i].remainingCooldown = skillManager.skills[i].RemainingCooldown;
    }
  }

  void UpdateHP() {
    var currentHP = hp.HP;
    var maxHP = hp.MaxHP;
    hpBar.value = currentHP;
    hpBar.highValue = maxHP;
    hpBar.title = $"{currentHP}/{maxHP}";
  }
}
