using System;
using TMPro;
using UnityEngine;

public class AIFightController : MonoBehaviour
{
    [SerializeField] private TMP_Text enemyHealthText;

    // AI fight logic to be implemented
    public void OnEnable()
    {
        FightManagerSingleton.OnFightStarted += HandleFightStarted;
        FightManagerSingleton.OnFightEnded += HandleFightEnded;
        FightManagerSingleton.OnTurnResolved += HandleTurnResolved;
        FightManagerSingleton.OnFightStateChanged += OnFightStateChanged;
    }

    public void OnDisable()
    {
        FightManagerSingleton.OnFightStarted -= HandleFightStarted;
        FightManagerSingleton.OnFightEnded -= HandleFightEnded;
        FightManagerSingleton.OnTurnResolved -= HandleTurnResolved;
        FightManagerSingleton.OnFightStateChanged -= OnFightStateChanged;
    }


    private void HandleFightStarted()
    {
        enemyHealthText.text = FightManagerSingleton.Instance.CurrentEnemy.nutriments.fat.ToString();
    }

    private void HandleFightEnded()
    {
    }

    private void HandleTurnResolved()
    {
        enemyHealthText.text = FightManagerSingleton.Instance.CurrentEnemy.nutriments.fat.ToString();
    }

    private void OnFightStateChanged(FightManagerSingleton.FightState state)
    {
        if (state == FightManagerSingleton.FightState.EnemyTurn)
        {
            // Simple AI: Attack the player
            var enemy = FightManagerSingleton.Instance.CurrentEnemy;
            var player = PlayerManagerSingleton.Instance.GetPlayerEntity();
            var attackAction = new Attack();

            FightManagerSingleton.Instance.ResolveTurn(ref enemy, ref player.stats, attackAction);

            // No need to write enemy back - enemy is the attacker, not being modified
        }
    }
}
