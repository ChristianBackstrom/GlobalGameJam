using FoodDatabase;
using UnityEngine;

public static class OpenFoodDatabaseApi
{
  public static void FetchProductData(string barcode, System.Action<Nutriments> onSuccess, System.Action<string> onError)
  {
    // Implementation for fetching product data from Open Food Database API
    Debug.Log($"Fetching product data for barcode: {barcode}");

    // Example API call (pseudo-code)
    // string apiUrl = $"https://world.openfoodfacts.org/api/v0/product/{barcode}.json";
    // StartCoroutine(MakeApiRequest(apiUrl));

    string apiUrl = $"https://world.openfoodfacts.org/api/v0/product/{barcode}.json?fields=nutriments.proteins,nutriments.fat,nutriments.carbohydrates,nutriments.energy";
    Debug.Log($"API URL: {apiUrl}");
    MakeApiRequest(apiUrl, onSuccess, onError);
  }

  private static async void MakeApiRequest(string url, System.Action<Nutriments> onSuccess, System.Action<string> onError)
  {
    // Implementation for making the API request and handling the response
    Debug.Log($"Making API request to: {url}");
    using var httpClient = new System.Net.Http.HttpClient();

    var response = await httpClient.GetAsync(url);
    if (response.IsSuccessStatusCode)
    {
      var data = await response.Content.ReadAsStringAsync();
      Debug.Log($"Received data: {data}");
      var foodData = JsonUtility.FromJson<FoodDatabaseResponse>(data);

      onSuccess?.Invoke(foodData.product.nutriments);
    }
    else
    {
      Debug.LogError($"API request failed with status code: {response.StatusCode}");
      onError?.Invoke($"Error fetching data: {response.ReasonPhrase}");
    }
  }
}
