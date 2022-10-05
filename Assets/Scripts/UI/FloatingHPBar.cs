using System;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHPBar : MonoBehaviour {
  public GameObject entity;
  public Slider hpSlider, damagedSlider;
  HPController hpController;
  void Start() {
    hpController = GetComponentInParent<HPController>();
    hpController.HPChangeEvent += UpdateHPBar;
    UpdateHPBar();

    hpController.DeathEvent += () => gameObject.SetActive(false);
  }

  void UpdateHPBar() {
    var percentage = (float)hpController.HP / hpController.MaxHP;
    hpSlider.value = Math.Max(0, percentage);
    if (hpSlider.value != damagedSlider.value) {
      Util.ExecuteAfterTime(() => damagedSlider.value = Math.Max(0, percentage), 1000);
    }
  }
}
