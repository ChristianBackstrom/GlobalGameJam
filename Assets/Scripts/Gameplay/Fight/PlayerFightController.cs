using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFightController : MonoBehaviour
{
    [SerializeField] private Button actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainer;

    [SerializeField] private TMP_Text playerHealthText;

    private Dictionary<IAction, Button> availableActions = new Dictionary<IAction, Button>();

    private void OnEnable()
    {
        FightManagerSingleton.OnFightStarted += HandleFightStarted;
        FightManagerSingleton.OnFightEnded += HandleFightEnded;
        FightManagerSingleton.OnTurnResolved += HandleTurnResolved;
        FightManagerSingleton.OnFightStateChanged += OnFightStateChanged;
    }

    private void ExecuteActionByType<T>(T action) where T : IAction
    {
        var player = PlayerManagerSingleton.Instance.GetPlayerEntity();
        var enemy = FightManagerSingleton.Instance.CurrentEnemy;

        FightManagerSingleton.Instance.ResolveTurn(ref player.stats, ref enemy, action);
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
            // Enable action buttons
            foreach (var actionButtons in availableActions)
            {
                if (actionButtons.Key.CanUse())
                    actionButtons.Value.interactable = true;
                else
                    actionButtons.Value.interactable = false;

                actionButtons.Key.UpdateState();
            }
        }
        else
        {
            // Disable action buttons
            foreach (var actionButtons in availableActions)
            {
                actionButtons.Value.interactable = false;
            }
        }
    }

    private void HandleFightStarted()
    {
        Debug.Log("UI: Fight has started.");
        // Update UI to show fight elements

        playerHealthText.text = PlayerManagerSingleton.Instance.GetPlayerEntity().stats.health.ToString();

        // Use persistent actions from PlayerManagerSingleton instead of creating new ones
        foreach (var actionPair in PlayerManagerSingleton.Instance.GetAllPlayerActions())
        {
            var actionName = actionPair.Key;
            var action = actionPair.Value;

            var button = Instantiate(actionButtonPrefab, actionButtonContainer);
            button.GetComponentInChildren<TMP_Text>().text = actionName.ToString();
            button.onClick.AddListener(() => ExecuteActionByType(action));
            availableActions.Add(action, button);
        }
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
        playerHealthText.text = PlayerManagerSingleton.Instance.GetPlayerEntity().stats.health.ToString();
    }
}
