using UnityEngine;
using System.Collections;

namespace Scripts.Weapon
{
    public class AssualtRifle : Firearms
    {
        private IEnumerator reloadAmmoCheckerCoroutine;
        public GameObject BulletImpactPrefab;

        private FPMouseLook mouseLook;
        protected override void Awake()
        {
            base.Awake();
            reloadAmmoCheckerCoroutine = CheckReloadAmmoAnimationEnd();
            mouseLook = FindObjectOfType<FPMouseLook>();
        }
        protected override void Reload()
        {
            GunAnimator.SetLayerWeight(2,1);
            GunAnimator.SetTrigger(CurrentAmmo > 0 ? "ReloadLeft" : "ReloadOut");
            FirearmsReloadAudioSource.clip = CurrentAmmo > 0? FirearmsAudioData.ReloadLeft: FirearmsAudioData.ReloadOutof;
            FirearmsReloadAudioSource.Play();
            if (reloadAmmoCheckerCoroutine == null)
            {
                reloadAmmoCheckerCoroutine = CheckReloadAmmoAnimationEnd();
                StartCoroutine(reloadAmmoCheckerCoroutine);
            }
            else
            {
                StopCoroutine(reloadAmmoCheckerCoroutine);
                reloadAmmoCheckerCoroutine = null;
                reloadAmmoCheckerCoroutine = CheckReloadAmmoAnimationEnd();
                StartCoroutine(reloadAmmoCheckerCoroutine);
            }
        }

        protected override void Shooting()
        {
            if (CurrentAmmo <= 0) return;
            if (!isAllowShooting()) return;
            MuzzleParticle.Play();
            CurrentAmmo -= 1;
            GunAnimator.Play("Fire", isAiming?1:0, 0);
            FirearmsShootingAudioSource.clip = FirearmsAudioData.ShootingAudio;
            FirearmsShootingAudioSource.Play();
            CreatBullet();
            CasingParticle.Play();
            lastFireTime = Time.time;
            mouseLook.FiringForTest();
            
        }
        private void CreatBullet()
        {
            GameObject tmp_Bullet = Instantiate(BulletPrefab, MuzzlePoint.position, MuzzlePoint.rotation);
            tmp_Bullet.transform.eulerAngles += CaculateSpreadOffset();
            var tmp_BulletScript = tmp_Bullet.AddComponent<Bullet>();
            tmp_BulletScript.ImpactPrefab = BulletImpactPrefab;
            tmp_BulletScript.ImpactAudioData = ImpactAudioData;
            tmp_BulletScript.BulletSpeed = 100;
        }


        /*private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                DoAttack();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }
            if (Input.GetMouseButtonDown(1))
            {
                isAiming = true;
                Aim();
            }            
            if (Input.GetMouseButtonUp(1))
            {
                isAiming = false;
                Aim();
            }
        }*/
    }
}
