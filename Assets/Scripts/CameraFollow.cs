using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
  public Transform player;
  public Vector3 offset;
  public float minX = float.MinValue, maxX = float.MaxValue;
  void Update() {
    var x = Math.Clamp(player.position.x + offset.x, minX, maxX);
    var y = player.position.y + offset.y;
    transform.position = new Vector3(x, y, offset.z);
  }
}
