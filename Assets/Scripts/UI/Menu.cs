using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
  readonly string titleSceneName = "Title";
  readonly string tutorialSceneName = "Tutorial";
  readonly string survivalSceneName = "Survival";
  public void ReturnToTitle() {
    SceneManager.LoadScene(titleSceneName, LoadSceneMode.Single);
  }
  public void PlayTutorial() {
    SceneManager.LoadScene(tutorialSceneName, LoadSceneMode.Single);
  }
  public void PlaySurvival() {
    SceneManager.LoadScene(survivalSceneName, LoadSceneMode.Single);
  }
  public void Quit() {
    Application.Quit();
  }
}
