using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KaimiraGames;
using UnityEngine;
using UnityEngine.Tilemaps;

// Idea: Director (Spawn AI) has a deck of cards (containing monster spawn
// logic) The Director gains credits over time and spends them on cards in
// intervals to spawn monsters
// Inspired by Risk of Rain spawn system
public abstract class Director : MonoBehaviour {
  public GameObject platforms;
  public GameObject player;
  public float credits = 0;
  public float baseCreditsRate = 0.3f;
  public int waveInterval = 500;
  public int lullInterval = 20000;
  public Vector2 spawnRadius = new(10, 5);
  List<Vector3> groundSpawnPositions;
  bool spawnWave;
  WeightedList<Card> cards;
  Card selectedCard;
  CancellationTokenSource loopSource;
  int SpawnInterval => spawnWave ? waveInterval : lullInterval;

  protected virtual void Start() {
    groundSpawnPositions = GenerateGroundSpawnPositions();
    cards = new(GenerateCards().Select(c => c.ToWeightedListItem()).ToList());
    SelectCard();

    loopSource = new();
    SpawnLoop(loopSource.Token);
  }

  protected virtual void Update() {
    credits += Time.deltaTime * baseCreditsRate;
  }

  protected virtual void OnDestroy() {
    loopSource.Cancel();
  }

  protected virtual bool TrySpawn() {
    if (credits < selectedCard.BaseCost) return false;
    var spawnPosition = GetRandomGroundSpawnPosition();
    spawnPosition.y += 1;
    var spawned = Spawn(spawnPosition);
    if (credits >= selectedCard.EliteCost) {
      SpawnElite(spawned);
      credits -= selectedCard.EliteCost;
    } else {
      SpawnCommon(spawned);
      credits -= selectedCard.BaseCost;
    }
    return true;
  }

  protected virtual GameObject Spawn(Vector3 spawnPosition) {
    var spawned = Instantiate(selectedCard.entityPrefab, spawnPosition, Quaternion.identity);
    spawned.GetComponent<MonsterAI>().player = player;
    return spawned;
  }

  protected virtual void SpawnCommon(GameObject entity) {
    entity.transform.localScale = new(1, 1);
    // entity.GetComponent<EntityController>().spee
  }

  protected virtual void SpawnElite(GameObject entity) {
    entity.transform.localScale = new(2, 2);
  }

  protected abstract IEnumerable<Card> GenerateCards();
  protected abstract bool IsCardValid(Card card);

  protected virtual void SelectCard() {
    while (selectedCard == null || !IsCardValid(selectedCard)) {
      selectedCard = cards.Next();
    }
  }

  protected Vector3 GetRandomGroundSpawnPosition() {
    var nearby = groundSpawnPositions.Where(p => {
      var dist = Vector3.Distance(player.transform.position, p);
      return dist > 5 && dist < 50;
    });
    return nearby.ElementAt(Random.Range(0, nearby.Count()));
  }

  protected List<Vector3> GenerateGroundSpawnPositions() {
    var tilemaps = platforms.GetComponentsInChildren<Tilemap>();
    var possibleBounds = MergeBounds(tilemaps.Select(t => t.cellBounds));
    var validPositions = new List<Vector3>();
    foreach (var pos in possibleBounds.allPositionsWithin) {
      foreach (var tilemap in tilemaps) {
        var worldPos = tilemap.CellToLocal(pos);
        if (tilemap.HasTile(pos)) {
          validPositions.Add(worldPos);
          break;
        }
      }
    }
    return validPositions;
  }

  private BoundsInt MergeBounds(IEnumerable<BoundsInt> bounds) {
    var min = bounds.Select(b => b.min).Aggregate(Vector3Int.Min);
    var max = bounds.Select(b => b.max).Aggregate(Vector3Int.Max);
    var result = new BoundsInt();
    result.SetMinMax(min, max);
    return result;
  }

  private async void SpawnLoop(CancellationToken token) {
    while (!token.IsCancellationRequested) {
      SelectCard();
      spawnWave = true;
      var success = TrySpawn();
      if (!success) {
        spawnWave = false;
      }
      await Task.Delay(SpawnInterval);
    }
  }

  public class Card {
    public GameObject entityPrefab;
    public int weight;
    public int BaseCost { get; set; }
    public int EliteCost => BaseCost * 4;

    public WeightedListItem<Card> ToWeightedListItem() {
      return new WeightedListItem<Card>(this, weight);
    }
  }
}