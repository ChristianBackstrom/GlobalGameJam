using UnityEngine;

public class Guard : IAction
{
  public ActionName Name => ActionName.Guard;

  public float energyCost => 5f;

  public bool IsReady() => true;

  public void Execute(ref EntityStats actor, ref EntityStats target)
  {
    actor.hasGuard = true;
  }

  public void UpdateState()
  {
    // Instant actions don't need state updates
  }

  public bool CanUse() => true;
}
