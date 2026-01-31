using UnityEngine;

public class Guard : IAction
{
  public void Execute(ref EntityStats actor, ref EntityStats target)
  {
    actor.hasGuard = true;
  }
}
