using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SmashBall : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("asdjasidjasoidj");
        if (collision.gameObject.CompareTag("Ground"))
        {

            
            Vector3 initialPosition = collision.transform.position;

            collision.transform.DOMove(new Vector3(collision.transform.position.x, collision.transform.position.y + 2, collision.transform.position.z), .2f).OnComplete(() =>
            {
                collision.transform.DOMove(initialPosition, 1f);
            });
        }
    }
}
