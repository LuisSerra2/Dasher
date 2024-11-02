using System.Collections;
using UnityEngine;

public class ScaleLerping : Singleton<ScaleLerping>
{
    public Vector3 initialScale = Vector3.zero;
    public Vector3 endScale = new Vector3(0.6f, 0.6f, 0.6f);
    public float scalingSpeed = 2f;
    public float scalingDuration = 1f;


    public void ScaleAnimation() {
        StartCoroutine(Scale(transform, initialScale, endScale, scalingDuration, scalingSpeed));
    }

    public IEnumerator Scale(Transform obj, Vector3 initial, Vector3 end, float duration, float speed) {

        float t = 0.0f;
        float rate = (1f / duration) * speed;

        while (t < duration) {
            t += Time.deltaTime * rate;
            obj.localScale = Vector3.Lerp(initial, end, t);
            yield return null;
        }
    }

    public IEnumerator ScaleUI(RectTransform obj, Vector3 initial, Vector3 end, float duration, float speed)
    {

        float t = 0.0f;
        float rate = (1f / duration) * speed;

        while (t < duration)
        {
            t += Time.deltaTime * rate;
            obj.localScale = Vector3.Lerp(initial, end, t);
            yield return null;
        }
    }

}
