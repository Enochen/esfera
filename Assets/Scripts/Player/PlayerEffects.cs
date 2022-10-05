using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour {
  public ParticleSystem dust;
  public ParticleSystem upJump;
  public ParticleSystem flashJump;

  public void TriggerDust() {
    dust.Play();
  }
  public void TriggerUpJump() {
    upJump.Play();
  }
  public void TriggerFlashJump() {
    flashJump.Play();
  }
}
