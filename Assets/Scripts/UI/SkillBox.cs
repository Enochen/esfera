using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillBox : MonoBehaviour {
  public float remainingCooldown;
  [SerializeField] string keyText;
  [SerializeField] Color availableColor;
  [SerializeField] Color cooldownColor;
  Image box;
  TextMeshProUGUI key;

  void Start() {
    box = GetComponentInChildren<Image>();
    key = GetComponentInChildren<TextMeshProUGUI>();
  }
  void Update() {
    if (remainingCooldown <= 0) {
      box.color = availableColor;
      key.text = keyText;
    } else {
      box.color = cooldownColor;
      key.text = remainingCooldown.ToString("0.0");
    }
  }
}
