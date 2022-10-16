
using System;
using UnityEngine;

public class EXPController : MonoBehaviour {
  private static readonly int expCostBase = 3;
  [SerializeField] private int level = 1;
  [SerializeField] private int exp;
  public event Action EXPChangeEvent;
  public event Action LevelChangeEvent;
  public event Action LevelUpEvent;
  public bool isInvincible = false;
  public int EXPToNextLevel => (int)(expCostBase * Math.Pow(level, 3));

  public void AddEXP(int delta) {
    EXP += delta;
    LevelUpCheck();
  }

  private void LevelUpCheck() {
    if (EXP < EXPToNextLevel) return;
    Level++;
    EXP -= EXPToNextLevel;
    LevelUpEvent?.Invoke();
  }

  public int EXP {
    get => exp;
    set {
      exp = value;
      EXPChangeEvent?.Invoke();
    }
  }

  public int Level {
    get => level;
    set {
      level = value;
      LevelChangeEvent?.Invoke();
    }
  }
}
