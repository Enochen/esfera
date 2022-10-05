using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TutorialVictory : MonoBehaviour {
  public GameObject fadeToBlack;
  public GameObject victoryScreen;
  public TextMeshProUGUI timeText;
  public UIDocument uiDocument;
  public GameObject uiCanvas;

  bool entered;

  async void DisplayVictory(float time) {
    var image = fadeToBlack.GetComponent<UnityEngine.UI.Image>();
    SetAlpha(image.color, 0);
    while (image.color.a < 1) {
      image.color = SetAlpha(image.color, image.color.a + Time.deltaTime);
      await Task.Yield();
    }
    timeText.text = $"Your Time: {time:0.00} sec";
    victoryScreen.SetActive(true);
  }

  Color SetAlpha(Color original, float a) {
    var newColor = original;
    newColor.a = a;
    return newColor;
  }

  void OnTriggerEnter2D(Collider2D col) {
    if (!entered && col.CompareTag("Player")) {
      entered = true;
      uiCanvas.SetActive(false);
      uiDocument.enabled = false;
      Util.ExecuteAfterTime(() => DisplayVictory(Time.timeSinceLevelLoad), 1000);
    }
  }
}
