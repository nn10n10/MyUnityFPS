using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using Scripts.Weapon;
using UnityEngine;
using UnityEngine.Serialization;

public class CroshairUI : MonoBehaviour
{
    public RectTransform Reticle;
    public CharacterController CharacterController;

    public float OriginalSize;
    public float targetSize;

    private float currentSize;

    private void Update()
    {
       bool tmp_isMoving = CharacterController.velocity.magnitude > 0;
       if (tmp_isMoving)
       {
           currentSize = Mathf.Lerp(currentSize, targetSize, Time.deltaTime * 5);
       }
       else
       {
           currentSize = Mathf.Lerp(currentSize, OriginalSize, Time.deltaTime * 5);
       }

       Reticle.sizeDelta = new Vector2(currentSize,currentSize);

       
    }
}
