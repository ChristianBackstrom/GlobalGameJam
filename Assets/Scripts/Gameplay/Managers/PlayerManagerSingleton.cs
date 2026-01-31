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

    private void Awake()
    {
        InitializeSingleton();

        if (playerEntity == null)
        {
            Debug.LogError("Player EntityData is not assigned in PlayerManagerSingleton.");
        }
    }


    public EntityData GetPlayerEntity()
    {
        return playerEntity;
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
}
