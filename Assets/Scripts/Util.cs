using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using static EntityController;

public static class Util {
  public static async void ExecuteEveryTime(Action action, int millisecondsDelay, CancellationToken token, bool delayFirst = false) {
    if (delayFirst) await Task.Delay(millisecondsDelay);
    while (!token.IsCancellationRequested) {
      action?.Invoke();
      await Task.Delay(millisecondsDelay);
    }
  }

  public static async void ExecuteAfterTime(Action action, int millisecondsDelay) {
    await Task.Delay(millisecondsDelay);
    action?.Invoke();
  }

  public static bool IsInLayerMask(this GameObject obj, LayerMask layerMask) {
    return (layerMask.value & (1 << obj.layer)) > 0;
  }

  public static Vector2 HadamardProduct(Vector2 a, Vector2 b)
  => new(a.x * b.x, a.y * b.y);

  public static int X(this FacingDirection facing) => facing switch {
    FacingDirection.LEFT => -1,
    FacingDirection.RIGHT => 1,
  };

  public static bool TryGetComponentInLayer<T>(this GameObject obj, LayerMask mask, out T component) where T:Component {
    component = null;
    return obj.IsInLayerMask(mask) && obj.TryGetComponent(out component);
  }

  public static Vector3 SetFacing(this Vector3 original, FacingDirection facing) {
    var scale = original;
    scale.x = facing.X() * Math.Abs(scale.x);
    return scale;
  }
}