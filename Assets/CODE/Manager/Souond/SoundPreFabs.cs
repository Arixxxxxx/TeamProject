using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPreFabs : MonoBehaviour
{
    

    AudioSource audios;
    public bool soundEnd;
    public bool itMeNarration;
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
        soundEnd = true;
        audios.clip = Clips;
        StartCoroutine(PlaySound());
    }

    IEnumerator PlaySound()
    {
        if (itMeNarration) // 나레이션 카운터 확인용
        {
            GameUIManager.Inst.F_ItMeNarration(true);
        }

        audios.Play();
        
        yield return null;
        
        while (audios.isPlaying) 
        {
            yield return null;
        }

        soundEnd = false;
        audios.clip = null;

        if (itMeNarration)
        {
            itMeNarration = false;
            GameUIManager.Inst.F_ItMeNarration(false);
        }

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


    public void F_EndSound(float Speed)
    {
        if(audios.isPlaying) 
        {
            StartCoroutine(EndSound(Speed));
        }
    }

    IEnumerator EndSound(float Speed)
    {
        yield return null;

        while (audios.volume > 0)
        {
            audios.volume -= Time.deltaTime * Speed;
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
