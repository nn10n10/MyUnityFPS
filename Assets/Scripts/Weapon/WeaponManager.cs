using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using Scripts.Weapon;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public Firearms MainWeapon;
    public Firearms SecondearyWeapon;
    public Transform WorldCameraTransform;
    private Firearms carriedWeapon;
    public Text AmmoCountTextLabel;

    public List<Firearms> Arms = new List<Firearms>();
    public float RaycastMaxDistance=2;

    public GameObject corsshari;
    public LayerMask CheckItemLayerMask;
    private IEnumerator WaitingForHolsterEndCoroutine;
    public Animator MainWEaponAnimator;
    public Animator SecondWeaponAnimator;
    

    [SerializeField]private FPCharacterControllerMovement FpCharacterControllerMovement;

    private void UpdateAmmoInfo(int _ammo,int _remainingAmmo)
    {
        AmmoCountTextLabel.text = _ammo+"/"+ _remainingAmmo;
    }

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
            corsshari.gameObject.SetActive(false);
        }

        if (Input.GetMouseButtonUp(1))
        {
            carriedWeapon.Aiming(false);
            corsshari.SetActive(true);
        }

        UpdateAmmoInfo(carriedWeapon.CurrentAmmo,carriedWeapon.CurrentMaxAmmoCarried);
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
        FpCharacterControllerMovement.SetupAnimator(carriedWeapon.GunAnimator);
        _targetWeaon.CurrentAmmo = _targetWeaon.AmmoInMag;
        _targetWeaon.CurrentMaxAmmoCarried = _targetWeaon.MaxAmmoCarried;
    }


    private void CheckItem()
    {
        bool tmp_IsItem = Physics.Raycast(WorldCameraTransform.position, 
            WorldCameraTransform.forward,
            out RaycastHit tmp_RaycastHit,
            RaycastMaxDistance,CheckItemLayerMask);
        if (tmp_IsItem)
        {
            Debug.Log(tmp_RaycastHit.collider.name);
            if (Input.GetKeyDown(KeyCode.E))
            {
                bool tmp_HasItem = tmp_RaycastHit.collider.TryGetComponent<BaseItem>(out BaseItem tmp_BaseItem);
                if (tmp_BaseItem)
                {
                    PickupWeapon(tmp_BaseItem);
                    PickupAttachment(tmp_BaseItem);
                }
            }
        }
    }

    private void PickupWeapon(BaseItem _baseItem)
    {
        if (_baseItem is FirearmsItem tmp_FirearmsItem)
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

    private void PickupAttachment(BaseItem _baseItem)
    {
        if (!(_baseItem is AttachmentItem tmp_attachment)) return;
        switch (tmp_attachment.CurrentAttachmentType)
        {
            case AttachmentItem.attachmentType.Scope:
                foreach (Firearms.ScopeInfo tmp_ScopeInfo in carriedWeapon.ScopeInfos)
                {
                    if (tmp_ScopeInfo.ScopeName.CompareTo(tmp_attachment.ItemName) != 0)
                    {
                        tmp_ScopeInfo.ScopeGameObject.SetActive(false);
                        continue;
                    }
                    tmp_ScopeInfo.ScopeGameObject.SetActive(true);
                    carriedWeapon.BaseIronSight.ScopeGameObject.SetActive(false);
                    carriedWeapon.SetupCarriedScope(tmp_ScopeInfo);
                }
                break;
            case AttachmentItem.attachmentType.Other:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
    }

}

