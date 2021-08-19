using UnityEngine;

namespace Scripts.Weapon
{
   public abstract class Firearms : MonoBehaviour , IWeapon
    {
        public Transform MuzzlePoint;
        public Transform CasingPoint;

        public ParticleSystem MuzzleParticle;
        public ParticleSystem CasingParticle;
        public float FireRate;
        public int AmmoInMag = 30;
        public int MaxAmmoCarried = 120;

        protected Animator GunAnimator;
        private float lastFireTime;
        protected int CurrentAmmo;
        protected int CurrentMaxAmmo;


        protected virtual void Start()
        {
            CurrentAmmo = AmmoInMag;
            CurrentMaxAmmo = MaxAmmoCarried;       
        }
        public void DoAttack()
        {
            if (CurrentAmmo <= 0) return;
            CurrentAmmo -= 1;
            Shooting();
            lastFireTime = Time.time;
        }
        protected abstract void Shooting();
        protected abstract void Reload();

        private bool isAllowShooting() 
        {
            return Time.time - lastFireTime > 1 / FireRate;
        }
    } 
}
