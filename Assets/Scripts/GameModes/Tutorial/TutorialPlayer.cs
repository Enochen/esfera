using UnityEngine;

public class TutorialPlayer : MonoBehaviour {
  public Vector3 spawnPoint;

  public void Start() {
    var lifecycle = GetComponent<PlayerLifecycle>();
    var controller = GetComponent<PlayerController>();
    var hp = GetComponent<HPController>();
    lifecycle.DeathAnimEndEvent += () => {
      transform.position = spawnPoint;
      controller.facing = FacingDirection.LEFT;
      hp.HP = hp.MaxHP;
      lifecycle.Spawn();
    };
  }
}
