using UnityEngine;

public class Attack : IAction
{
  public void Execute(ref EntityStats actor, ref EntityStats target)
  {
    float damage = actor.nutriments.proteins;

    target.TakeDamage(damage);
  }
}
