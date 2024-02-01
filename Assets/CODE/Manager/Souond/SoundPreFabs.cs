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
}
