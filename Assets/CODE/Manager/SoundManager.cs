using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour
{
    public static SoundManager inst;

    [SerializeField] GameObject soundPrefab;


    AudioSource audios;
    [SerializeField] List<AudioClip> BgmList;
    [SerializeField] List<AudioClip> SfxList;
    [SerializeField] List<AudioClip> EtcSoundList;




    Queue<GameObject> audioClipQueue = new Queue<GameObject>();
    Transform dynamicTrs;

    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }

        dynamicTrs = transform.Find("Dynamic").GetComponent<Transform>();
        audios = GetComponent<AudioSource>();

        soundPrefabs_Init(10);



    }
    void Start()
    {

    }

    private void soundPrefabs_Init(int value)
    {
        for (int i = 0; i < value; i++)
        {
            GameObject obj = Instantiate(soundPrefab, dynamicTrs);
            obj.transform.position = Vector3.zero;
            obj.gameObject.SetActive(false);
            audioClipQueue.Enqueue(obj);
        }
    }

    public void F_Bgm_Player(int BgmIndexNum, float FadeSpeed)
    {
        bool isHaveClip;

        if (audios.clip == null)
        {
            isHaveClip = false;
        }
        else
        {
            isHaveClip = true;
        }

        StartCoroutine(BgmChanger(isHaveClip, BgmIndexNum, FadeSpeed));

    }

    IEnumerator BgmChanger(bool value, int BgmIndexNum, float FadeSpeed)
    {
        if (value)
        {
            while (audios.volume > 0) // ∫º∑˝∆‰¿ÃµÂ
            {
                audios.volume -= Time.deltaTime * FadeSpeed;
                yield return null;
            }
        }
        else
        {
            audios.volume = 0;
        }

        audios.clip = BgmList[BgmIndexNum];

        yield return null;

        audios.Play();

        while (audios.volume < 1)
        {
            audios.volume += Time.deltaTime * 0.25f;
            yield return null;
        }
    }

    /// <summary>
    /// Play Sfx
    /// </summary>
    /// <param name="value">Sfx Index Num</param>
    /// <returns></returns>
    public void F_Get_SoundPreFabs_PlaySFX(int value)
    {
        if (audioClipQueue.Count == 0)
        {
            soundPrefabs_Init(1);
        }

        GameObject obj = audioClipQueue.Dequeue();
        obj.gameObject.SetActive(true);
        obj.GetComponent<SoundPreFabs>().F_SetClipAndPlay(SfxList[value]);

    }

    public SoundPreFabs F_Get_ControllSoundPreFabs_PlaySFX(int value)
    {
        if (audioClipQueue.Count == 0)
        {
            soundPrefabs_Init(1);
        }

        GameObject obj = audioClipQueue.Dequeue();
        obj.gameObject.SetActive(true);
        SoundPreFabs sc = obj.GetComponent<SoundPreFabs>();
        sc.F_SetClipAndPlay(EtcSoundList[value]);

        return sc;

    }

    /// <summary>
    /// Return SoundPreFabs
    /// </summary>
    /// <param name="obj">GameObject</param>
    public void F_ReturnSoundPreFabs(GameObject obj)
    {
        obj.SetActive(false);
        audioClipQueue.Enqueue(obj);
    }


    public void F_BgmEnd()
    {
        StartCoroutine(EndBGM());
    }

    IEnumerator EndBGM()
    {
        while (audios.volume > 0) // ∫º∑˝∆‰¿ÃµÂ
        {
            audios.volume -= Time.deltaTime * 0.2f;
            yield return null;
        }
    }

    public void F_SetLoop(bool value)
    {
        audios.loop = value;
    }
}