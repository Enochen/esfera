using System.Linq;
using UnityEngine;

public class PlayerHPRegen : MonoBehaviour {
  public int baseRegen;
  public int regenPeriod;
  public HPController hp;
  protected BuffsController buffsController;

  int FinalRegenAmount() {
    var buffs = buffsController.buffs.Where((buff) => buff is Peace).Count() * 0.5f;
    return (int)(baseRegen * (1 + buffs));
  }

  public void Start() {
    hp = GetComponent<HPController>();
    buffsController = GetComponent<BuffsController>();
    _ = Util.ExecuteEveryTime(() => hp.Heal(FinalRegenAmount()), () => regenPeriod);
  }
}