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
    private void Start() {
        cameraTransform = transform;
    }
    void Update() 
    {
        var tmp_MouseX = Input.GetAxis("Mouse X");
        var tmp_MouseY = Input.GetAxis("Mouse Y");
        cameraRotation.y += tmp_MouseX * mouseSensitivity;
        cameraRotation.x -= tmp_MouseY * mouseSensitivity;

        cameraRotation.x = Mathf.Clamp(cameraRotation.x,MaxminAngle.x,MaxminAngle.y);
        cameraTransform.rotation = Quaternion.Euler(cameraRotation.x,cameraRotation.y,0);

        characterTransform.rotation = Quaternion.Euler(0,cameraRotation.y,0);
    }
}
