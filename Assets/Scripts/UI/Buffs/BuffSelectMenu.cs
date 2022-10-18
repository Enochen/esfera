using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuffSelectMenu : MonoBehaviour {
  public GameObject selected;
  public GameObject[] buffCards = new GameObject[3];
  public Button confirmButton;

  public void Start() {
    foreach (var card in buffCards) {
      var toggle = card.GetComponent<Toggle>();
      toggle.onValueChanged.AddListener((value) => {
        if (value) {
          selected = card;
          confirmButton.interactable = true;
        } else {
          selected = null;
          confirmButton.interactable = false;
        }
      });
    }
  }

  void OnEnable() {
    confirmButton.interactable = false;
  }

  public void SetCards(IEnumerable<Buff> buffs) {
    foreach (var (card, buff) in buffCards.Zip(buffs, (c, b) => (c, b))) {
      card.GetComponent<BuffCard>().SetBuff(buff);
    }
  }
}