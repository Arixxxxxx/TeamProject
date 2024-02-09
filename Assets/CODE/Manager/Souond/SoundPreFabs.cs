using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPreFabs : MonoBehaviour
{
    AudioSource audios;
    private void Awake()
    {
        audios = GetComponent<AudioSource>();
    }
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void F_SetClipAndPlay(AudioClip Clips)
    {
        audios.clip = Clips;
        StartCoroutine(PlaySound());
    }

    IEnumerator PlaySound()
    {
        audios.Play();
        
        yield return null;
        
        while (audios.isPlaying) 
        {
            yield return null;
        }

        audios.clip = null;

        SoundManager.inst.F_ReturnSoundPreFabs(gameObject);
    }


    public void F_SetSoundLoop(bool Value)
    {
        audios.loop = Value;
    }
    public void F_QuickEndSound()
    {
        if (audios.isPlaying)
        {
            audios.Stop();
            audios.clip = null;
            audios.loop = false;
            audios.volume = 1;

            SoundManager.inst.F_ReturnSoundPreFabs(gameObject);

        }
    }


    public void F_EndSound()
    {
        if(audios.isPlaying) 
        {
            StartCoroutine(EndSound());
        }
    }

    IEnumerator EndSound()
    {
        yield return null;

        while (audios.volume > 0)
        {
            audios.volume -= Time.deltaTime;
            yield return null;
        }

        audios.clip = null;
        audios.loop = false;
        audios.volume = 1;

        SoundManager.inst.F_ReturnSoundPreFabs(gameObject);
    }

    public void F_SetVolume(float value)
    {
        audios.volume = value;
    }
}
