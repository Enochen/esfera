using UnityEngine;

public class KeyHintDisplay : MonoBehaviour {
  [SerializeField] Key key;
  [SerializeField] float speed = 1;
  [SerializeField] [Range(0, 1)] float cycleOffset;
  Animator animator;

  void Start() {
    animator = GetComponent<Animator>();
    animator.SetFloat("speed", speed);
    animator.Play(GetKeyState, -1, cycleOffset);
  }

  string GetKeyState => key switch {
    Key.ARROW_UP => "ArrowUp",
    Key.ARROW_LEFT => "ArrowLeft",
    Key.ARROW_DOWN => "ArrowDown",
    Key.ARROW_RIGHT => "ArrowRight",
    Key.SPACE => "Space",
    Key.Q => "Q",
    Key.W => "W",
    Key.E => "E",
    Key.R => "R",
  };

  enum Key {
    ARROW_UP, ARROW_LEFT, ARROW_DOWN, ARROW_RIGHT,
    Q, W, E, R,
    SPACE,
  }
}
