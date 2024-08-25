using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SmashBall : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Vector3 initialPosition = collision.transform.position;
            Color initialColor = collision.transform.GetComponent<MeshRenderer>().material.color;

            collision.transform.GetComponent<MeshRenderer>().material.color = Color.white;

            collision.transform.DOMove(new Vector3(collision.transform.position.x, collision.transform.position.y + 2, collision.transform.position.z), .2f).OnComplete(() =>
            {
                collision.transform.DOMove(initialPosition, .2f).OnComplete(() => collision.transform.GetComponent<MeshRenderer>().material.color = initialColor);
            });
        }
    }
}
