using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightManagerSingleton : MonoBehaviour
{
    #region Singleton Pattern
    public static FightManagerSingleton Instance { get; private set; }

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

    [SerializeField] private string fightSceneName = "FightScene";
    [SerializeField] private List<EntityData> Enemies;

    public EntityStats CurrentEnemy;

    private FightState currentFightState;
    public FightState CurrentFightState
    {
        get { return currentFightState; }
        private set
        {
            currentFightState = value;
            OnFightStateChanged?.Invoke(currentFightState);
        }
    }

    public static event Action OnFightStarted;
    public static event Action OnFightEnded;
    public static event Action OnTurnResolved;
    public static event Action<FightState> OnFightStateChanged;


    // Handle Turn Order, Fight State, etc.
    public enum FightState
    {
        PlayerTurn,
        EnemyTurn,
        Finished
    }

    private void Awake()
    {
        InitializeSingleton();
    }


    [ContextMenu("Start Debug Fight")]
    public void StartRandomFight()
    {
        if (Enemies != null && Enemies.Count > 0)
        {
            StartFight(Enemies[UnityEngine.Random.Range(0, Enemies.Count)].stats);
        }
        else
        {
            Debug.LogWarning("No enemies assigned to FightManagerSingleton.");
        }
    }

    public void StartFight(EntityStats enemy)
    {
        Debug.Log("Fight started with enemy: " + enemy.name);
        CurrentEnemy = enemy;

        // Load the fight scene and initialize fight parameters here
        var loadScene = SceneManager.LoadSceneAsync(fightSceneName, LoadSceneMode.Additive);
        loadScene.completed += OnFightSceneLoaded;
    }

    private void OnFightSceneLoaded(AsyncOperation operation)
    {
        Debug.Log("Fight scene loaded.");

        // Initialize fight parameters, UI, etc. here
        CurrentFightState = CurrentEnemy.nutriments.energy < PlayerManagerSingleton.Instance.GetPlayerNutriments().energy
        ?
        FightState.PlayerTurn :
        FightState.EnemyTurn;

        OnFightStarted?.Invoke();
    }


    // turn management, attack resolution, etc. would go here
    public void ResolveTurn(ref EntityStats attacker, ref EntityStats defender, IAction action)
    {
        if (action.CanUse())
        {
            action.Execute(ref attacker, ref defender);
        }

        // Update CurrentEnemy if the enemy was the defender (took damage)
        // This ensures UI updates see the correct value when OnTurnResolved fires
        if (CurrentFightState == FightState.PlayerTurn)
        {
            // Player is attacking, enemy is defender
            CurrentEnemy = defender;
        }
        else if (CurrentFightState == FightState.EnemyTurn)
        {
            // Enemy is attacking, player is defender (no need to update CurrentEnemy)
        }

        Debug.Log("Turn resolved by " + attacker.name + "; defender has " + defender.nutriments.fat + " fat left.");
        OnTurnResolved?.Invoke();

        if (defender.health <= 0)
        {
            EndFight(attacker);
            CurrentFightState = FightState.Finished;
            if( defender.name == PlayerManagerSingleton.Instance.GetPlayerEntity().stats.name)
            {
                SimpleSceneManager.Instance.LoadGameSceneAdditive(SimpleSceneManager.deathSceneName);
            }
            return;
        }

        // Switch turn
        if (CurrentFightState == FightState.PlayerTurn)
        {
            CurrentFightState = FightState.EnemyTurn;
        }
        else if (CurrentFightState == FightState.EnemyTurn)
        {
            CurrentFightState = FightState.PlayerTurn;
        }
    }

    private void EndFight(EntityStats attacker)
    {
        Debug.Log("Fight ended. Winner: " + attacker.name);
        // Unload the fight scene
        SceneManager.UnloadSceneAsync(fightSceneName);
        // Reset current enemy
        
        OnFightEnded?.Invoke();
    }
}
