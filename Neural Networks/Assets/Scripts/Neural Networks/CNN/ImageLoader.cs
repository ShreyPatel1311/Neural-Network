using UnityEngine;
using UnityEngine.UI;

public class ImageLoader : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;
    [SerializeField] private float brushSize = 2f;
    [SerializeField] private float brushIntensity = 0f;

    private Texture2D texture;
    private Vector3[] corners = new Vector3[4];

    private void DrawCircle(int centerX, int centerY, float radius, float intensity)
    {
        byte[] imageData = texture.GetRawTextureData();

        for (int y = 0; y < 28; y++)
        {
            for (int x = 0; x < 28; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), new Vector2(centerX, centerY));
                if (distance <= radius)
                {
                    imageData[y * 28 + x] = 0;
                }
            }
        }

        texture.LoadRawTextureData(imageData);
        texture.Apply();
        rawImage.texture = texture; 
    }

    public void GetPixelCoordinates()
    {
        // Get the RectTransform of the RawImage
        RectTransform rectTransform = rawImage.GetComponent<RectTransform>();
        Canvas canvas = rawImage.GetComponentInParent<Canvas>();
        rectTransform.GetWorldCorners(corners);

        for (int i = 0; i < 4; i++)
        {
            corners[i] = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, corners[i]);
        }
    }

    private void Start()
    {
        rawImage = rawImage.GetComponent<RawImage>();
        texture = rawImage.texture as Texture2D;
        if(texture == null)
        {
            texture = new Texture2D(28, 28, TextureFormat.Alpha8, false);
            rawImage.texture = texture;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0)) // Left mouse button is pressed
        {
            // Convert screen point to local position in RectTransform
            Vector2 mouseP = Input.mousePosition;
            GetPixelCoordinates();

            float centerX = (mouseP.x - corners[0].x) / (corners[2].x - corners[0].x) * texture.width;
            float centerY = (mouseP.y - corners[0].y) / (corners[1].y - corners[0].y) * texture.height;
            DrawCircle((int)centerX, (int)centerY, brushSize, brushIntensity);
        }
    }
}
