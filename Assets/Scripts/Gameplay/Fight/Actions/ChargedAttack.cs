using UnityEngine;

/// <summary>
/// A powerful attack that requires 2 turns of charging before execution.
/// Deals double damage when fully charged.
/// </summary>
public class ChargedAttack : IAction
{
  private const int CHARGE_TURNS = 2;
  private int currentChargeTurn = 0;
  private bool isCharging = false;

  private bool isFullyCharged => currentChargeTurn >= CHARGE_TURNS;

  public ActionName Name => ActionName.ChargedStrike;

  public float energyCost => 30f;

  public bool CanUse() => isFullyCharged || !isCharging;

  public void Execute(ref EntityStats actor, ref EntityStats target)
  {
    if (!isCharging)
    {
      // Start charging
      isCharging = true;
      currentChargeTurn = 0;
      Debug.Log($"{actor.name} begins charging a powerful attack!");
    }
    else if (isFullyCharged)
    {
      // Execute the charged attack
      float damage = actor.nutriments.proteins * 3f;
      target.TakeDamage(damage);

      // Reset state
      isCharging = false;
      currentChargeTurn = 0;

      Debug.Log($"{actor.name} unleashes a charged attack for {damage} damage!");
    }
  }

  public void UpdateState()
  {
    if (isCharging)
    {
      currentChargeTurn++;

      if (isFullyCharged)
      {
        Debug.Log("Charge complete! Attack ready to execute!");
      }
      else
      {
        Debug.Log($"Charging... ({currentChargeTurn}/{CHARGE_TURNS})");
      }
    }
  }

  public void Reset()
  {
    isCharging = false;
    currentChargeTurn = 0;
  }
}
