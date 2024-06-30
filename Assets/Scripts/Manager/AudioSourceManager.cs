using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioSourceManager : MonoBehaviour
{
    public static AudioSourceManager instance;
    [HideInInspector]
    public AudioSource audioSource;
    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }
    public void PlaySound(AudioClip clip)
    {
        audioSource.playOnAwake = false;
        audioSource.PlayOneShot(clip);
        audioSource.volume = GlobalValue.Volumn;
    }
    public void PlayMusic(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();

    }
    public void StopPlayMusic()
    {
        audioSource.Stop();
    }
}
