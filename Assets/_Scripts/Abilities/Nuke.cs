using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Nuke : MonoBehaviour
{
    public Transform bombAnimation;
    public GameObject warningMarker;

    public float explosionLenght = 15;
    public float duration = 1;
    public float speed = 1;

    public float cameraShakeIntensity;
    public float ShakeTime;

    private bool canNuke = true;

    Transform bombClone;
    GameObject warningClone;

    public Color[] initialColor;
    public Color[] endColor;
    private Material[] bombMaterial;
    private Color startColor;

    private Vector3 initialWarningScale;
    private float initialHeight;

    //public void ApplyWAbilityProperties(WAbility wAbility)
    //{
    //    explosionLenght = wAbility.explosionLenght;
    //}

    private void Start()
    {
        Vector3 warningPosition = new Vector3(transform.position.x, 0, transform.position.z);
        warningClone = Instantiate(warningMarker, warningPosition, Quaternion.identity);

        initialWarningScale = warningClone.transform.localScale;
        initialHeight = transform.position.y;

        startColor = initialColor[Random.Range(0, initialColor.Length)];
        transform.GetChild(0).GetComponent<MeshRenderer>().material.color = startColor;
    }

    private void Update()
    {
        if (warningClone != null)
        {
            float height = transform.position.y;
            float scaleFactor = explosionLenght / initialHeight * (initialHeight - height);

            scaleFactor = Mathf.Max(scaleFactor, 0.1f);

            warningClone.transform.localScale = initialWarningScale * scaleFactor;
        }
    }

    private void GetAllObjectInArea()
    {
        StartCoroutine(NukeAnimation());
    }

    IEnumerator NukeAnimation()
    {
        SoundManager.PlaySound(SoundType.Nuke);
        Destroy(warningClone);

        Vector3 bombPosition = new Vector3(transform.GetChild(0).position.x, -1f, transform.GetChild(0).position.z);

        bombClone = Instantiate(bombAnimation, bombPosition, Quaternion.identity, transform);
        bombClone.transform.localScale = new Vector3(explosionLenght, explosionLenght, explosionLenght);
        Destroy(transform.GetChild(0).gameObject);
        //StartCoroutine(ScaleLerping.Instance.Scale(bombClone, bombClone.localScale, new Vector3(explosionLenght, explosionLenght, explosionLenght), duration, speed));

        MeshRenderer[] meshRenderers = bombClone.GetComponentsInChildren<MeshRenderer>();
        bombMaterial = new Material[meshRenderers.Length];

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            bombMaterial[i] = meshRenderers[i].material;
        }

        Color targetEndColor = GetEndColor(startColor);

        StartCoroutine(ColorLerpAllMaterials(bombMaterial, startColor * 2, targetEndColor, duration / 1.5f));

        yield return new WaitForSeconds(duration / 2);

        StartCoroutine(ScaleLerping.Instance.Scale(bombClone, bombClone.localScale, Vector3.zero, duration, speed));

        yield return new WaitForSeconds(duration / 2);

        Destroy(gameObject);
    }

    private IEnumerator ColorLerpAllMaterials(Material[] materials, Color startColor, Color endColor, float lerpDuration)
    {
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            foreach (Material material in materials)
            {
                material.color = Color.Lerp(startColor, endColor, timeElapsed / lerpDuration);
            }
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        foreach (Material material in materials)
        {
            material.color = endColor;
        }
    }

    private Color GetEndColor(Color startColor)
    {
        if (startColor == initialColor[0])
        {
            return endColor[0];
        } else if (startColor == initialColor[1])
        {
            return endColor[1];
        } else if (startColor == initialColor[2])
        {
            return endColor[2];
        } else
        {
            return Color.white;
        }
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
