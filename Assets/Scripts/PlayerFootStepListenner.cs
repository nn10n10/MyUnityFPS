using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootStepListenner : MonoBehaviour
{
    public FootStepAudioData FootStepAudioData;
    public AudioSource FootStepAudioSource;
    public LayerMask layerMask;

    private CharacterController characterController;
    private Transform footstepTransform;
    private float nextPlayTime ;
    public enum State{
        Idle,
        Walk,
        Springting,
        Crouching,
        Others
    }
    public State characterState;
    private object c;

    //1.角色移动或者发生较大幅度动作发出声音
    //2.Physic API检测移动
    //3.播放声音

    private void Start() {
        characterController = GetComponent<CharacterController>();
        footstepTransform = transform;
    }

    private State GetCharacterState()
    {
        return characterState;
    }

    private void FixedUpdate() {
        if (characterController.isGrounded)
        {
            if (characterController.velocity.normalized.magnitude>=0.1f)
            {
               nextPlayTime+=Time.deltaTime;

               if (characterController.velocity.magnitude>=4)
               {
                   characterState = State.Springting;
               }else if (characterController.velocity.magnitude<4 && characterController.velocity.magnitude>2)
               {
                   characterState = State.Walk;
               }
               else if (characterController.velocity.magnitude<2 && characterController.velocity.magnitude>0)
               {
                   characterState = State.Crouching;
               }

               bool tmp_isHit = Physics.Linecast(footstepTransform.position,Vector3.down*(characterController.height/2 + characterController.skinWidth-characterController.center.y),
               out RaycastHit tmp_HitInfo,layerMask); 
               if (tmp_isHit)
               {
                //检测类型
                foreach (var tmp_AudioElement in FootStepAudioData.FootStepAduios)
                {
                    if(tmp_HitInfo.collider.CompareTag(tmp_AudioElement.Tag))
                    {
                        float tmp_AudioDelay = 0;
                        switch (characterState)
                        {
                            case State.Idle:
                                tmp_AudioDelay = float.MaxValue;
                                break;
                            case State.Walk:
                                tmp_AudioDelay = tmp_AudioElement.Delay;
                                break;
                            case State.Springting:
                                tmp_AudioDelay = tmp_AudioElement.SprintingDelay;
                                break;
                            case State.Crouching:
                                tmp_AudioDelay = tmp_AudioElement.CrouchingDelay;
                                break;
                            case State.Others:
                                break;                                                        
                        }
                  
                        if(nextPlayTime>=tmp_AudioDelay){
                        // 播放移动声音
                        int tmp_AudioCount = tmp_AudioElement.AudioClips.Count;
                        int tmp_AudioIndex = UnityEngine.Random.Range(0,tmp_AudioCount);
                        AudioClip tmp_FootStepAudioClip = tmp_AudioElement.AudioClips[tmp_AudioIndex];
                        FootStepAudioSource.clip = tmp_FootStepAudioClip;
                        FootStepAudioSource.Play();
                        nextPlayTime = 0;
                        break;
                        }
                    }   
                }

               }
            }
        }
    }
}
