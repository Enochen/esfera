using UnityEngine;

public class AttackHitbox : MonoBehaviour {
  public int lines;
  public float damage;
  public Vector2 knockback = new(1, 2);
  public LayerMask enemyMask;
  AudioSource hitSound;
  EntityController entity;
  EntityCombat combat;

  void Start() {
    hitSound = GetComponent<AudioSource>();
    entity = GetComponentInParent<EntityController>();
    combat = GetComponentInParent<EntityCombat>();
  }

  void OnTriggerEnter2D(Collider2D col) {
    if (col.gameObject.TryGetComponentInLayer<HPController>(enemyMask, out var hpController)) {
      if (hitSound != null) {
        hitSound.Play();
      }
      var finalDamage = (int)(damage * combat.FinalAP());
      var finalKnockback = knockback;
      finalKnockback.x *= entity.facing.X();
      hpController.GetHit(entity.gameObject, finalKnockback, finalDamage, lines);
    }
  }
}
