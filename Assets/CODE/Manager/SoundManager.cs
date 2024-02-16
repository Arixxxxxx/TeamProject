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
    [SerializeField] List<AudioClip> bossAudio;
    [SerializeField] List<AudioClip> bossSkillSFX;
    [SerializeField] List<AudioClip> InfoNarration;

    AudioSource fireBaseAuido;


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

        soundPrefabs_Init(40);

        if (transform.childCount == 2)
        {
            fireBaseAuido = transform.Find("FireBaseSound").GetComponent<AudioSource>();
        }


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

    public void F_Bgm_Player(int BgmIndexNum, float FadeSpeed, float maxVolume)
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

        StartCoroutine(BgmChanger(isHaveClip, BgmIndexNum, FadeSpeed, maxVolume));

    }

    IEnumerator BgmChanger(bool value, int BgmIndexNum, float FadeSpeed, float maxVolume)
    {
        if (value)
        {
            while (audios.volume > 0) // 볼륨페이드
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

        while (audios.volume < maxVolume)
        {
            audios.volume += Time.deltaTime * 0.25f;
            yield return null;
        }
        
        audios.volume = maxVolume;

    }

    /// <summary>
    /// Play Sfx
    /// </summary>
    /// <param name="value">Sfx Index Num</param>
    /// <returns></returns>
    public void F_Get_SoundPreFabs_PlaySFX(int value, float volume)
    {
        if (audioClipQueue.Count == 0)
        {
            soundPrefabs_Init(1);
        }

        GameObject obj = audioClipQueue.Dequeue();
        obj.gameObject.SetActive(true);
        obj.GetComponent<SoundPreFabs>().F_SetClipAndPlay(SfxList[value], volume);

    }

    public SoundPreFabs F_Get_ControllSoundPreFabs_ETC_PlaySFX(int value, float volume)
    {
        if (audioClipQueue.Count == 0)
        {
            soundPrefabs_Init(1);
        }

        GameObject obj = audioClipQueue.Dequeue();
        obj.gameObject.SetActive(true);
        SoundPreFabs sc = obj.GetComponent<SoundPreFabs>();
        sc.F_SetClipAndPlay(EtcSoundList[value], volume);

        return sc;

    }

    public SoundPreFabs F_Get_ControllSoundPreFabs_BossSFX(int value, float volume)
    {
        if (audioClipQueue.Count <= 1)
        {
            soundPrefabs_Init(1);
        }

        GameObject obj = audioClipQueue.Dequeue();
        obj.gameObject.SetActive(true);
        SoundPreFabs sc = obj.GetComponent<SoundPreFabs>();
        sc.F_SetClipAndPlay(bossAudio[value], volume);
        sc.itMeNarration = true;

        return sc;

    }

    public SoundPreFabs F_Get_ControllSoundPreFabs_BossSkillSFX(int value)
    {
        if (audioClipQueue.Count == 0)
        {
            soundPrefabs_Init(1);
        }

        GameObject obj = audioClipQueue.Dequeue();
        obj.gameObject.SetActive(true);
        SoundPreFabs sc = obj.GetComponent<SoundPreFabs>();
        sc.F_SetClipAndPlay(bossSkillSFX[value]);

        return sc;

    }

    public SoundPreFabs F_Get_ControllSoundPreFabs_InfoNarrtionSFX(int value, float volume)
    {
        if (audioClipQueue.Count == 0)
        {
            soundPrefabs_Init(1);
        }

        GameObject obj = audioClipQueue.Dequeue();
        obj.gameObject.SetActive(true);
        SoundPreFabs sc = obj.GetComponent<SoundPreFabs>();
        sc.itMeNarration = true;
        sc.F_SetClipAndPlay(InfoNarration[value], volume);

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
        while (audios.volume > 0) // 볼륨페이드
        {
            audios.volume -= Time.deltaTime * 0.2f;
            yield return null;
        }
    }

    public void F_SetLoop(bool value)
    {
        audios.loop = value;
    }

    /// <summary>
    /// BGM 볼륨 컨트롤러
    /// </summary>
    /// <param name="value">볼륨값</param>
    /// <param name="speed">페이드 스피드</param>
    public void F_SetBGMVolume(float value, float speed)
    {
        StartCoroutine(VolueController(value, speed));
    }

    IEnumerator VolueController(float value, float speed)
    {
        float curVolume = audios.volume;

        if (curVolume < value)
        {
            while (audios.volume < 1)
            {
                audios.volume += Time.deltaTime * speed;
                yield return null;
            }


        }
        else if (curVolume > value)
        {
            while (audios.volume > value)
            {
                audios.volume -= Time.deltaTime * speed;
                yield return null;
            }
        }

        audios.volume = value;
    }

    public void F_fireBaseSoundActive(bool value)
    {
        if (value == true && fireBaseAuido.isPlaying == false)
        {
            fireBaseAuido.Play();
        }
        else if (value == false && fireBaseAuido.isPlaying == true)
        {
            fireBaseAuido.Stop();
        }
    }
}