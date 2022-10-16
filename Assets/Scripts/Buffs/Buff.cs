using UnityEngine;

public abstract class Buff : MonoBehaviour {
  public BuffMeta meta;
}

[System.Serializable]
public struct BuffMeta {
  public string name;
  public string description;
  public Sprite icon;
}