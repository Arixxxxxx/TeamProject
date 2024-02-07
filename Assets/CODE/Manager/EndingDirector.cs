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
        SoundManager.inst.F_Bgm_Player(0,0.5f);
        yield return null;
        yield return sec05Wait[3]; 
        text_sc.F_Set_TalkBox_Main_Text(storyData[0]);
        FastInfo.gameObject.SetActive(true);   
        NextOk = true;

        yield return null;

        while (NextOk == true) //���������� ��ٸ��� 
        {
            yield return null;
        }

       
        yield return sec05Wait[1]; // �� �ٳ������� 2�� ���
        text_sc.F_HideText(3f); // �ؽ�Ʈ ����鼭 ���̵�

        frameAnim.SetTrigger("Off"); // ù��° �׸� ���̵� �ƿ�
        yield return sec05Wait[1];

        ImageFrame.sprite = endingStorySprite; // ��������
        
        frameAnim.SetTrigger("On");


        yield return sec05Wait[2]; 
        text_sc.F_Set_TalkBox_Main_Text(storyData[1]);

        NextOk = true;

        yield return null;

        while (NextOk == true) //���������� ��ٸ��� 
        {
            yield return null;
        }

        FastInfo.gameObject.SetActive(false);

        yield return sec05Wait[3]; // �� �ٳ������� 2�� ���

        text_sc.F_HideText(3f); // �ؽ�Ʈ ����鼭 ���̵�
        frameAnim.SetTrigger("Off"); // ù��° �׸� ���̵�

        yield return sec05Wait[3];
        SoundManager.inst.F_Bgm_Player(1, 0.7f);
        yield return sec05Wait[3];
        ImageFrame.gameObject.SetActive(false);
        text_sc.gameObject.SetActive(false);

        Cutton.gameObject.SetActive(true); // �ι�° �׼� ����
        credit.SetActive(true);
        creditWorld.SetActive(true);


        
        yield return sec05Wait[5];

        textUpStrat = true; // �ؽ�Ʈ ���� ��� ����

        yield return sec05Wait[2];
        

        for (int i = 0; i < creditIMG.Length; i++)
        {
            int IMGCount = 0;

            creditIMGFrame.sprite = creditIMG[IMGCount]; // 0��°
            creditAnim.SetTrigger("On");
            yield return sec05Wait[7];
            yield return sec05Wait[1];
            creditAnim.SetTrigger("Off");
            yield return sec05Wait[7];
            yield return sec05Wait[1];
            IMGCount++;

            if(IMGCount == creditIMG.Length-1)
            {
                Debug.Log("����");
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
