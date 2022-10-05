using UnityEngine;

public abstract class AbstractParallax : MonoBehaviour {
  private float length, startPosition;
  public float parallaxEffect;

  protected abstract float GetValue();

  void Start() {
    startPosition = transform.position.x;
    length = GetComponent<SpriteRenderer>().bounds.size.x;
  }

  void Update() {
    float pos = GetValue();
    float dist = pos * parallaxEffect;
    float inv = pos * (1 - parallaxEffect);

    transform.position = new Vector3(startPosition + dist, transform.position.y, transform.position.z);

    if (inv > startPosition + length) {
      startPosition += length;
    } else if (inv < startPosition - length) {
      startPosition -= length;
    }
  }
}
