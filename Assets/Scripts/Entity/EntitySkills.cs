using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySkills : MonoBehaviour {
  public Skill[] skills;
  protected AttackState attackState = AttackState.IDLE;
  protected Animator animator;
  protected EntityController entity;

  protected virtual void Start() {
    animator = GetComponent<Animator>();
    entity = GetComponent<EntityController>();
    skills = GenerateSkills();
    foreach (var behavior in animator.GetBehaviours<HandleOnTransition>()) {
      behavior.OnStateEnterEvent += BeginAnimation;
      behavior.OnStateExitEvent += FinishAnimation;
    }
  }

  protected virtual void Update() {
    foreach (var skill in skills) {
      if (skill.meta.input?.isPressed ?? false) {
        TryActivateSkill(skill);
        break;
      }
    }
  }

  protected virtual Skill[] GenerateSkills() {
    return Array.Empty<Skill>();
  }

  private void BeginAnimation(AnimatorStateInfo animatorState) {
    foreach (var skill in skills) {
      if (animatorState.IsTag(skill.meta.animTag)) {
        entity.SetAnimLocked(true);
        break;
      }
    }
  }

  private void FinishAnimation(AnimatorStateInfo animatorState) {
    foreach (var skill in skills) {
      if (animatorState.IsTag(skill.meta.animTag)) {
        entity.SetAnimLocked(false);
        skill.AnimationEnd();
        UpdateAttackState(AttackState.IDLE);
        break;
      }
    }
  }

  private void UpdateAttackState(AttackState state) {
    attackState = state;
    animator.SetInteger("attackType", (int)attackState);
  }

  public void TryActivateSkill(Skill skill) {
    if (entity.CannotMove) return;
    if (attackState != AttackState.IDLE) return;
    if (!skill.IsAvailable) return;
    skill.Activate();
    UpdateAttackState(skill.meta.state);
  }
}
