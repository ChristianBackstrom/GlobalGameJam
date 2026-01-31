using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFightController : MonoBehaviour
{
    [SerializeField] private Button attackButton;
    [SerializeField] private Button defendButton;

    [SerializeField] private TMP_Text playerHealthText;

    private void OnEnable()
    {
        FightManagerSingleton.OnFightStarted += HandleFightStarted;
        FightManagerSingleton.OnFightEnded += HandleFightEnded;
        FightManagerSingleton.OnTurnResolved += HandleTurnResolved;
        FightManagerSingleton.OnFightStateChanged += OnFightStateChanged;

        attackButton.onClick.AddListener(() =>
        {

            var attackAction = new Attack();

            ExecuteActionByType(attackAction);
        });

        defendButton.onClick.AddListener(() =>
        {
            var guardAction = new Guard();
            ExecuteActionByType(guardAction);
        });
    }

    private void ExecuteActionByType<T>(T action) where T : IAction
    {
        var player = PlayerManagerSingleton.Instance.GetPlayerEntity();
        var enemy = FightManagerSingleton.Instance.CurrentEnemy;
        FightManagerSingleton.Instance.ResolveTurn(ref player.stats, ref enemy, action);

        // Write the modified enemy back to the singleton
        FightManagerSingleton.Instance.CurrentEnemy = enemy;
    }


    private void OnDisable()
    {
        FightManagerSingleton.OnFightStarted -= HandleFightStarted;
        FightManagerSingleton.OnFightEnded -= HandleFightEnded;
        FightManagerSingleton.OnTurnResolved -= HandleTurnResolved;
        FightManagerSingleton.OnFightStateChanged -= OnFightStateChanged;
    }

    private void OnFightStateChanged(FightManagerSingleton.FightState state)
    {
        if (state == FightManagerSingleton.FightState.PlayerTurn)
        {
            attackButton.interactable = true;
            defendButton.interactable = true;

            if (PlayerManagerSingleton.Instance.GetPlayerEntity().stats.hasGuard)
            {
                PlayerManagerSingleton.Instance.GetPlayerEntity().stats.hasGuard = false;
            }
        }
        else
        {
            attackButton.interactable = false;
            defendButton.interactable = false;
        }
    }

    private void HandleFightStarted()
    {
        Debug.Log("UI: Fight has started.");
        // Update UI to show fight elements

        playerHealthText.text = PlayerManagerSingleton.Instance.GetPlayerNutriments().fat.ToString();
    }

    private void HandleFightEnded()
    {
        Debug.Log("UI: Fight has ended.");
        // Update UI to hide fight elements
    }

    private void HandleTurnResolved()
    {
        Debug.Log("UI: Turn has been resolved.");
        // Update UI elements based on the new state
        playerHealthText.text = PlayerManagerSingleton.Instance.GetPlayerNutriments().fat.ToString();
    }
}
