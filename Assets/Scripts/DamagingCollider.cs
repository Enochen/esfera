using UnityEngine;

public class DamagingCollider : MonoBehaviour {
  public LayerMask collisionMask;
  public int damage;

  void OnCollisionStay2D(Collision2D col) {
    if (col.gameObject.TryGetComponentInLayer<HPController>(collisionMask, out var hp)) {
      hp.GetHit(gameObject, new Vector2(0, 5), damage);
    }
  }
}
