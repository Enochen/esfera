using UnityEngine;

public class CameraParallax : AbstractParallax {
  public GameObject myCamera;

  protected override float GetValue() {
    return myCamera.transform.position.x;
  }
}
