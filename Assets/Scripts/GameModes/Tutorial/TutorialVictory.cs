using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
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
    image.color = image.color.SetAlpha(0);
    while (image.color.a < 1) {
      image.color = image.color.SetAlpha(image.color.a + Time.deltaTime);
      await UniTask.Yield();
    }
    timeText.text = $"Your Time: {time:0.00} sec";
    victoryScreen.SetActive(true);
  }

  void OnTriggerEnter2D(Collider2D col) {
    if (!entered && col.CompareTag("Player")) {
      entered = true;
      uiCanvas.SetActive(false);
      uiDocument.enabled = false;
     _ = Util.ExecuteAfterTime(() => DisplayVictory(Time.timeSinceLevelLoad), 1000);
    }
  }
}
