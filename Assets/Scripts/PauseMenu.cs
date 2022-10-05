using UnityEngine;

public class PauseMenu : MonoBehaviour {
  public InputManager input;
  public GameObject menu;
  bool isVisible;

  void Update() {
    if (input.pause.ConsumeTrigger()) {
      isVisible = !isVisible;
      menu.SetActive(isVisible);
    }
  }
}
