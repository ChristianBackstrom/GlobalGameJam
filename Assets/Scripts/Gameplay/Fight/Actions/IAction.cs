using UnityEngine;

public interface IAction
{
  void Execute(ref EntityStats actor, ref EntityStats target);
}
