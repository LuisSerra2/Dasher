using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    public List<CameraData> cameras = new List<CameraData>();

    public void ChangeCamera()
    {
        foreach (var cam in cameras)
        {
            if (cam.isCenter)
            {
                cam.isCenter = false;
                cameras[cam.cameraIndex].isCenter = true;
                cam.VirtualCamera.Priority = 10;
                cameras[cam.cameraIndex].VirtualCamera.Priority = 20;
                return;
            }
        }
    }
}

[Serializable]
public class CameraData
{
    public bool isCenter;
    public CinemachineVirtualCamera VirtualCamera;
    public int cameraIndex;
}
