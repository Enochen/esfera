using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using KaimiraGames;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

// Director (Spawn AI) has a deck of cards (possible monsters to spawn) The
// Director gains credits over time and spends them on cards in intervals to
// spawn monsters
// Inspired by Risk of Rain spawn system
public abstract class Director : MonoBehaviour {
  public GameObject platforms;
  public GameObject player;
  public float credits = 0;
  public float baseCreditsRate = 0.3f;
  public float timeMultiplier = 0.3f;
  public Vector2Int waveInterval = new(200, 500);
  public Vector2Int lullInterval = new(18000, 20000);
  public Vector2 spawnRadius = new(10, 5);
  List<Vector3> groundSpawnPositions;
  bool spawnWave;
  WeightedList<Card> cards;
  Card selectedCard;
  CancellationTokenSource loopSource;
  Vector2Int SpawnInterval => spawnWave
                     ? waveInterval
                     : lullInterval;

  protected virtual void Start() {
    groundSpawnPositions = GenerateGroundSpawnPositions();
    cards = new(GenerateCards().Select(c => new WeightedListItem<Card>(c, c.baseWeight)).ToList());
    SelectCard();

    loopSource = new();
    SpawnLoop(loopSource.Token);
  }

  protected virtual void Update() {
    credits += Time.deltaTime * (baseCreditsRate + Time.timeSinceLevelLoad * 0.3f);
  }

  protected virtual void OnDestroy() {
    loopSource.Cancel();
  }

  protected virtual bool TrySpawn() {
    if (credits < selectedCard.baseCost) return false;
    var spawnPosition = GetRandomGroundSpawnPosition();
    spawnPosition.y += 1;
    var spawned = Spawn(spawnPosition);

    if (credits >= selectedCard.LegendaryCost) {
      MakeLegendary(spawned);
      credits -= selectedCard.LegendaryCost;
    } else if (credits >= selectedCard.EliteCost) {
      MakeElite(spawned);
      credits -= selectedCard.EliteCost;
    } else {
      MakeCommon(spawned);
      credits -= selectedCard.CommonCost;
    }
    return true;
  }

  protected virtual GameObject Spawn(Vector3 spawnPosition) {
    var spawned = Instantiate(selectedCard.entityPrefab, spawnPosition, Quaternion.identity);
    spawned.GetComponent<MonsterAI>().player = player;
    return spawned;
  }

  protected virtual void MakeCommon(GameObject entity) {
  }

  protected virtual void MakeElite(GameObject entity) {
    entity.transform.localScale = new(1.5f, 1.5f);
    entity.GetComponent<HPController>().MaxHP *= 6;
    entity.GetComponent<HPController>().HP *= 6;
    entity.GetComponent<MonsterCombat>().baseAP *= 2;
    entity.GetComponent<MonsterController>().expAmount *= 3;
    entity.GetComponent<EntityMovement>().moveSpeed /= 1.5f;
  }

  protected virtual void MakeLegendary(GameObject entity) {
    entity.transform.localScale = new(4f, 4f);
    entity.GetComponent<HPController>().MaxHP *= 25;
    entity.GetComponent<HPController>().HP *= 25;
    entity.GetComponent<MonsterCombat>().baseAP *= 8;
    entity.GetComponent<MonsterController>().expAmount *= 10;
    entity.GetComponent<EntityMovement>().moveSpeed /= 2f;
  }

  protected abstract IEnumerable<Card> GenerateCards();
  protected abstract bool IsCardValid(Card card);

  protected virtual void SelectCard() {
    Assert.IsTrue(cards.Count > 0);
    while (selectedCard == null || !IsCardValid(selectedCard)) {
      selectedCard = cards.Next();
    }
  }

  protected Vector3 GetRandomGroundSpawnPosition() {
    var nearby = groundSpawnPositions.Where(p => {
      var dist = Vector3.Distance(player.transform.position, p);
      return dist > 5 && dist < 30;
    });
    return nearby.ElementAt(Random.Range(0, nearby.Count()));
  }

  protected List<Vector3> GenerateGroundSpawnPositions() {
    var tilemaps = platforms.GetComponentsInChildren<Tilemap>();
    var possibleBounds = MergeBounds(tilemaps.Select(t => t.cellBounds));
    var validPositions = new List<Vector3>();
    foreach (var pos in possibleBounds.allPositionsWithin) {
      foreach (var tilemap in tilemaps) {
        if (tilemap.HasTile(pos)) {
          validPositions.Add(tilemap.CellToLocal(pos));
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
      if (!TrySpawn()) {
        spawnWave = false;
      }
      await UniTask.Delay(Random.Range(SpawnInterval.x, SpawnInterval.y));
    }
  }

  public class Card {
    public GameObject entityPrefab;
    public MonsterRarity rarity;
    public int baseWeight;
    public int baseCost;
    public int CommonCost => baseCost;
    public int EliteCost => baseCost * 4;
    public int LegendaryCost => baseCost * 50;
  }

  public enum MonsterRarity {
    BASIC,
    CHAMPION,
  }
}