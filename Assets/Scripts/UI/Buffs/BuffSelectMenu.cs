using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuffSelectMenu : MonoBehaviour {
  public GameObject selected;
  public GameObject[] buffCards = new GameObject[3];
  public Button confirmButton;

  public void Start() {
    confirmButton.interactable = false;
    foreach (var card in buffCards) {
      card.GetComponent<BuffCard>().onSelect.AddListener((eventData) => {
        selected = eventData.selectedObject;
        confirmButton.interactable = true;
      });
    }
  }

  public void SetCards(IEnumerable<Buff> buffs) {
    foreach (var (card, buff) in buffCards.Zip(buffs, (c, b) => (c, b))) {
      card.GetComponent<BuffCard>().SetBuff(buff);
    }
  }
}