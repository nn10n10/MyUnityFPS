using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Weapon
{
    public class Bullet : MonoBehaviour
    {
        public float BulletSpeed;
        public GameObject ImpactPrefab;
        public ImpactAudioData ImpactAudioData;
        private Transform bulletTransform;
        private Vector3 prevPosition;

        private void Start()
        {
            bulletTransform = transform;
            prevPosition = bulletTransform.position;
        }

        private void Update()
        {
            prevPosition = bulletTransform.position;
            bulletTransform.Translate(0,0,BulletSpeed*Time.deltaTime);
            if (!Physics.Raycast(prevPosition,
                (bulletTransform.position-prevPosition).
                normalized,out RaycastHit tmp_Hit,(bulletTransform.position-prevPosition).magnitude)) return;
            var tmp_BullectEffect=
               Instantiate(ImpactPrefab, 
                   tmp_Hit.point, Quaternion.LookRotation(tmp_Hit.normal, Vector3.up));
          
            Destroy(tmp_BullectEffect,3);

            var tmp_TagsWithAudio = ImpactAudioData.ImpactTagsWithAudios.Find((tmp_AudioData)=>
            {
                return tmp_AudioData.Tag.Equals(tmp_Hit.collider.tag);
            });
            if (tmp_TagsWithAudio == null) return;
            int tmp_Lenth = tmp_TagsWithAudio.ImpactAudioClips.Count;
            AudioClip tmp_AudioClip = tmp_TagsWithAudio.ImpactAudioClips[Random.Range(0, tmp_Lenth)];
            AudioSource.PlayClipAtPoint(tmp_AudioClip,tmp_Hit.point);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawCube(transform.position,new Vector3(0.1f,0.1f,.25f));
        }

#endif
        
        
        
    }

}