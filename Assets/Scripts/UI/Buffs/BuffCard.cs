using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuffCard : MonoBehaviour, ISelectHandler {
  public TextMeshProUGUI nameText;
  public Image buffIcon;
  public TextMeshProUGUI descText;
  public bool isSelected;
  public UnityEvent<BaseEventData> onSelect;
  [SerializeField] private Buff buff;

  public Buff Buff { get => buff; }

  public void OnEnable() {
    SetBuff(buff);
  }
  
  public void OnSelect(BaseEventData eventData) {
    onSelect?.Invoke(eventData);
  }

  public void SetBuff(Buff buff) {
    this.buff = buff;
    nameText.text = buff.meta.name;
    buffIcon.sprite = buff.meta.icon;
    descText.text = buff.meta.description;
  }
}