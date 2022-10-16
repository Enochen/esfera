public class MonsterController : EntityController {
  public int expAmount;
  protected override void Start() {
    base.Start();
    GetComponent<HPController>().DeathEvent += () => {
      foreach(var player in Global.players) {
        player.GetComponent<EXPController>().AddEXP(expAmount);
      }
    };
  }
 }
