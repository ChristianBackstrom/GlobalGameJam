using UnityEngine;
using UnityEngine.UI; // For RawImage
using ZXing;
using UnityEngine.InputSystem; // New Input System

public class ExampleWebcam : MonoBehaviour
{
    WebCamTexture webcamTexture;
    Color32[] data;

    // Assign these in the Inspector as needed
    public RawImage rawImage; // For UI display
    public Renderer targetRenderer; // For 3D object display (e.g., Quad)

    // Assign this InputAction in the Inspector (from an Input Actions asset)
    public InputAction snapshotAction;

    // Normalized scan area (x, y, width, height) in [0,1] relative to webcam texture
    [Header("Barcode Scan Area (normalized)")]
    public Rect scanArea = new Rect(0.25f, 0.25f, 0.5f, 0.5f);

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
        if (snapshotAction != null && snapshotAction.WasPressedThisFrame())
        {
            Debug.Log("Snapshot taken");
            
            //if (webcamTexture.width > 16 && webcamTexture.height > 16)
            //{
                data = webcamTexture.GetPixels32();
                var tex = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.RGBA32, false);
                tex.SetPixels32(data);
                tex.Apply();

                var reader = new BarcodeReader();
                var result = reader.Decode(tex.GetPixels32(), tex.width, tex.height);

                if (result != null)
                {
                    Debug.Log("Format: " + result.BarcodeFormat);
                    Debug.Log("Text: " + result.Text);
                }
                else
                {
                    Debug.Log("No barcode found");
                }

                Destroy(tex);
            //}
        }
    }

    void OnDrawGizmos()
    {
        // Only draw if webcamTexture is initialized and assigned to a RawImage or Renderer
        if (rawImage != null && rawImage.rectTransform != null)
        {
            // Get world corners of the RawImage
            Vector3[] corners = new Vector3[4];
            rawImage.rectTransform.GetWorldCorners(corners);
            // Calculate scan area corners in world space
            Vector3 bottomLeft = Vector3.Lerp(corners[0], corners[3], scanArea.y) + (corners[3] - corners[0]) * scanArea.x;
            Vector3 topLeft = Vector3.Lerp(corners[1], corners[2], scanArea.y) + (corners[2] - corners[1]) * scanArea.x;
            Vector3 bottomRight = bottomLeft + (corners[1] - corners[0]) * scanArea.width;
            Vector3 topRight = topLeft + (corners[2] - corners[1]) * scanArea.width;
            bottomLeft += (corners[3] - corners[0]) * scanArea.height;
            bottomRight += (corners[3] - corners[0]) * scanArea.height;
            // Draw rectangle
            Gizmos.color = Color.green;
            Gizmos.DrawLine(bottomLeft, bottomRight);
            Gizmos.DrawLine(bottomRight, topRight);
            Gizmos.DrawLine(topRight, topLeft);
            Gizmos.DrawLine(topLeft, bottomLeft);
        }
        // Optionally, add similar logic for 3D Renderer if needed
    }
}