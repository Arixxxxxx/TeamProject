using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore;
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
    [SerializeField] GameObject testBtn;
    

    [Header("# Insert Obj for Opening")]
    [Space]
    [SerializeField] Transform camtransform; // 카메라 시작위치
    [SerializeField] GameObject mainUiCanvas;
    [SerializeField] ParticleSystem[] backgroundPs;
    Image cutton;
    [Header("# Intro Story Telling (Action0)")]
    [Space]
    [SerializeField] Text_Tyiping maintext;
    [SerializeField] Animator photoFrameAnim;
    [SerializeField] Image frameIMG;
    [SerializeField][Multiline] string[] textBox;
    [SerializeField] Image fastInfo;
    [SerializeField] Sprite[] introSprite;
    // # Intro Story Telling (How To Play)")]
    [SerializeField] GameObject howToPlay_MainObj;
    [SerializeField] GameObject howToPlay_Action0;
    [SerializeField] GameObject howToPlay_Action1;


    [Header("# Action Time Value Setting")]
    [Space]
   
    [SerializeField] float CuttonFadeOffTime;
    [SerializeField] float Action1_0_DelayTime;
    [SerializeField] float Action2_DelayTime;
    Animator bgAnim;
    GameManager gm;
    CameraManager cm;
    Transform playerStartPos;
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
        cutton = transform.Find("Canvas/Cutton").GetComponent<Image>();
        cutton.gameObject.SetActive(true);

        playerStartPos = transform.Find("PlayerStartPos").GetComponent<Transform>();
       


         howToPlay_MainObj = transform.Find("Action2/Canvas/HowToPlay").gameObject;
        howToPlay_Action0 = howToPlay_MainObj.transform.Find("FirstPage").gameObject;
        howToPlay_Action1 = howToPlay_MainObj.transform.Find("SecPage").gameObject;

        //mainUiCanvas.gameObject.SetActive(false);

    }
    void Start()
    {
        
        testBtn.SetActive(false);
        gm = GameManager.Inst;
        cm = CameraManager.inst;
        GameManager.Inst.F_PlayerTransformMove(playerStartPos.position); // 플레이어 위치 초기화
        
        bgAnim = cutton.GetComponent<Animator>();
        GameManager.Inst.MoveStop = true;


        StartOpening();
        GameUIManager.Inst.F_GameUIActive(false);

        SoundManager.inst.F_Bgm_Player(0,0.25f);
    }

    private void Update()
    {
        howToPlayWindowsController();

        if(inputText == true && Input.GetKeyDown(KeyCode.Space))
        {
            maintext.F_SetAddSpeed(true);
        }

        if (inputText == true && Input.GetKeyUp(KeyCode.Space))
        {
            maintext.F_SetAddSpeed(false);
        }
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
    public bool inputText;
    IEnumerator IntroStoryAction0()
    {
        //photoFrameAnim.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f); // 그림설명 애니메이션 작동될때까지 기다리기
        photoFrameAnim.SetTrigger("FadeOn"); // 박스 스프라이트 연출
        nextOk = true;
        yield return new WaitForSeconds(1.5f);
        fastInfo.gameObject.SetActive(true); // Spacebar Fast 인포 켜주기
        //backgroundPs[0].gameObject.SetActive(true); // 파티클 취소(24/01.26)
        //backgroundPs[1].gameObject.SetActive(true);// 파티클 취소(24/01.26)
        maintext.F_Set_TalkBox_Main_Text(textBox[0]); // 첫번째 오프닝스토리 문구 넣어주기
        inputText = true;

        yield return null;

        while(nextOk == true) //끝날때까지 기다리기 
        {
            yield return null; 
        }
        yield return new WaitForSeconds(2f); // 글 다나왓으면 2초 대기
        photoFrameAnim.SetTrigger("FadeOff"); // 첫번째 그림 페이드 아웃
        yield return new WaitForSeconds(0.5f);
        //maintext.F_TextEmpty(); // 대화창 한번지우고
        maintext.F_HideText(5.5f);

        yield return new WaitForSeconds(0.7f); // 잠깐쉬고
        
        frameIMG.sprite = introSprite[0]; // 두번쨰 사진으로 변경
        photoFrameAnim.SetTrigger("FadeOn2"); // 애니메이션 재생

        nextOk = true; // 메세지 출력 끝났다면
        yield return new WaitForSeconds(1f);
        maintext.F_Set_TalkBox_Main_Text(textBox[1]); // 두번쨰 오프닝스토리 문구 넣어주기
      
        yield return null;

        while (nextOk == true) // 메시지 출력동안 대기
        {
            yield return null;
        }
        inputText = false;
        fastInfo.gameObject.SetActive(false); // Spacebar Fast 인포 켜주기
        yield return new WaitForSeconds(2f);// 글 다나왓으면 2초 대기
        photoFrameAnim.SetTrigger("FadeOff2"); // 이미지 페이드아웃
        
        yield return new WaitForSeconds(0.5f); // 잠깐 쉬엇다가
        maintext.F_HideText(1.2f); // 텍스트 페이드아웃

       
          Action0 = false; // 다음 연출 시작
        SoundManager.inst.F_Bgm_Player(1,0.25f);
    }

    IEnumerator CuttonFade()
    {
        while(Action0 == true)
        {
            yield return null;
        }

        cm.F_OP_CamTargetSetting(GameManager.Inst.F_GetPalyerTargetPoint(), 25, false, camtransform); // 카메라 위치 및 앵글값 초기화

        yield return new WaitForSeconds(CuttonFadeOffTime);
        bgAnim.SetTrigger("Off"); // 검은화면 제거

        //for (int i = 0; i < backgroundPs.Length; i++) // 배경파티클 미사용
        //{
        //    backgroundPs[i].gameObject.SetActive(false);
        //}

        yield return new WaitForSeconds(Action1_0_DelayTime);

        //bgAnim.SetTrigger("On");  // 페이드아웃 후 캐릭터 집중 연출
        //yield return new WaitForSeconds(1);
        //cm.F_DirectAction();
        //yield return new WaitForSeconds(0.5f);
        //bgAnim.SetTrigger("Off");
        //yield return new WaitForSeconds(2.1f);
        
        bgAnim.transform.parent.gameObject.SetActive(false);  //  검은화면 Enable false
        //F_Action2Start();
        // Action 1 시작 (추후에 함수 실행 위치 이동)

        cm.F_OP_CamTargetSetting(GameManager.Inst.F_GetPalyerTargetPoint(), 9.5f, true, camtransform); // fade in + Camera 이동 연출


    }
    

    // 액션 2시작 조작키 설명 => 호출은 카메라매니저에서 함

    public void F_Action2Start()
    {
        StartCoroutine(Action2_Start());
    }

    IEnumerator Action2_Start()
    {
        GameManager.Inst.MoveStop = false; // 캐릭터 움직이게 해줌
 
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

        yield return new WaitForSeconds(2f); //
        Action2_1.gameObject.SetActive(false); // 꺼줌

        // 게임설명
        howtoPlayEnd = true; // 설명체크 불리언 변수
        GameManager.Inst.F_TimeSclaeController(true);  // 시간 멈춤

        howToPlay_MainObj.SetActive(true); //  설명창 오픈
        if(howToPlay_Action0.activeSelf == false)
        {
            howToPlay_Action0.SetActive(true);
        }

        while(howtoPlayEnd == true) //끝날때까지 대기
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(1f); 

        GameUIManager.Inst.F_GameUIActive(true); // UI켜주고

        yield return new WaitForSeconds(1.5f);
        GameManager.Inst.MainGameStart = true; // 게임시작
    }

    bool howtoPlayEnd;
    bool howtoPlayWaittime;
    float count;
    private void howToPlayWindowsController()
    {
        if(howToPlay_MainObj.activeSelf == false) { return; }

        if(howToPlay_Action0.activeSelf == true && Input.GetKeyDown(KeyCode.Space) && howtoPlayWaittime == false)
        {
            howtoPlayWaittime = true;
            howToPlay_Action0.SetActive(false);
            howToPlay_Action1.SetActive(true);
        }

        if (howtoPlayWaittime) // 딜레이 걸어주는 bool 변수
        {
            count += Time.fixedUnscaledDeltaTime; // 언스케일델타타임

            if(count > 1)
            {
                count = 0;
                howtoPlayWaittime = false;
            }
        }

        if (howToPlay_Action1.activeSelf == true && Input.GetKeyDown(KeyCode.Space) && howtoPlayWaittime == false) // 바로 입력 안되게 딜레이 줌
        {
            howToPlay_Action1.SetActive(false);
            howToPlay_MainObj.SetActive(false);
            GameManager.Inst.F_TimeSclaeController(false);
            howtoPlayEnd = false;
        }

    }
}
