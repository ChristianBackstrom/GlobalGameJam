using System;

[Serializable]
public enum ActionName
{
  Attack,
  Guard,
  ChargedStrike,
  HeavyStrike
}

public interface IAction
{
  ActionName Name { get; }

  float energyCost { get; }

  /// <summary>
  /// Called when a Charge action is fully charged and ready to execute.
  /// </summary>
  void Execute(ref EntityStats actor, ref EntityStats target);

  /// <summary>
  /// Updates the action state (charge/cooldown progress) - call each turn
  /// </summary>
  void UpdateState();

  /// <summary>
  /// Checks if the action can be initiated
  /// </summary>
  bool CanUse();
}
