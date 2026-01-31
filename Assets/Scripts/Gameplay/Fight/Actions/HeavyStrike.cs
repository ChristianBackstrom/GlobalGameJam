using UnityEngine;

/// <summary>
/// A devastating attack that executes immediately but requires 3 turns of cooldown.
/// Deals triple damage but can't be used again until cooldown expires.
/// </summary>
public class HeavyStrike : IAction
{
  private const int COOLDOWN_TURNS = 3;
  private int currentCooldownTurn = 4;
  private bool onCooldown => currentCooldownTurn < COOLDOWN_TURNS;

  public ActionName Name => ActionName.HeavyStrike;

  public float energyCost => 30f;

  public bool CanUse() => !onCooldown;

  public void Execute(ref EntityStats actor, ref EntityStats target)
  {
    // Execute immediately
    float damage = actor.nutriments.proteins * 3f;
    target.TakeDamage(damage);

    // Start cooldown
    currentCooldownTurn = 0;

    Debug.Log($"{actor.name} uses Heavy Strike for {damage} damage!");
  }

  public void UpdateState()
  {
    if (onCooldown)
    {
      currentCooldownTurn++;

      if (!onCooldown)
      {
        Debug.Log("Heavy Strike is ready to use again!");
      }
      else
      {
        Debug.Log($"Heavy Strike cooldown: {currentCooldownTurn}/{COOLDOWN_TURNS}");
      }
    }
  }
}
