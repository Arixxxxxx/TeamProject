using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMixer_Controller : MonoBehaviour
{
    public static AudioMixer_Controller inst;

    [SerializeField] AudioMixer minxerMain;


    private void Awake()
    {
        if(inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void F_SetMasterVolume(float volume)
    {
        minxerMain.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void F_SetMusicVolume(float volume)
    {
        minxerMain.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }

    public void F_SetSFXVolume(float volume)
    {
        minxerMain.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
}
