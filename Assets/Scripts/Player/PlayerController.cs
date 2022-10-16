public class PlayerController : EntityController {
  protected override void Start() {
    base.Start();
    Global.myPlayer = this;
    Global.players.Add(this);
  }

  void OnDestroy() {
    Global.myPlayer = null;
    Global.players.Remove(this);
  }
}
