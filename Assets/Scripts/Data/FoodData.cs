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
    public string categories;
    public Nutrients nutriments;
  }

  [System.Serializable]
  public struct Nutrients
  {
    public float proteins;
    public float fat;
    public float carbohydrates;
    public float energy;

    public float energy_kcal { get { return Mathf.CeilToInt(energy / 4.184f); } }

    public static Nutrients operator +(Nutrients a, Nutrients b)
    {
      return new Nutrients
      {
        energy = a.energy + b.energy,
        proteins = a.proteins + b.proteins,
        fat = a.fat + b.fat,
        carbohydrates = a.carbohydrates + b.carbohydrates,
      };
    }

    public static Nutrients operator -(Nutrients a, Nutrients b)
    {
      return new Nutrients
      {
        energy = a.energy - b.energy,
        proteins = a.proteins - b.proteins,
        fat = a.fat - b.fat,
        carbohydrates = a.carbohydrates - b.carbohydrates,
      };
    }

    public static Nutrients operator *(Nutrients a, float scalar)
    {
      return new Nutrients
      {
        energy = a.energy * scalar,
        proteins = a.proteins * scalar,
        fat = a.fat * scalar,
        carbohydrates = a.carbohydrates * scalar,
      };
    }

    public static Nutrients operator /(Nutrients a, float scalar)
    {
      return new Nutrients
      {
        energy = a.energy / scalar,
        proteins = a.proteins / scalar,
        fat = a.fat / scalar,
        carbohydrates = a.carbohydrates / scalar
      };
    }

    public override string ToString()
    {
      return $"Energy: {energy_kcal} kcal, Proteins: {proteins} g, Fat: {fat} g, Carbohydrates: {carbohydrates} g";
    }
  }
}