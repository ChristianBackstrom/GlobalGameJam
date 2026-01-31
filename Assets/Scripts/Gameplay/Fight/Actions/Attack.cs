using UnityEngine;

public class Attack : IAction
{
  public ActionName Name => ActionName.Attack;

  public float energyCost => 10f;

  public void Execute(ref EntityStats actor, ref EntityStats target)
  {
    float damage = actor.nutriments.proteins;
    target.TakeDamage(damage);
  }

  public void UpdateState()
  {
    // Instant actions don't need state updates
  }


  public bool CanUse() => true;

  public void Reset()
  {
    // No state to reset for Attack
  }
}
