using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using UnityEngine;

public class DamagingCollider : MonoBehaviour {
  public LayerMask collisionMask;
  public int damage;
  public int interval;

  Dictionary<HPController, Action> damaging;

  void Start() {
    damaging = new();
  }

  void OnCollisionEnter2D(Collision2D col) {
    if (col.gameObject.TryGetComponentInLayer<HPController>(collisionMask, out var hp)) {
      if(damaging.ContainsKey(hp)) return;
      CancellationTokenSource source = new();
      Util.ExecuteEveryTime(() => {
        hp.GetHit(gameObject, new Vector2(0, 5), damage);
      }, interval, source.Token);
      damaging[hp] = source.Cancel;
    }
  }

  void OnCollisionExit2D(Collision2D col) {
    if (col.gameObject.TryGetComponent<HPController>(out var hp)) {
      damaging[hp]?.Invoke();
      damaging.Remove(hp);
    }
  }

  void OnDestroy() {
    foreach(var source in damaging.Values) {
      source?.Invoke();
    }
  }
}
