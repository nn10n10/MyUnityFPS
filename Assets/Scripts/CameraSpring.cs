using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpring : MonoBehaviour
{
    public float Frequence=25;
    public float Damp=15;
    public Vector2 MinRecoilRange;
    public Vector2 MaxRecoilRange;

    private CameraSpringUtilty _cameraSpringUtilty;
    private Transform cameraSpringTransform;

    private void Start()
    {
        _cameraSpringUtilty = new CameraSpringUtilty(Frequence,Damp);
        cameraSpringTransform = transform;
    }

    private void Update()
    {
        _cameraSpringUtilty.UpdateSpring(Time.deltaTime,Vector3.zero);
        cameraSpringTransform.localRotation=Quaternion.Slerp(cameraSpringTransform.localRotation,Quaternion.Euler(_cameraSpringUtilty.Values), 
       Time.deltaTime*10);
    }

    public void StartCameraSpring()
    {
        _cameraSpringUtilty.Values = new Vector3(0, 
            UnityEngine.Random.Range(MinRecoilRange.x, MaxRecoilRange.x),
            UnityEngine.Random.Range(MinRecoilRange.y, MaxRecoilRange.y));
    }
}
