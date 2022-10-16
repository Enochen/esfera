using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHUD : MonoBehaviour {
  public SkillBox[] skillBoxes = new SkillBox[4];
  HPController hp;
  EXPController exp;
  Label levelLabel;
  ProgressBar hpBar, expBar;
  PlayerSkills skillManager;

  void Start() {
    hp = GetComponent<HPController>();
    exp = GetComponent<EXPController>();
    skillManager = GetComponent<PlayerSkills>();

    levelLabel = GetComponent<UIDocument>().rootVisualElement.Q<Label>("level-label");
    exp.LevelChangeEvent += UpdateLevel;
    UpdateLevel();

    hpBar = GetComponent<UIDocument>().rootVisualElement.Q<ProgressBar>("hp-bar");
    hp.HPChangeEvent += UpdateHP;
    UpdateHP();
    
    expBar = GetComponent<UIDocument>().rootVisualElement.Q<ProgressBar>("exp-bar");
    exp.EXPChangeEvent += UpdateEXP;
    UpdateEXP();
  }

  void Update() {
    for (var i = 0; i < skillManager.skills.Length; i++) {
      skillBoxes[i].remainingCooldown = skillManager.skills[i].RemainingCooldown;
    }
  }

  void UpdateLevel() {
    levelLabel.text = $"Lv. {exp.Level}";
  }

  void UpdateHP() {
    var currentHP = hp.HP;
    var maxHP = hp.MaxHP;
    hpBar.value = currentHP;
    hpBar.highValue = maxHP;
    hpBar.title = $"{currentHP}/{maxHP}";
  }

  void UpdateEXP() {
    var currentEXP = exp.EXP;
    var maxEXP = exp.EXPToNextLevel;
    expBar.value = currentEXP;
    expBar.highValue = maxEXP;
    expBar.title = $"EXP: {(float)(currentEXP / maxEXP):0.00}";
  }
}
