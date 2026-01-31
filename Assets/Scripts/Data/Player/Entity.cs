using System;
using FoodDatabase;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEntityData", menuName = "Game/EntityData")]
public class EntityData : ScriptableObject
{
    [Header("Entity Information")]
    public EntityStats stats;

    public void AddNutriments(Nutrients additionalNutriments)
    {
        stats.nutriments += additionalNutriments;
    }
}

[Serializable]
public struct EntityStats
{
    public string name;
    public string description;
    public Nutrients nutriments;
}
