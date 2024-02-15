using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore;
using UnityEngine.UI;

public class EndingDirector : MonoBehaviour
{
    public static EndingDirector inst;

    [Header("Insert Intro Material")]
    [Space]
    [SerializeField] Sprite endingStorySprite;
    [SerializeField] Sprite[] creditIMG;
    [SerializeField]
    [Multiline] string[] storyData;

    Text_Tyiping text_sc;
    Image FastInfo;
    GameObject UI_Canvse;
    Image ImageFrame;
    Animator frameAnim;

    [SerializeField] GameObject credit;
    Animator creditAnim;
    TMP_Text crreditText;
    
    [SerializeField] GameObject creditWorld;
    Animator Cutton;
    Image creditIMGFrame;

    WaitForSeconds[] sec05Wait= new WaitForSeconds[8];
    
    float secondTime;
    bool nextOk;
    public bool NextOk {  get { return nextOk; } set { nextOk = value; } }

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

        for(int i = 0; i < sec05Wait.Length; i++)
        {
            secondTime += 0.5f;
            sec05Wait[i] = new WaitForSeconds(secondTime);
        }

        UI_Canvse = GameObject.Find("UI_Canvas").gameObject;
        
        Cutton = UI_Canvse.transform.Find("Cutton(B)").GetComponent<Animator>();
        Cutton.gameObject.SetActive(false);

        ImageFrame = UI_Canvse.transform.Find("IMG_Frame").GetComponent<Image>();
        frameAnim = ImageFrame.GetComponent<Animator>();
        text_sc = UI_Canvse.transform.Find("Text").GetComponent<Text_Tyiping>();

        creditAnim = credit.GetComponent<Animator>();
        crreditText = credit.transform.Find("Text").GetComponent<TMP_Text>();
        creditIMGFrame = credit.transform.Find("Image").GetComponent<Image>();
        credit.gameObject.SetActive(false);
        creditWorld.gameObject.SetActive(false);

        FastInfo = UI_Canvse.transform.Find("Fast_Info").GetComponent<Image>();
    }
    void Start()
    {

        StartCoroutine(EndingActionStart());
    }

    bool textUpStrat;
    IEnumerator EndingActionStart()
    {
        frameAnim.SetTrigger("On");
        SoundManager.inst.F_Bgm_Player(0,0.5f, 1);
        yield return null;
        yield return sec05Wait[3]; 
        text_sc.F_Set_TalkBox_Main_Text(storyData[0]);
        SoundPreFabs sc = SoundManager.inst.F_Get_ControllSoundPreFabs_ETC_PlaySFX(0, 1);
        sc.F_SetVolume(0.7f);
        sc.F_SetSoundLoop(true);

        FastInfo.gameObject.SetActive(true);   
        NextOk = true;

        yield return null;

        while (NextOk == true) //끝날때까지 기다리기 
        {
            yield return null;
        }
        sc.F_QuickEndSound();

        yield return sec05Wait[1]; // 글 다나왓으면 2초 대기
        text_sc.F_HideText(3f); // 텍스트 지우면서 하이드

        frameAnim.SetTrigger("Off"); // 첫번째 그림 페이드 아웃
        yield return sec05Wait[1];

        ImageFrame.sprite = endingStorySprite; // 사진변경
        
        frameAnim.SetTrigger("On");


        yield return sec05Wait[2]; 
        text_sc.F_Set_TalkBox_Main_Text(storyData[1]);
        sc = SoundManager.inst.F_Get_ControllSoundPreFabs_ETC_PlaySFX(0, 1); // 볼펜소리
        sc.F_SetVolume(0.7f);
        sc.F_SetSoundLoop(true);

        NextOk = true;

        yield return null;

        while (NextOk == true) //끝날때까지 기다리기 
        {
            yield return null;
        }
        sc.F_QuickEndSound();
        FastInfo.gameObject.SetActive(false);

        yield return sec05Wait[3]; // 글 다나왓으면 2초 대기

        text_sc.F_HideText(3f); // 텍스트 지우면서 하이드
        frameAnim.SetTrigger("Off"); // 첫번째 그림 페이드

        yield return sec05Wait[3];
        SoundManager.inst.F_Bgm_Player(1, 0.3f, 1);
        yield return sec05Wait[6];
        
        ImageFrame.gameObject.SetActive(false);
        text_sc.gameObject.SetActive(false);

        Cutton.gameObject.SetActive(true); // 두번째 액션 켜줌
        credit.SetActive(true);
        creditWorld.SetActive(true);


        
        yield return sec05Wait[5];

        textUpStrat = true; // 텍스트 위로 출력 시작

        yield return sec05Wait[2];
        

        for (int i = 0; i < creditIMG.Length; i++)
        {

            creditIMGFrame.sprite = creditIMG[i]; 
            creditAnim.SetTrigger("On");
            yield return sec05Wait[7];
            yield return sec05Wait[7];
            creditAnim.SetTrigger("Off");
            yield return sec05Wait[7];
            yield return sec05Wait[1];

            if (i == 3)
            {
                yield return sec05Wait[6];
                yield return sec05Wait[6];
                yield return sec05Wait[6];
                endScene = true;
            }
        }
       
        while(endScene == false)
        {
            yield return null;
        }

        Cutton.SetTrigger("On");
        yield return sec05Wait[7];
        SoundManager.inst.F_BgmEnd();
        yield return sec05Wait[4];
        yield return sec05Wait[7];
        SceneManager.LoadScene(0);
    }

    //IEnumerator Credit()
    //{

    //}
    [Header("Text Up Speed Vlaue")]
    [Space]
    [SerializeField] float textUpSpeed;

    [SerializeField] bool endScene;
    void Update()
    {
        if (textUpStrat)
        {
            crreditText.transform.position += transform.up * Time.deltaTime * textUpSpeed;
        }

        if (FastInfo.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            text_sc.F_SetAddSpeed(true);
        }
        else if (FastInfo.gameObject.activeSelf && Input.GetKeyUp(KeyCode.Space))
        {
            text_sc.F_SetAddSpeed(false);
        }

      if(Input.GetKeyDown(KeyCode.C))
        {
            endScene = true;
        }  
    }

}
