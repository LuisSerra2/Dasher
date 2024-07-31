using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnIndicator : MonoBehaviour
{
    public float indicatorDuration = 1f;
    public List<Color> indicatorColors;
    public float colorChangeInterval = 0.25f; 

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    public void ShowIndicator(GameObject indicatorPrefab, Vector3 position)
    {
        GameObject indicator = Instantiate(indicatorPrefab, transform);
        Image indicatorImage = indicator.GetComponent<Image>();
        indicatorImage.enabled = true;

        StartCoroutine(PositionIndicator(indicator, position));
        StartCoroutine(HideIndicatorAfterDelay(indicator, indicatorDuration));
        StartCoroutine(ChangeColor(indicatorImage));
    }

    private IEnumerator PositionIndicator(GameObject indicator, Vector3 targetPosition)
    {
        while (indicator != null)
        {
            Vector3 screenPosition = mainCamera.WorldToViewportPoint(targetPosition);

            if (screenPosition.z > 0 && screenPosition.x > 0 && screenPosition.x < 1 && screenPosition.y > 0 && screenPosition.y < 1)
            {
                indicator.GetComponent<Image>().enabled = false;
            } else
            {
                indicator.GetComponent<Image>().enabled = true;
                screenPosition.x = Mathf.Clamp(screenPosition.x, 0.05f, 0.95f);
                screenPosition.y = Mathf.Clamp(screenPosition.y, 0.05f, 0.95f);
                indicator.transform.position = mainCamera.ViewportToScreenPoint(screenPosition);
            }

            yield return null;
        }
    }

    private IEnumerator HideIndicatorAfterDelay(GameObject indicator, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(indicator);
    }

    private IEnumerator ChangeColor(Image indicatorImage)
    {
        int colorIndex = 0;

        while (indicatorImage != null)
        {
            indicatorImage.color = indicatorColors[colorIndex];
            colorIndex = (colorIndex + 1) % indicatorColors.Count;
            yield return new WaitForSeconds(colorChangeInterval);
        }
    }
}
