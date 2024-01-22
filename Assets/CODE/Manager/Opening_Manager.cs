using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Opening_Manager : MonoBehaviour
{
    public static Opening_Manager inst;

    [Header("# Main Cheak Setting")]
    [Space]
    [SerializeField] bool openingSkip;
    [SerializeField] bool fadeOffSkip;
    [SerializeField] Animator Action2_0;
    [SerializeField] Animator Action2_1;
    

    [Header("# Insert Obj for Opening")]
    [Space]
    [SerializeField] GameObject cameraTarget;
    [SerializeField] Transform camtransform; // 카메라 시작위치
    [SerializeField] GameObject mainUiCanvas;
    [SerializeField] Animator photoFrameAnim;
    [SerializeField] ParticleSystem[] backgroundPs;
    Image cutton;
    [Header("# Intro Story Telling (Action0)")]
    [Space]
    [SerializeField] Text_Tyiping maintext;
    [SerializeField] Animator mask;
    [SerializeField] Image maskIMG;
    [SerializeField][Multiline] string[] textBox;
    [SerializeField] Sprite[] introSprite;
    [Header("# Action Time Value Setting")]
    [Space]
   
    [SerializeField] float CuttonFadeOffTime;
    [SerializeField] float Action1_0_DelayTime;
    [SerializeField] float Action2_DelayTime;
    Animator bgAnim;
    GameManager gm;
    CameraManager cm;
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

        mainUiCanvas.gameObject.SetActive(false);

    }
    void Start()
    {
        gm = GameManager.Inst;
        cm = CameraManager.inst;

        cutton = transform.Find("Canvas/Cutton").GetComponent<Image>();
        bgAnim = cutton.GetComponent<Animator>();
        GameManager.Inst.MoveStop = true;




        StartOpening();

    }

    bool Action0, Action1, Action2;
    private void StartOpening()
    {
        // 인트로 스토리 설명
        Action0 = true;
        StartCoroutine(IntroStoryAction0());


        //Action0 화면 페이드오프
        if (!fadeOffSkip)
        {
            StartCoroutine(CuttonFade());
        }
        
    }
    public bool nextOk;
    IEnumerator IntroStoryAction0()
    {
        photoFrameAnim.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f); // 그림설명 애니메이션 작동될때까지 기다리기
        mask.SetTrigger("FadeOn"); // 박스 스프라이트 연출
        backgroundPs[0].gameObject.SetActive(true);
        backgroundPs[1].gameObject.SetActive(true);
        nextOk = true;
        maintext.F_Set_TalkBox_Main_Text(textBox[0]); // 첫번째 오프닝스토리 문구 넣어주기
        yield return null;

        while(nextOk == true) //끝날때까지 기다리기 
        {
            yield return null; 
        }
        mask.SetTrigger("FadeOff"); // 첫번째 그림 페이드 아웃
        yield return new WaitForSeconds(0.5f); 
        maintext.F_TextEmpty(); // 대화창 한번지우고

        yield return new WaitForSeconds(2f); // 잠깐쉬고
        
        maskIMG.sprite = introSprite[0]; // 두번쨰 사진으로 변경
        mask.SetTrigger("FadeOn2"); // 애니메이션 재생

        nextOk = true; // 메세지 출력 끝났다면
        maintext.F_Set_TalkBox_Main_Text(textBox[1]); // 두번쨰 오프닝스토리 문구 넣어주기
      
        yield return null;

        while (nextOk == true) // 메시지 출력동안 대기
        {
            yield return null;
        }
        mask.SetTrigger("FadeOff2"); // 이미지 페이드아웃
        photoFrameAnim.SetTrigger("FadeOut");
        yield return new WaitForSeconds(0.5f); // 잠깐 쉬엇다가
        maintext.F_HideText(); // 텍스트 페이드아웃
        
      
        Action0 = false; // 다음 연출 시작
    }

    IEnumerator CuttonFade()
    {
        while(Action0 == true)
        {
            yield return null;
        }

        cm.F_OP_CamTargetSetting(cameraTarget.transform, 27, false, camtransform); // 카메라 위치 및 앵글값 초기화

        yield return new WaitForSeconds(CuttonFadeOffTime);
        bgAnim.SetTrigger("Off"); // 검은화면 제거
        for (int i = 0; i < backgroundPs.Length; i++)
        {
            backgroundPs[i].gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(Action1_0_DelayTime);
        bgAnim.transform.parent.gameObject.SetActive(false);  //  검은화면 Enable false

        // Action 1 시작 (추후에 함수 실행 위치 이동)
        cm.F_OP_CamTargetSetting(cameraTarget.transform, 9.5f, true, camtransform); // fade in + Camera 이동 연출

       
    }
    

    // 액션 2시작 조작키 설명 => 호출은 카메라매니저에서 함

    public void F_Action2Start()
    {
        StartCoroutine(Action2_Start());
    }

    IEnumerator Action2_Start()
    {
        GameManager.Inst.MoveStop = false; // 캐릭터 움직이게 해줌
        mainUiCanvas.gameObject.SetActive(true); // 게임 UI 켜줌

        yield return new WaitForSeconds(Action2_DelayTime);

        Action2_0.gameObject.SetActive(true); // WASD 조작부 설명 UI 켜줌
        Action2_0.SetTrigger("In"); // 페이드인 출력
        yield return new WaitForSeconds(3f);

        Action2_0.SetTrigger("Out"); // 페이드 아웃
        

        yield return new WaitForSeconds(2f);

        Action2_0.gameObject.SetActive(false); 
        Action2_1.gameObject.SetActive(true);// 쉬프트, 스페이스바 설명 UI 켜줌
        Action2_1.SetTrigger("In"); // 페이드인 출력
        yield return new WaitForSeconds(3f);

        Action2_1.SetTrigger("Out"); // 아웃

        yield return new WaitForSeconds(2f);
        Action2_1.gameObject.SetActive(false); // 꺼줌



    }

}
