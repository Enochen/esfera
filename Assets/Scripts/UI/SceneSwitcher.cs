using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {
  readonly string titleSceneName = "Title";
  readonly string tutorialSceneName = "Tutorial";
  readonly string survivalSceneName = "Survival";
  public void ReturnToTitle() {
    SceneManager.LoadScene(titleSceneName, LoadSceneMode.Single);
    Time.timeScale = 1;
  }
  public void PlayTutorial() {
    SceneManager.LoadScene(tutorialSceneName, LoadSceneMode.Single);
    Time.timeScale = 1;
  }
  public void PlaySurvival() {
    SceneManager.LoadScene(survivalSceneName, LoadSceneMode.Single);
    Time.timeScale = 1;
  }
  public void Quit() {
    Application.Quit();
  }
}
