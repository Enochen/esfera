using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public static class Util {
  public static async Task ExecuteEveryTime(
    Action action,
    int millisecondsDelay,
    CancellationToken cancellationToken = default,
    bool delayFirst = true
  ) {
    await ExecuteEveryTime(action, () => millisecondsDelay, cancellationToken, delayFirst);
  }

  public static async Task ExecuteEveryTime(
    Action action,
    Func<int> millisecondsDelay,
    CancellationToken cancellationToken = default,
    bool delayFirst = true
  ) {
    if (!delayFirst) action?.Invoke();
    while (!cancellationToken.IsCancellationRequested) {
      await ExecuteAfterTime(action, millisecondsDelay(), cancellationToken: cancellationToken);
    }
  }

  public static async Task ExecuteAfterTime(
    Action action,
    int millisecondsDelay,
    CancellationToken cancellationToken = default
  ) {
    try {
      await UniTask.Delay(millisecondsDelay, cancellationToken: cancellationToken);
      action?.Invoke();
    } catch (OperationCanceledException) { }
  }

  public static bool IsInLayerMask(this GameObject obj, LayerMask layerMask) {
    return (layerMask.value & (1 << obj.layer)) > 0;
  }

  public static int X(this FacingDirection facing) => facing switch {
    FacingDirection.LEFT => -1,
    FacingDirection.RIGHT => 1,
  };

  public static bool TryGetComponentInLayer<T>(this GameObject obj, LayerMask mask, out T component) where T : Component {
    component = null;
    return obj.IsInLayerMask(mask) && obj.TryGetComponent(out component);
  }

  public static Vector3 SetFacing(this Vector3 original, FacingDirection facing) {
    var scale = original;
    scale.x = facing.X() * Math.Abs(scale.x);
    return scale;
  }

  public static Color SetAlpha(this Color original, float a) {
    var newColor = original;
    newColor.a = a;
    return newColor;
  }

  public static IEnumerable<T> TakeRandom<T>(this IEnumerable<T> collection, int count) {
    var available = collection.Count();
    var needed = count;
    foreach (var item in collection) {
      if (UnityEngine.Random.Range(0, available--) < needed) {
        yield return item;
        if (--needed == 0) {
          break;
        }
      }
    }
  }
}