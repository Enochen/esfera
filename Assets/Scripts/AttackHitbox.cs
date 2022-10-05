using UnityEngine;

public class AttackHitbox : MonoBehaviour {
  public int lines;
  public int damage;
  public Vector2 knockback = new(1, 2);
  public LayerMask enemyMask;

  void OnTriggerEnter2D(Collider2D col) {
    if (col.gameObject.TryGetComponentInLayer<HPController>(enemyMask, out var hpController)) {
      var source = GetComponentInParent<EntityController>().gameObject;
      hpController.GetHit(source, knockback, damage, lines);
    }
  }
}
