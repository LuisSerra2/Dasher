using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim : MonoBehaviour
{
    public Vector3 InitialScale;
    public Vector3 endScale;

    private bool canAnim = true;

    private void Update()
    {
        Animate();
    }

    private void Animate()
    {
        if (!canAnim) return;
        canAnim = false;

        transform.DOScale(endScale, 1f).OnComplete(() =>
        {
            transform.DOScale(InitialScale, 1f).OnComplete(() =>
            {
                canAnim = true;
            });
        });

    }
}
