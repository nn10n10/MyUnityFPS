using UnityEngine;

namespace Scripts.Weapon
{
    public class AssualtRifle : Firearms
    {
        protected override void Reload()
        {
           
        }

        protected override void Shooting()
        {
            Debug.Log("shooting");
        }
        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                DoAttack();
            }
        }
    }
}
