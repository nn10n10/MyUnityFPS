using UnityEngine;

namespace Scripts.Weapon
{
   public abstract class Firearms : MonoBehaviour , IWeapon
    {
        public Transform MuzzlePoint;
        public Transform CasingPoint;

        public ParticleSystem MuzzleParticle;
        public ParticleSystem CasingParticle;

        public int AmmoInMag = 30;
        public int MaxAmmoCarried = 120;

        protected Animator GunAnimator;
        public void DoAttack()
        {

        }
    } 
}
