using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Nuke : MonoBehaviour
{
    public Transform bombAnimation;

    public float explosionLenght = 5;
    public float duration = 1;
    public float speed = 1;

    public float cameraShakeIntensity;
    public float ShakeTime;

    private bool canNuke = true;

    Transform bombClone;

    private void GetAllObjectInArea()
    {
        StartCoroutine(NukeAnimation());
    }

    IEnumerator NukeAnimation()
    {
        bombClone = Instantiate(bombAnimation, transform.GetChild(0).transform.position, Quaternion.identity, transform);
        Destroy(transform.GetChild(0).gameObject);
        StartCoroutine(ScaleLerping.Instance.Scale(bombClone, bombClone.localScale, new Vector3(explosionLenght, explosionLenght, explosionLenght), duration, speed));

        yield return new WaitForSeconds(duration / 2);

        StartCoroutine(
                ScaleLerping.Instance.Scale(bombClone, bombClone.localScale, Vector3.zero, duration, speed));

        yield return new WaitForSeconds(duration / 2);

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (canNuke)
        {
            canNuke = false;
            CameraShake.Instance.ShakeCamera(ShakeTime, cameraShakeIntensity);
            GetAllObjectInArea();
        }
    }
}
