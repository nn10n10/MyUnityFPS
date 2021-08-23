using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine;

public class FPCharacterControllerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private Animator charactorAnimator;

    private Transform characterTransform;
    private Vector3 movementDirection;
    private bool isCrouched;
    private float originHeight;
    private float velocity;
    public float WalkSpeed;
    public float SprintingSpeed;
    public float JumpHeight;


    public float SprintingSpeedWhenCrouched;
    public float WalkSpeedWhenCrouched;

    public float CrouchHeight = 1f;
    public float Gravity= 9.8f;


    private void Start() {
        characterController = GetComponent<CharacterController>();
        originHeight = characterController.height;
        characterTransform = transform;
    }
    

    private void Update() {
        float tmp_CurrentSpeed = WalkSpeed;
        if(characterController.isGrounded)
        {

            var tmp_Horizontal = Input.GetAxis("Horizontal");
            var tmp_Vertical = Input.GetAxis("Vertical");
            movementDirection =  characterTransform.TransformDirection(new Vector3(tmp_Horizontal,0,tmp_Vertical)).normalized;

            if(Input.GetButtonDown("Jump"))
            {
                movementDirection.y = JumpHeight;
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                var tmp_CurrentHeight = isCrouched ? originHeight : CrouchHeight;
                StartCoroutine(DoCrouch(tmp_CurrentHeight));
                isCrouched = !isCrouched;
            }
            if (isCrouched)
            {
                tmp_CurrentSpeed = Input.GetKey(KeyCode.LeftShift) ? SprintingSpeedWhenCrouched : WalkSpeedWhenCrouched;                              
            }
            else
            {
                tmp_CurrentSpeed = Input.GetKey(KeyCode.LeftShift) ? SprintingSpeed : WalkSpeed;                
            }
            var tmp_velocity= characterController.velocity;
            tmp_velocity.y=0;
            velocity = tmp_velocity.magnitude;
            if (charactorAnimator!=null)
            {
                charactorAnimator.SetFloat("Velocity",velocity,0.25f,Time.deltaTime);
            }
        }
        movementDirection.y -= Gravity*Time.deltaTime;
        characterController.Move(movementDirection*tmp_CurrentSpeed*Time.deltaTime);
    }
    private IEnumerator DoCrouch(float _target)
    {
        float tmp_CurrentHeight = 0;
        while(Mathf.Abs(characterController.height - _target)>0.1f)
        {
            yield return null;
            characterController.height = Mathf.SmoothDamp(characterController.height,_target,ref tmp_CurrentHeight,Time.deltaTime*5);
        }
    }

    internal void SetupAnimator(Animator _animator)
    {
         charactorAnimator = _animator;
    }
}
