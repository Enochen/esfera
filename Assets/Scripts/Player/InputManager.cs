using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class InputManager : MonoBehaviour {
  public float moveX;
  public InputButton up = new();
  public InputButton down = new();
  public InputButton jump = new();
  public InputButton skill0 = new();
  public InputButton skill1 = new();
  public InputButton skill2 = new();
  public InputButton skill3 = new();
  public InputButton pause = new();
  public void HandleMove(CallbackContext context) {
    moveX = context.ReadValue<float>();
  }
  public void HandleDown(CallbackContext c) => down.HandleCallback(c);
  public void HandleUp(CallbackContext c) => up.HandleCallback(c);
  public void HandleJump(CallbackContext c) => jump.HandleCallback(c);
  public void HandleSkill0(CallbackContext c) => skill0.HandleCallback(c);
  public void HandleSkill1(CallbackContext c) => skill1.HandleCallback(c);
  public void HandleSkill2(CallbackContext c) => skill2.HandleCallback(c);
  public void HandleSkill3(CallbackContext c) => skill3.HandleCallback(c);
  public void HandlePause(CallbackContext c) => pause.HandleCallback(c);

}

public class InputButton {
  public bool isPressed;
  private bool trigger;
  public event Action OnPress;
  public bool CheckTrigger() {
    return isPressed && trigger;
  }
  public bool ConsumeTrigger() {
    var result = CheckTrigger();
    if(result) trigger = false;
    return result;
  }

  public void HandleCallback(CallbackContext context) {
    isPressed = context.action.IsPressed();
    OnPress?.Invoke();
    if (context.performed) {
      trigger = true;
    }
  }
}