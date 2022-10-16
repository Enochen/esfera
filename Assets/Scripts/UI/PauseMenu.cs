using UnityEngine;

public class PauseMenu : MonoBehaviour {
  public InputManager input;
  public GameObject menu;
  bool isVisible;

  void Update() {
    if (input.pause.ConsumeTrigger()) {
      isVisible = !isVisible;
      Time.timeScale = isVisible ? 0 : 1;
      menu.SetActive(isVisible);
    }
  }
}
