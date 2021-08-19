using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="FPS/FootStep Audio Data")]
public class FootStepAudioData : ScriptableObject
{
    public List<FootStepAduio> FootStepAduios = new List<FootStepAduio>();
}
[System.Serializable]
public class FootStepAduio
{
    public string Tag;
    public List<AudioClip> AudioClips = new List<AudioClip>();
    public float Delay;
    public float SprintingDelay;
    public float CrouchingDelay;
}