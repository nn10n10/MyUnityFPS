using UnityEngine;
using System.Collections;

namespace Scripts.Weapon
{
   public abstract class Firearms : MonoBehaviour , IWeapon
    {
        public Transform MuzzlePoint;
        public Transform CasingPoint;

        public Camera EyeCamera;
        public ParticleSystem MuzzleParticle;
        public ParticleSystem CasingParticle;

        public AudioSource FirearmsShootingAudioSource;
        public AudioSource FirearmsReloadAudioSource;
        public ImpactAudioData ImpactAudioData;
        public FirearmsAudioData FirearmsAudioData;
        public float FireRate;
        public int AmmoInMag = 30;
        public int MaxAmmoCarried = 120;
        public float SpreadAngle;
        public GameObject BulletPrefab;
        private IEnumerator doAimCoroutine;

        internal Animator GunAnimator;
        
        protected float lastFireTime;
        protected float OriginFOV;
        protected bool isAiming;
        protected bool IsHoldingTrigger;
        protected int CurrentAmmo;
        protected int CurrentMaxAmmoCarried;
        protected AnimatorStateInfo GunStateInfo;


        protected virtual void Awake()
        {
            CurrentAmmo = AmmoInMag;
            CurrentMaxAmmoCarried = MaxAmmoCarried;
            GunAnimator = GetComponent<Animator>();
            OriginFOV = EyeCamera.fieldOfView;
            doAimCoroutine = DoAim();
        }
        public void DoAttack()
        {
            Shooting();

        }
        protected abstract void Shooting();
        protected abstract void Reload();
        /*
        protected abstract void Aim();
        */

        protected bool isAllowShooting() 
        {
            return Time.time - lastFireTime > 1 / FireRate;
        }

        protected Vector3 CaculateSpreadOffset()
        {
          float tmp_SpreadPercet= SpreadAngle /  EyeCamera.fieldOfView; 
          return tmp_SpreadPercet * UnityEngine.Random.insideUnitCircle;
        }
        
        protected IEnumerator CheckReloadAmmoAnimationEnd()
        {
            while(true)
            {
                yield return null;
                GunStateInfo = GunAnimator.GetCurrentAnimatorStateInfo(2);
                if (GunStateInfo.IsTag("ReloadAmmo"))
                {
                    if (GunStateInfo.normalizedTime >= 0.9f)
                    {
                        int tmp_NeedAmmoCount = AmmoInMag - CurrentAmmo;
                        int tmp_RemianAmmoCount = CurrentMaxAmmoCarried - tmp_NeedAmmoCount;
                        if (tmp_RemianAmmoCount<=0)
                        {
                            CurrentAmmo += CurrentMaxAmmoCarried;
                        }
                        else
                        {
                            CurrentAmmo = AmmoInMag;
                        }

                        CurrentMaxAmmoCarried = tmp_RemianAmmoCount < 0 ? 0 : tmp_RemianAmmoCount;
                        yield break;
                    }
                }
            }

        }

        protected IEnumerator DoAim()
        {
            while (true)
            {
                yield return null;
                float tmp_CurrentFOV = 0;
                EyeCamera.fieldOfView = Mathf.SmoothDamp(EyeCamera.fieldOfView,isAiming?26:OriginFOV, ref tmp_CurrentFOV, Time.deltaTime * 2);
            }
        }

        internal void Aiming(bool _isAiming)
        {
            isAiming = _isAiming;
            GunAnimator.SetBool("Aim",isAiming);
            if (doAimCoroutine == null)
            {
                doAimCoroutine = DoAim();
                StartCoroutine(doAimCoroutine);
            }
            else
            {
                StopCoroutine(doAimCoroutine);
                doAimCoroutine = null;
                doAimCoroutine = DoAim();
                StartCoroutine(doAimCoroutine);
            }
        }

        internal void HoldTrigger()
        {
            DoAttack();
            IsHoldingTrigger = true;
        }

        internal void ReleaseTrigger()
        {
            IsHoldingTrigger = false;
        }

        internal void ReloadAmmo()
        {
            Reload();
        }
        
    } 
}
