using UnityEngine;
using UnityEngine.UI;

public class CharSelect : MonoBehaviour {
  public Toggle boxerSelect;
  public GameObject boxerDescription;

  void Start() {
    boxerSelect.onValueChanged.AddListener((value) => {
      if (value) {
        boxerDescription.SetActive(true);
      } else {
        boxerDescription.SetActive(false);
      }
    });
  }
}