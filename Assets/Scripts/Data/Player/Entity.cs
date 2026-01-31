using System;
using System.Collections.Generic;
using FoodDatabase;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEntityData", menuName = "Game/EntityData")]
public class EntityData : ScriptableObject
{
    [Header("Entity Information")]
    public EntityStats stats;
    public List<ActionName> actions; // List of action names the entity can perform

    public EntityData baseStats;



    [ContextMenu("Reset Stats")]
    public void ResetToBaseValues()
    {
        stats = baseStats.stats;
    }

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
    public bool hasGuard;
    public Nutrients nutriments;
    public int health;

    /// <summary>
    /// Applies damage to this entity, accounting for guard status.
    /// If the entity has guard, damage is halved and guard is consumed.
    /// </summary>
    public void TakeDamage(float damage)
    {
        if (hasGuard)
        {
            damage *= 0.5f;
            hasGuard = false; // Guard is consumed after blocking
        }

        health -= Mathf.CeilToInt(damage);
    }
}
