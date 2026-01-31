using System;
using System.Collections.Generic;
using FoodDatabase;
using UnityEngine;

public class PlayerManagerSingleton : MonoBehaviour
{
    #region Singleton Pattern
    public static PlayerManagerSingleton Instance { get; private set; }

    public List<string> categories = new List<string>();

    private void InitializeSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    #endregion

    [SerializeField] private EntityData playerEntity;

    // Persistent action instances that maintain state across fights
    private Dictionary<ActionName, IAction> playerActions = new Dictionary<ActionName, IAction>();

    private void Awake()
    {
        InitializeSingleton();

        if (playerEntity == null)
        {
            Debug.LogError("Player EntityData is not assigned in PlayerManagerSingleton.");
        }
        else
        {
            // Initialize all player actions
            InitializePlayerActions();
        }
    }

    /// <summary>
    /// Creates persistent action instances for all actions available to the player
    /// </summary>
    private void InitializePlayerActions()
    {
        playerActions.Clear();

        foreach (var actionName in playerEntity.actions)
        {
            IAction action = actionName switch
            {
                ActionName.Attack => new Attack(),
                ActionName.Guard => new Guard(),
                ActionName.ChargedStrike => new ChargedAttack(),
                ActionName.HeavyStrike => new HeavyStrike(),
                _ => null
            };

            if (action != null)
            {
                playerActions[actionName] = action;
            }
        }
    }


    public EntityData GetPlayerEntity()
    {
        return playerEntity;
    }

    /// <summary>
    /// Gets the persistent action instance for the specified action name.
    /// Returns null if action doesn't exist.
    /// </summary>
    public IAction GetPlayerAction(ActionName actionName)
    {
        if (playerActions.TryGetValue(actionName, out IAction action))
        {
            return action;
        }

        Debug.LogWarning($"Action {actionName} not found in player actions.");
        return null;
    }

    /// <summary>
    /// Gets all persistent player action instances
    /// </summary>
    public Dictionary<ActionName, IAction> GetAllPlayerActions()
    {
        return playerActions;
    }

    public Nutrients GetPlayerNutriments()
    {
        if (playerEntity != null)
        {
            return playerEntity.stats.nutriments;
        }
        else
        {
            Debug.LogError("Player EntityData is not assigned. Cannot retrieve nutriments.");
            return new Nutrients();
        }
    }

    public void AddNutrimentsToPlayer(Nutrients additionalNutriments)
    {
        if (playerEntity != null)
        {
            playerEntity.AddNutriments(additionalNutriments);
        }
        else
        {
            Debug.LogError("Player EntityData is not assigned. Cannot add nutriments.");
        }
    }

    internal void ResetActions()
    {
        foreach (var action in playerActions.Values)
        {
            action.Reset();
        }
    }
}
