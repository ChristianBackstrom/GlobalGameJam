using UnityEngine;

namespace FoodDatabase
{
  [System.Serializable]
  public struct FoodDatabaseResponse
  {
    public string code;
    public Product product;
  }

  [System.Serializable]
  public struct Product
  {
    public Nutriments nutriments;
  }

  [System.Serializable]
  public struct Nutriments
  {
    public float proteins;
    public float fat;
    public float carbohydrates;
    // This won't work with JsonUtility because JSON has "energy-kcal" with hyphen
    public float energy;

    public float energy_kcal { get { return Mathf.CeilToInt(energy / 4.184f); } }
  }
}