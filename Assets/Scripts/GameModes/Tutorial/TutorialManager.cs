using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {
  public GameObject potatorMonsterPrefab;
  public GameObject trashMonsterPrefab;
  public GameObject player;
  public List<GameObject> bars;
  GameObject potator;
  GameObject trash;
  bool entered;
  
  void Start() {
    Setup();
    player.GetComponent<PlayerLifecycle>().DeathAnimEndEvent += Setup;
  }

  void Setup() {
    if (potator != null) Destroy(potator);
    if (trash != null) Destroy(trash);
    bars.ForEach(b => b.SetActive(false));

    potator = Instantiate(potatorMonsterPrefab, transform);
    potator.GetComponent<HPController>().DeathEvent += () => {
     _ = Util.ExecuteAfterTime(() => {
        trashMonsterPrefab.GetComponent<SpriteRenderer>().sprite = null;
        trash = Instantiate(trashMonsterPrefab, transform);
        trash.GetComponent<TutorialMonsterAI>().player = player;
        trash.transform.localScale = new(1.5f, 1.5f);

        trash.GetComponent<HPController>().DeathEvent += () => {
         _ = Util.ExecuteAfterTime(() => bars.ForEach(b => b.SetActive(false)), 500);
        };
      }, 1000);
    };
  }

  void OnTriggerEnter2D(Collider2D col) {
    if (!entered && col.CompareTag("Player")) {
      entered = true;
      bars.ForEach(b => b.SetActive(true));
    }
  }
}
