using UnityEngine;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
    private Image image;
    public float speed = 1f;

    private float hue = 0f;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        hue += speed * Time.deltaTime; 
        if (hue > 1f) hue = 0f; 

        Color newColor = Color.HSVToRGB(hue, 1f, 1f); 
        image.color = newColor; 
    }
}
