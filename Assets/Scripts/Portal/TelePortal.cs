using UnityEngine;

public class TelePortal : Portal
{
    public Vector3 destination;
    public override void EnterPortal(GameObject entity) {
      entity.transform.position = destination;
      if(entity.TryGetComponent<EntityLifecycle>(out var controller)) {
        controller.Spawn();
      }
    }
}
