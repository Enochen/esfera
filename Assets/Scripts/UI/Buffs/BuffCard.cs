using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffCard : MonoBehaviour {
  public TextMeshProUGUI nameText;
  public Image buffIcon;
  public TextMeshProUGUI descText;
  [SerializeField] private Buff buff;

  public Buff Buff { get => buff; }

  void OnEnable() {
    GetComponent<Toggle>().isOn = false;
    SetBuff(buff);
  }

  public void SetBuff(Buff buff) {
    this.buff = buff;
    nameText.text = buff.meta.name;
    buffIcon.sprite = buff.meta.icon;
    descText.text = buff.meta.description;
  }
}