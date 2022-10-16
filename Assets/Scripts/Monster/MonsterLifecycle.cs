public class MonsterLifecycle : EntityLifecycle {
  protected override void Start() {
    base.Start();
  }

  protected override void OnSpawnAnimBegin() {
    base.OnSpawnAnimBegin();
  }

  protected override void OnSpawnAnimEnd() {
    base.OnSpawnAnimEnd();
  }

  protected override void OnDeathAnimBegin() {
    base.OnDeathAnimBegin();
  }

  protected override void OnDeathAnimEnd() {
    base.OnDeathAnimEnd();
    Destroy(gameObject);
  }
}
