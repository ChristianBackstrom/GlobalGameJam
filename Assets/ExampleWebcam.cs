using UnityEngine;
using UnityEngine.UI; // For RawImage
using ZXing;
using UnityEngine.InputSystem; // New Input System
using System.Collections.Generic;
using TMPro;
using ZXing.Common;

public class ExampleWebcam : MonoBehaviour
{
    WebCamTexture webcamTexture;
    Color32[] data;

    // Assign these in the Inspector as needed
    public RawImage rawImage; // For UI display
    public Renderer targetRenderer; // For 3D object display (e.g., Quad)
    
    public TextMeshProUGUI NutrientInfoText; // To display nutrient info

    // Assign this InputAction in the Inspector (from an Input Actions asset)
    public InputAction snapshotAction;

    // Normalized scan area (x, y, width, height) in [0,1] relative to webcam texture
    [Header("Barcode Scan Area (normalized)")]
    public Rect scanArea = new Rect(0.25f, 0.25f, 0.5f, 0.5f);

    [Header("Barcode Formats to Detect")]
    public List<BarcodeFormat> allowedFormats = new List<BarcodeFormat> { BarcodeFormat.QR_CODE, BarcodeFormat.CODE_128 };
    
    private bool hasScannedBarcode = false;

    // Optionally assign a RectTransform for the scan area overlay
    [Header("Optional: Use RectTransform for Scan Area")]
    public RectTransform scanAreaRectTransform;

    void OnEnable()
    {
        if (snapshotAction != null)
            snapshotAction.Enable();
    }

    void OnDisable()
    {
        if (snapshotAction != null)
            snapshotAction.Disable();
    }

    void Start()
    {
        webcamTexture = new WebCamTexture();
        webcamTexture.Play();

        // Display on UI RawImage if assigned
        if (rawImage != null)
        {
            rawImage.texture = webcamTexture;
        }
        // Display on 3D object if assigned
        if (targetRenderer != null)
        {
            targetRenderer.material.mainTexture = webcamTexture;
        }
    }

    void Update()
    {
        if (hasScannedBarcode || webcamTexture == null || !webcamTexture.isPlaying)
            return;
        //if (snapshotAction != null && snapshotAction.WasPressedThisFrame())
        //{
            data = webcamTexture.GetPixels32();
            var tex = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.RGBA32, false);
            tex.SetPixels32(data);
            tex.Apply();

            // Set up decoding options with allowed formats and TryHarder
            var options = new DecodingOptions { PossibleFormats = allowedFormats, TryHarder = true };
            var reader = new BarcodeReader { Options = options };
            var result = reader.Decode(tex.GetPixels32(), tex.width, tex.height);

            // If not found, try rotated versions (90, 180, 270 degrees)
            if (result == null)
            {
                for (int i = 1; i <= 3; i++)
                {
                    var rotated = RotateTexture(tex, 90 * i);
                    result = reader.Decode(rotated.GetPixels32(), rotated.width, rotated.height);
                    Destroy(rotated);
                    if (result != null) break;
                }
            }
            Destroy(tex);
            if (result != null)
            {
                Debug.Log("Format: " + result.BarcodeFormat);
                Debug.Log("Text: " + result.Text);
                OpenFoodDatabaseApi.FetchProductData(result.Text,
                    (nut)=>
                    {
                        NutrientInfoText.text =
                            $"Fat: {nut.fat} + Player: {PlayerManagerSingleton.Instance.GetPlayerNutriments().fat} = {nut.fat + PlayerManagerSingleton.Instance.GetPlayerNutriments().fat}\n" +
                            $"Protein: {nut.proteins} + Player: {PlayerManagerSingleton.Instance.GetPlayerNutriments().proteins} = {nut.proteins + PlayerManagerSingleton.Instance.GetPlayerNutriments().proteins}\n" +
                            $"Carbohydrates: {nut.carbohydrates} + Player: {PlayerManagerSingleton.Instance.GetPlayerNutriments().carbohydrates} = {nut.carbohydrates + PlayerManagerSingleton.Instance.GetPlayerNutriments().carbohydrates}\n" +
                            $"Cals: {nut.energy_kcal} + Player: {PlayerManagerSingleton.Instance.GetPlayerNutriments().energy_kcal} = {nut.energy_kcal + PlayerManagerSingleton.Instance.GetPlayerNutriments().energy_kcal}\n" +
                            $"Energy: {nut.energy} + Player: {PlayerManagerSingleton.Instance.GetPlayerNutriments().energy} = {nut.energy + PlayerManagerSingleton.Instance.GetPlayerNutriments().energy}";
                        Debug.Log("I found: " + nut);
                        
                        PlayerManagerSingleton.Instance.AddNutrimentsToPlayer(nut);
                    },
                    (ex)=>Debug.LogError("Product nutrients not found"));
                // Stop the webcam when a barcode is found
                if (webcamTexture != null && webcamTexture.isPlaying)
                {
                    webcamTexture.Stop();
                    hasScannedBarcode = true;
                    Debug.Log("Webcam stopped after barcode found.");
                }
            }
            Destroy(tex);
        //}
    }

    void OnDrawGizmos()
    {
        // If a RectTransform is assigned for the scan area, draw its rectangle
        if (scanAreaRectTransform != null)
        {
            Vector3[] corners = new Vector3[4];
            scanAreaRectTransform.GetWorldCorners(corners);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(corners[0], corners[1]);
            Gizmos.DrawLine(corners[1], corners[2]);
            Gizmos.DrawLine(corners[2], corners[3]);
            Gizmos.DrawLine(corners[3], corners[0]);
            return;
        }
        // Fallback: draw using normalized scanArea on RawImage
        if (rawImage != null && rawImage.rectTransform != null)
        {
            RectTransform rt = rawImage.rectTransform;
            // Get the world position of the bottom left corner
            Vector3 worldBL = rt.TransformPoint(new Vector3(rt.rect.xMin, rt.rect.yMin, 0));
            Vector3 worldBR = rt.TransformPoint(new Vector3(rt.rect.xMax, rt.rect.yMin, 0));
            Vector3 worldTL = rt.TransformPoint(new Vector3(rt.rect.xMin, rt.rect.yMax, 0));
            Vector3 worldTR = rt.TransformPoint(new Vector3(rt.rect.xMax, rt.rect.yMax, 0));

            // Calculate scan area in local space
            float x = Mathf.Lerp(rt.rect.xMin, rt.rect.xMax, scanArea.x);
            float y = Mathf.Lerp(rt.rect.yMin, rt.rect.yMax, scanArea.y);
            float w = rt.rect.width * scanArea.width;
            float h = rt.rect.height * scanArea.height;

            Vector3 scanBL = rt.TransformPoint(new Vector3(x, y, 0));
            Vector3 scanBR = rt.TransformPoint(new Vector3(x + w, y, 0));
            Vector3 scanTL = rt.TransformPoint(new Vector3(x, y + h, 0));
            Vector3 scanTR = rt.TransformPoint(new Vector3(x + w, y + h, 0));

            Gizmos.color = Color.green;
            Gizmos.DrawLine(scanBL, scanBR);
            Gizmos.DrawLine(scanBR, scanTR);
            Gizmos.DrawLine(scanTR, scanTL);
            Gizmos.DrawLine(scanTL, scanBL);
        }
        // Optionally, add similar logic for 3D Renderer if needed
    }

    // Rotates a Texture2D by the given angle (must be 90, 180, or 270)
    private Texture2D RotateTexture(Texture2D original, int angle)
    {
        int w = original.width;
        int h = original.height;
        Texture2D rotated = new Texture2D(h, w, original.format, false);
        Color32[] origPixels = original.GetPixels32();
        Color32[] rotatedPixels = new Color32[origPixels.Length];
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                int newX = 0, newY = 0;
                switch (angle)
                {
                    case 90:
                        newX = h - 1 - y;
                        newY = x;
                        break;
                    case 180:
                        newX = w - 1 - x;
                        newY = h - 1 - y;
                        break;
                    case 270:
                        newX = y;
                        newY = w - 1 - x;
                        break;
                }
                rotatedPixels[newY * h + newX] = origPixels[y * w + x];
            }
        }
        rotated.SetPixels32(rotatedPixels);
        rotated.Apply();
        return rotated;
    }
}