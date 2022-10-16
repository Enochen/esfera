using UnityEngine;

public class PlayerHPRegen : MonoBehaviour {
  public int regenAmount;
  public int regenPeriod;
  public HPController hp;
  public void Start() {
    hp = GetComponent<HPController>();
    _ = Util.ExecuteEveryTime(() => hp.Heal(regenAmount), () => regenPeriod);
  }

}