using UnityEngine;

public class CameraScroll : MonoBehaviour {
  public float speed;

  // Update is called once per frame
  void Update() {
    transform.position += speed * Time.deltaTime * Vector3.right;
  }
}
