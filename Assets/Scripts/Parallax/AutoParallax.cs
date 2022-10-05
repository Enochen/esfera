using UnityEngine;

public class AutoParallax : AbstractParallax {
  public float speed;

  private float counter;

  protected override float GetValue() {
    counter += Time.deltaTime * speed;
    return counter;
  }
}
