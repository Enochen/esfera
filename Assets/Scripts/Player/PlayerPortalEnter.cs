using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPortalEnter : MonoBehaviour {
  public LayerMask portalMask;
  public Portal currentPortal;
  InputManager controls;

  void Start() {
    controls = GetComponent<InputManager>();
  }

  void Update() {
    if (currentPortal == null) return;
    if (controls.up.ConsumeTrigger()) {
      currentPortal.EnterPortal(gameObject);
    }
  }

  void OnTriggerEnter2D(Collider2D col) {
    if (col.gameObject.TryGetComponentInLayer<Portal>(portalMask, out var portal)) {
      currentPortal = portal;
    }
  }

  void OnTriggerExit2D(Collider2D col) {
    if (col.gameObject.TryGetComponentInLayer<Portal>(portalMask, out var portal)) {
      if (currentPortal == portal) {
        currentPortal = null;
      }
    }
  }
}
