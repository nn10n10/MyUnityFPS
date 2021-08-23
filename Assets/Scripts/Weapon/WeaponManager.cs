using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using Scripts.Weapon;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Firearms MainWeapon;
    public Firearms SecondearyWeapon;
    public Transform WorldCameraTransform;
    private Firearms carriedWeapon;

    public List<Firearms> Arms = new List<Firearms>();
    public float RaycastMaxDistance=2;

    public LayerMask CheckItemLayerMask;
    private IEnumerator WaitingForHolsterEndCoroutine;
    

    [SerializeField]private FPCharacterControllerMovement FpCharacterControllerMovement;
    private void Start()
    {
        if (MainWeapon)
        {
            carriedWeapon = MainWeapon;
            FpCharacterControllerMovement.SetupAnimator(carriedWeapon.GunAnimator);
        }
    }

    private void Update()
    {
        CheckItem();
        if (!carriedWeapon) return;

        SwapWeapon();
        if (Input.GetMouseButton(0))
        {
            carriedWeapon.HoldTrigger();
        }

        if (Input.GetMouseButtonUp(0))
        {
            carriedWeapon.ReleaseTrigger();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            carriedWeapon.ReloadAmmo();
        }

        if (Input.GetMouseButtonDown(1))
        {
            carriedWeapon.Aiming(true);
        }

        if (Input.GetMouseButtonUp(1))
        {
            carriedWeapon.Aiming(false);
        }
    }

    private void SwapWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (MainWeapon==null) return;
            if (carriedWeapon==MainWeapon) return;
            if (carriedWeapon.gameObject.activeInHierarchy)
            {
                startWaitingForHolsterEndEndCoroutine();
                carriedWeapon.GunAnimator.SetTrigger("holster");
            }
            else
            {
                SetupCarriedWeapon(MainWeapon);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if(SecondearyWeapon==null) return;
            if (carriedWeapon==SecondearyWeapon) return;
            if (carriedWeapon.gameObject.activeInHierarchy)
            {
                startWaitingForHolsterEndEndCoroutine();
                carriedWeapon.GunAnimator.SetTrigger("holster");
            }
            else
            {
                SetupCarriedWeapon(SecondearyWeapon);
            }

        }
    }

    private void startWaitingForHolsterEndEndCoroutine()
    {
        if (WaitingForHolsterEndCoroutine == null)
            WaitingForHolsterEndCoroutine = WaitingForHolsterEnd();
        StartCoroutine(WaitingForHolsterEndCoroutine);

    }

    private IEnumerator WaitingForHolsterEnd()
    {
        while (true)
        {
            AnimatorStateInfo tmp_AnimatorStateInfo = carriedWeapon.GunAnimator.GetCurrentAnimatorStateInfo(0);
            if (tmp_AnimatorStateInfo.IsTag("holster"))
            {
                if (tmp_AnimatorStateInfo.normalizedTime>=0.9f)
                {
                    var tmp_targetWeapon = carriedWeapon == MainWeapon ? SecondearyWeapon : MainWeapon;
                    SetupCarriedWeapon(tmp_targetWeapon);
                    WaitingForHolsterEndCoroutine = null;
                    yield break;
                }
            }
            yield return null;
        }
    }

    private void SetupCarriedWeapon(Firearms _targetWeaon)
    {
        if (carriedWeapon)
            carriedWeapon.gameObject.SetActive(false);
        carriedWeapon = _targetWeaon;
        carriedWeapon.gameObject.SetActive(true);
    }

    private void CheckItem()
    {
        bool tmp_IsItem = Physics.Raycast(WorldCameraTransform.position, 
            WorldCameraTransform.forward,
            out RaycastHit tmp_RaycastHit,
            RaycastMaxDistance,CheckItemLayerMask);
        if (tmp_IsItem)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                bool tmp_HasItem = tmp_RaycastHit.collider.TryGetComponent<BaseItem>(out BaseItem tmp_BaseItem);
                if (tmp_BaseItem)
                {
                    if (tmp_BaseItem is FirearmsItem tmp_FirearmsItem)
                    {
                        foreach (Firearms tmp_Arm in Arms)
                        {
                            if (tmp_FirearmsItem.ArmsName.CompareTo(tmp_Arm.name) == 0)
                            {
                                switch (tmp_FirearmsItem.CurrentFirearmsType)
                                {
                                    case FirearmsItem.FirearmsType.AssultRefile:
                                        MainWeapon = tmp_Arm;
                                        break;
                                    case FirearmsItem.FirearmsType.HandGun:
                                        SecondearyWeapon = tmp_Arm;
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }


                                SetupCarriedWeapon(tmp_Arm);
                            }
                        }
                    }
                }
            }
            Debug.Log(tmp_RaycastHit.collider.name);
        }
    }

}

