using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.DemiLib;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    public CinemachineVirtualCamera cinemachine;
    public CinemachineBasicMultiChannelPerlin shake;
    void Awake()
    {
        instance = this;
        shake = cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void SetTarget(Transform target)
    {
        cinemachine.Follow = target;
        cinemachine.LookAt = target;
        cinemachine.m_Follow = target;
        cinemachine.m_LookAt = target;
    }

    /*public async void impulseShake(float intensity, float duration)
    {
        shake.m_AmplitudeGain = intensity;
        await Wait(duration);
        shake.m_AmplitudeGain = 0;
    }*/

    public void Shake()
    {
        shake.m_AmplitudeGain = 1;
    }

    public void Stop()
    {
        shake.m_AmplitudeGain = 0;
    }
}
