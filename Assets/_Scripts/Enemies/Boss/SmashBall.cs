using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashBall : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (collision.gameObject.TryGetComponent(out TurnToNoSmashZone noSmashZone))
            {
                Vector3 initialPosition = collision.transform.position;
                Color initialColor = collision.transform.GetComponent<MeshRenderer>().material.color;

                collision.transform.GetComponent<MeshRenderer>().material.color = Color.white;

                if (!noSmashZone.IsInNoZone())
                {
                    noSmashZone.hitZone = true;
                    collision.transform.DOMove(new Vector3(collision.transform.position.x, collision.transform.position.y + 2, collision.transform.position.z), .2f).OnComplete(() =>
                    {
                        noSmashZone.hitZone = false;
                        collision.transform.DOMove(initialPosition, .2f).OnComplete(() => collision.transform.GetComponent<MeshRenderer>().material.color = initialColor);
                    });
                } else
                {
                    collision.transform.GetComponent<MeshRenderer>().material.color = initialColor;
                }
            }

            
        }
    }
}
