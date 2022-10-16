using System.Collections.Generic;
using UnityEngine;

public class BuffsController : MonoBehaviour {
  public BuffSelectMenu selectMenu;
  public List<Buff> buffs;
  public List<Buff> possibleBuffs;

  public void Start() {
    GetComponent<EXPController>().LevelUpEvent += OpenMenu;
    selectMenu.confirmButton.onClick.AddListener(CloseMenu);
  }

  public void OpenMenu() {
    Time.timeScale = 0;
    selectMenu.SetCards(possibleBuffs.TakeRandom(3));
    selectMenu.gameObject.SetActive(true);
  }

  public void CloseMenu() {
    Time.timeScale = 1;
    var selectedBuff = selectMenu.selected.GetComponent<BuffCard>().Buff;
    buffs.Add(selectedBuff);
    selectMenu.gameObject.SetActive(false);
  }
}