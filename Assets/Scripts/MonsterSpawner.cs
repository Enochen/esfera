using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour {

  public GameObject monsterPrefab;
  public int initialDelay;
  public int interval;
  bool active = true;
  // Start is called before the first frame update
  void Start() {
    Util.ExecuteAfterTime(RepeatSpawn, initialDelay);
  }

  void SpawnMonster() {
    Instantiate(monsterPrefab, transform.transform);
  }

  async void RepeatSpawn() {
    while (active) {
      SpawnMonster();
      await Task.Delay(interval);
    }
  }

  void OnDestroy() {
    active = false;
  }
}
