using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class SurvivalPlayer : MonoBehaviour {
  public GameObject fadeToBlack;
  public GameObject deathScreen;
  public TextMeshProUGUI timeText;
  public UIDocument uiDocument;
  public GameObject uiCanvas;

  async void DisplayGameOver(float time) {
    var image = fadeToBlack.GetComponent<UnityEngine.UI.Image>();
    image.color = image.color.SetAlpha(0);
    while (image.color.a < 1) {
      image.color = image.color.SetAlpha(image.color.a + Time.deltaTime);
      await UniTask.Yield();
    }
    timeText.text = $"You survived for {time:0.00} sec";
    deathScreen.SetActive(true);
  }

  public void Start() {
    GetComponent<PlayerLifecycle>().DeathAnimEndEvent += () => {
      uiCanvas.SetActive(false);
      uiDocument.enabled = false;
     _ = Util.ExecuteAfterTime(() => DisplayGameOver(Time.timeSinceLevelLoad), 1000);
    };
  }
}
