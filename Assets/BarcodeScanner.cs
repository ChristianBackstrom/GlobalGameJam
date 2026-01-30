using UnityEngine;
using ZXing;

public class BarcodeScanner : MonoBehaviour
{
    public Texture2D barcodeTexture;

    void Start()
    {
        if (barcodeTexture == null)
        {
            Debug.Log("No barcode texture assigned.");
            return;
        }

        var reader = new BarcodeReader();
        var result = reader.Decode(barcodeTexture.GetPixels32(), barcodeTexture.width, barcodeTexture.height);

        if (result != null)
        {
            Debug.Log(result.BarcodeFormat.ToString());
            Debug.Log(result.Text);
        }
        else
        {
            Debug.Log("No barcode found");
        }
    }
}