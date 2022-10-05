using System;
using TMPro;
using UnityEngine;

public class FadeByDistance : MonoBehaviour {
  public Transform player;
  public Vector2 opaqueDist, fadeInterval;

  void Update() {
    byte opacity = 255;
    var diff = player.position - transform.position;
    var (diffX, diffY) = (Math.Abs(diff.x), Math.Abs(diff.y));
    if(diffX > opaqueDist.x) {
      var percent = (diffX - opaqueDist.x) / fadeInterval.x;
      opacity = Math.Min(opacity, GetOpacity(percent));
    }
    if(diffY > opaqueDist.y) {
      var percent = (diffY - opaqueDist.y) / fadeInterval.y;
      opacity = Math.Min(opacity, GetOpacity(percent));
    }
    SetAlpha(opacity);
  }

  byte GetOpacity(float percent) => (byte)(Math.Max(1 - percent, 0) * 255);

  void SetAlpha(byte alpha) {
    foreach (var sprite in GetComponentsInChildren<SpriteRenderer>()) {
      var color = sprite.color;
      color.a = alpha / 255f;
      sprite.color = color;
    }
    foreach (var tmp in GetComponentsInChildren<TextMeshProUGUI>()) {
      var color = tmp.faceColor;
      color.a = alpha;
      tmp.color = color;
    }
  }
}
