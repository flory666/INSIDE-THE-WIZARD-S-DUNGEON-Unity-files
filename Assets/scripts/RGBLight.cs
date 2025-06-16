using UnityEngine;
using UnityEngine.Rendering.Universal; // Needed for Light2D

public class RGBLight : MonoBehaviour
{
    public Light2D rgbLight; // Assign in Inspector or via code
    public float speed = 1f;

    private float hue = 0f;

    void Update()
    {
        // Cycle hue over time (0 to 1)
        hue += Time.deltaTime * speed;
        if (hue > 1f) hue -= 1f;

        // Convert HSV to RGB
        Color color = Color.HSVToRGB(hue, 1f, 1f);
        rgbLight.color = color;
    }
}
