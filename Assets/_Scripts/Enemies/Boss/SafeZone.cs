using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    public float defaultTimer = 2f;
    private float timer;
    private bool timerFinished = false;
    private bool playerInside = false;

    private BossController bossController;

    private void Start()
    {
        timer = defaultTimer;
        StartCoroutine(StartSafeZoneTimer());
    }

    private IEnumerator StartSafeZoneTimer()
    {
        yield return new WaitForSeconds(defaultTimer);
        timerFinished = true;

        if (playerInside)
        {
            SafeZoneSuccess();
        } else
        {
            bossController.OnAttack3Kill();
            PlayerController.Instance.ChangeStateOnDeath();
        }
    }

    public void SetBossController(BossController controller)
    {
        bossController = controller;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            playerInside = true;

            if (timerFinished)
            {
                SafeZoneSuccess();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            playerInside = false;
        }
    }

    private void SafeZoneSuccess()
    {
        if (bossController != null)
        {
            bossController.SpawnSafeZone();
        }

        Destroy(gameObject.transform.parent.gameObject);
    }
}
