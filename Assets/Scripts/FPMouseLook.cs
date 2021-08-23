using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPMouseLook : MonoBehaviour
{
    private Transform cameraTransform;
    [SerializeField] private Transform characterTransform;
    private Vector3 cameraRotation;
    public float mouseSensitivity;
    public Vector2 MaxminAngle;
    private float currentRecoilTime;
    private Vector2 currentRecoil;
    public AnimationCurve RecoilCurve;
    public Vector2 RecoilRange;
    private CameraSpring cameraSpring;

    public float RecoilFadeOutTime = 0.3f;
    private void Start() {
        cameraTransform = transform;
        cameraSpring = GetComponentInChildren<CameraSpring>();
    }
    void Update() 
    {
        var tmp_MouseX = Input.GetAxis("Mouse X");
        var tmp_MouseY = Input.GetAxis("Mouse Y");
        cameraRotation.y += tmp_MouseX * mouseSensitivity;
        cameraRotation.x -= tmp_MouseY * mouseSensitivity;
        CalculateRecoilOffset();

        cameraRotation.y += currentRecoil.y;
        cameraRotation.x -= currentRecoil.x;
        
        cameraRotation.x = Mathf.Clamp(cameraRotation.x,MaxminAngle.x,MaxminAngle.y);
        cameraTransform.rotation = Quaternion.Euler(cameraRotation.x,cameraRotation.y,0);
        characterTransform.rotation = Quaternion.Euler(0,cameraRotation.y,0);
    }

    private void CalculateRecoilOffset()
    {
        currentRecoilTime += Time.deltaTime;
        float tmp_RecoilFraction = currentRecoilTime / RecoilFadeOutTime;
        float tmp_RecoilValue = RecoilCurve.Evaluate(tmp_RecoilFraction);
        currentRecoil = Vector2.Lerp(Vector2.zero, currentRecoil, tmp_RecoilValue);
    }

    public void FiringForTest()
    {
        currentRecoil += RecoilRange;
        cameraSpring.StartCameraSpring();
        currentRecoilTime = 0;
    }
}
