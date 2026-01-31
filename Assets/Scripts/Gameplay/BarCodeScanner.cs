using System;
using FoodDatabase;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarCodeScanner : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI resultText;

    [SerializeField] private Nutrients nutriments;


    private void Start()
    {
        button.onClick.AddListener(OnScanButtonClicked);
    }

    private void OnScanButtonClicked()
    {
        if (string.IsNullOrEmpty(inputField.text))
        {
            resultText.text = "Please enter a barcode.";
            return;
        }

        string barcode = inputField.text;
        OpenFoodDatabaseApi.FetchProductData(barcode, OnFetchSuccess, OnFetchError);
    }

    private void OnFetchError(string obj)
    {
        resultText.text = $"Error fetching product data: \n {obj}";
    }

    private void OnFetchSuccess(Nutrients obj)
    {
        nutriments = obj;
        resultText.text = $"Product Data: \n Proteins: {obj.proteins}\n Fat: {obj.fat}\n Carbohydrates: {obj.carbohydrates} \n Energy (kcal): {obj.energy_kcal}";
    }
}
