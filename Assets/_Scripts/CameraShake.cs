using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

public class CameraShake : Singleton<CameraShake>
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    private float timer;
    private CinemachineBasicMultiChannelPerlin channelPerlin;

    private void Start()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        StopShake();
    }
    public void ShakeCamera(float ShakeTime, float ShakeIntensity)
    {
        channelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        channelPerlin.m_AmplitudeGain = ShakeIntensity;

        timer = ShakeTime;
    }

    private void StopShake()
    {
        channelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        channelPerlin.m_AmplitudeGain = 0;

        timer = 0;
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                StopShake();
            }
        }
    }
}
