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
    [SerializeField] Animator[] ActionInfo;

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
    [SerializeField] bool[] tutorialAction = new bool[3];
    [SerializeField] Animator tutorialBarAnim;
    //WASD 튜토리얼 초기화
    float curMoveTime = 0;
    float moveTotalTime = 5;
    Slider tutorialBar;
    [SerializeField] ParticleSystem[] suseccPs;

    //
    GameObject howToPlay_MainObj;
    GameObject howToPlay_Action0;
    GameObject howToPlay_Action1;


    [Header("# Action Time Value Setting")]
    [Space]

    [SerializeField] float CuttonFadeOffTime;
    [SerializeField] float Action1_0_DelayTime;
    [SerializeField] float Action2_DelayTime;
    Animator bgAnim;
    GameManager gm;
    CameraManager cm;
    Transform playerStartPos;
    [SerializeField] TMP_Text tutorialCountText;
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
        cutton = transform.Find("Canvas/Cutton").GetComponent<Image>();
        cutton.gameObject.SetActive(true);

        playerStartPos = transform.Find("PlayerStartPos").GetComponent<Transform>();



        howToPlay_MainObj = transform.Find("Action2/Canvas/HowToPlay").gameObject;
        howToPlay_Action0 = howToPlay_MainObj.transform.Find("FirstPage").gameObject;
        howToPlay_Action1 = howToPlay_MainObj.transform.Find("SecPage").gameObject;
        tutorialBar = tutorialBarAnim.transform.Find("Slider").GetComponent<Slider>();



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

        SoundManager.inst.F_Bgm_Player(0, 0.25f, 1);
        SoundManager.inst.F_SetLoop(true);
    }

    private void Update()
    {
        TutorialAction_0();
        TutorialAction_1();
        TutorialAction_2();

        howToPlayWindowsController();

        if (inputText == true && Input.GetKeyDown(KeyCode.Space))
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

        //
        SoundPreFabs sc = SoundManager.inst.F_Get_ControllSoundPreFabs_ETC_PlaySFX(0, 1);
        sc.F_SetVolume(0.7f);
        sc.F_SetSoundLoop(true);

        inputText = true;

        yield return null;

        while (nextOk == true) //끝날때까지 기다리기 
        {
            yield return null;
        }

        sc.F_QuickEndSound();

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

        //
        SoundPreFabs sc1 = SoundManager.inst.F_Get_ControllSoundPreFabs_ETC_PlaySFX(0, 1);
        sc1.F_SetVolume(0.7f);
        sc1.F_SetSoundLoop(true);
        yield return null;

        while (nextOk == true) // 메시지 출력동안 대기
        {
            yield return null;
        }
        sc1.F_QuickEndSound(); // 사운드 종료
        inputText = false;
        fastInfo.gameObject.SetActive(false); // Spacebar Fast 인포 켜주기
        yield return new WaitForSeconds(2f);// 글 다나왓으면 2초 대기
        photoFrameAnim.SetTrigger("FadeOff2"); // 이미지 페이드아웃

        yield return new WaitForSeconds(0.5f); // 잠깐 쉬엇다가
        maintext.F_HideText(1.2f); // 텍스트 페이드아웃


        Action0 = false; // 다음 연출 시작


    }

    IEnumerator CuttonFade()
    {
        while (Action0 == true)
        {
            yield return null;
        }

        cm.F_OP_CamTargetSetting(GameManager.Inst.F_GetPalyerTargetPoint(), 25, false, camtransform); // 카메라 위치 및 앵글값 초기화

        yield return new WaitForSeconds(CuttonFadeOffTime);

        bgAnim.SetTrigger("Off"); // 검은화면 제거

        yield return new WaitForSeconds(Action1_0_DelayTime);

        bgAnim.transform.parent.gameObject.SetActive(false);  //  검은화면 Enable false
        cm.F_OP_CamTargetSetting(GameManager.Inst.F_GetPalyerTargetPoint(), 11f, true, camtransform); // fade in + Camera 이동 연출


    }


    // 액션 2시작 조작키 설명 => 호출은 카메라매니저에서 함

    public void F_Action2Start()
    {
        StartCoroutine(Action2_Start());
      
        
    }
    SoundPreFabs sc;
    IEnumerator Action2_Start()
    {
        

        if (GameManager.Inst.TotalkillCount == 0) // 최초 실행이라면
        {
            SoundManager.inst.F_SetBGMVolume(0.4f, 0.5f); // 배경음악 볼륨 낮추기

            yield return new WaitForSeconds(2f);
            sc = SoundManager.inst.F_Get_ControllSoundPreFabs_InfoNarrtionSFX(0,1);
            ActionInfo[0].gameObject.SetActive(true);
            GameManager.Inst.MoveStop = false;
            ActionInfo[0].SetTrigger("In");
            yield return new WaitForSeconds(5f);

            ActionInfo[0].SetTrigger("Out"); // 페이드 아웃
            yield return new WaitForSeconds(2f);
            ActionInfo[0].gameObject.SetActive(false);
            yield return null;

            while (sc.soundEnd)
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);

            sc = SoundManager.inst.F_Get_ControllSoundPreFabs_InfoNarrtionSFX(1, 1);
            yield return new WaitForSeconds(0.5f);

            ActionInfo[1].gameObject.SetActive(true); // WASD 조작부 설명 UI 켜줌
            GameManager.Inst.MoveStop = false; // 캐릭터 움직이게 해줌
            ActionInfo[1].SetTrigger("In"); // 페이드인 출력

            // Tutorial  Phase  1 => WASD

            tutorialBarAnim.gameObject.SetActive(true);
            tutorialBarAnim.SetTrigger("Open");
            tutorialAction[0] = true;

            yield return null;

            while (tutorialAction[0])
            {
                yield return null;
            }

            yield return new WaitForSeconds(1f);

            ActionInfo[1].SetTrigger("Out"); // 페이드 아웃
            tutorialBarAnim.SetTrigger("Close");
            yield return new WaitForSeconds(2f);
            ActionInfo[1].gameObject.SetActive(false);
            tutorialBarAnim.gameObject.SetActive(false);

            //

            yield return new WaitForSeconds(2f);
            sc = SoundManager.inst.F_Get_ControllSoundPreFabs_InfoNarrtionSFX(2, 1);

            yield return null;

            while (sc.soundEnd)
            {
                yield return null;
            }

            sc = SoundManager.inst.F_Get_ControllSoundPreFabs_InfoNarrtionSFX(3, 1);
            yield return new WaitForSeconds(0.5f);

            ActionInfo[2].gameObject.SetActive(true);// 쉬프트, 스페이스바 설명 UI 켜줌
            ActionInfo[2].SetTrigger("In"); // 페이드인 출력


            // Tutorial  Phase  2 => Sprint

            tutorialBar.value = 0;
            curMoveTime = 0;
            tutorialBarAnim.gameObject.SetActive(true);
            tutorialBarAnim.SetTrigger("Open");
            tutorialAction[1] = true;

            yield return null;

            while (tutorialAction[1])
            {
                yield return null;
            }

            yield return new WaitForSeconds(1f);
            ActionInfo[2].SetTrigger("Out"); // 아웃
            tutorialBarAnim.SetTrigger("Close");
            yield return new WaitForSeconds(2f);
            ActionInfo[2].gameObject.SetActive(false); // 꺼줌
            tutorialBarAnim.gameObject.SetActive(false);

            //
            tutorialBar.value = 0;
            curMoveTime = 0;


            yield return new WaitForSeconds(1f);
            sc = SoundManager.inst.F_Get_ControllSoundPreFabs_InfoNarrtionSFX(2, 1);
            yield return null;

            while (sc.soundEnd)
            {
                yield return null;
            }

            // 순간이동 튜토리얼
            yield return new WaitForSeconds(0.5f);
            sc = SoundManager.inst.F_Get_ControllSoundPreFabs_InfoNarrtionSFX(4,1);
            yield return new WaitForSeconds(0.5f);

            ActionInfo[3].gameObject.SetActive(true);// 스페이스바 설명 UI 켜줌
            ActionInfo[3].SetTrigger("In"); // 페이드인 출력
                                            //여기 

            tutorialAction[2] = true;

            yield return null;

            while (tutorialAction[2])
            {
                yield return null;
            }
            //

            yield return new WaitForSeconds(1f);
            ActionInfo[3].SetTrigger("Out"); // 아웃
            yield return new WaitForSeconds(2f); //
            ActionInfo[3].gameObject.SetActive(false); // 꺼줌
            yield return new WaitForSeconds(1f);
            // 순간이동 튜토리얼 종료

            sc = SoundManager.inst.F_Get_ControllSoundPreFabs_InfoNarrtionSFX(2, 1);
            yield return null;

            while (sc.soundEnd)
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);
            sc = SoundManager.inst.F_Get_ControllSoundPreFabs_InfoNarrtionSFX(5, 1);
            yield return null;

            while (sc.soundEnd)
            {
                yield return null;
            }

            howtoPlayEnd = true; // 설명체크 불리언 변수
            GameManager.Inst.F_TimeSclaeController(true);  // 시간 멈춤

            howToPlay_MainObj.SetActive(true); //  설명창 오픈
            if (howToPlay_Action0.activeSelf == false)
            {
                howToPlay_Action0.SetActive(true);
            }

            while (howtoPlayEnd == true) //끝날때까지 대기
            {
                yield return null;
            }

            yield return new WaitForSeconds(1.5f);
            sc = SoundManager.inst.F_Get_ControllSoundPreFabs_InfoNarrtionSFX(6, 1);
            yield return null;

        }


        // 바로시작
        GameManager.Inst.MoveStop = false;
        yield return new WaitForSeconds(1f);
        SoundManager.inst.F_Bgm_Player(1, 0.5f, 0.7f);
        GameUIManager.Inst.F_GameUIActive(true); // UI켜주고

        if(sc != null)
        {
            while (sc.soundEnd)
            {
                yield return null;
            }
        }
        

        yield return new WaitForSeconds(1.5f);
        GameManager.Inst.MainGameStart = true; // 게임시작

    }

    bool howtoPlayEnd;
    bool howtoPlayWaittime;
    float count;
    private void howToPlayWindowsController()
    {
        if (howToPlay_MainObj.activeSelf == false) { return; }

        if (howToPlay_Action0.activeSelf == true && Input.GetKeyDown(KeyCode.Space) && howtoPlayWaittime == false)
        {
            howtoPlayWaittime = true;
            howToPlay_Action0.SetActive(false);
            howToPlay_Action1.SetActive(true);
        }

        if (howtoPlayWaittime) // 딜레이 걸어주는 bool 변수
        {
            count += Time.fixedUnscaledDeltaTime; // 언스케일델타타임

            if (count > 1)
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

    private void TutorialAction_0()
    {
        if (tutorialAction[0])
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                curMoveTime += Time.deltaTime;
            }
            else
            {
                curMoveTime -= Time.deltaTime * 0.5f;

                if (curMoveTime <= 0)
                {
                    curMoveTime = 0;
                }
            }

            if (tutorialBar.value < 1)
            {
                tutorialBar.value = curMoveTime / moveTotalTime;
            }
            else if (tutorialBar.value >= 1)
            {
                tutorialAction[0] = false;
                curMoveTime = 0;
                StartCoroutine(Sucsess(0));

            }
        }

    }

    private void TutorialAction_1()
    {
        if (tutorialAction[1])
        {
            if (GameManager.Inst.isRun)
            {
                curMoveTime += Time.deltaTime * 1.2f;
            }

            if (tutorialBar.value < 1)
            {
                tutorialBar.value = curMoveTime / moveTotalTime;
            }
            else if (tutorialBar.value >= 1)
            {
                tutorialAction[1] = false;
                curMoveTime = 0;
                StartCoroutine(Sucsess(0));
            }

        }


    }

    private void TutorialAction_2()
    {
        if (tutorialAction[2] == false) { return; }
        else
        {
            GameManager.Inst.TotorialCountStart = true;
            int count = GameManager.Inst.GetTutorialTeleportCounter();
            if (count < 3)
            {
                tutorialCountText.text = $"{count}회 / 3회 (진행중)";
            }
            else if (count == 3)
            {
                tutorialAction[2] = false;
                GameManager.Inst.TotorialCountStart = false;
                StartCoroutine(Sucsess(1));
                tutorialCountText.text = $"{count}회 / 3회 (완료)";
            }
        }
    }


    IEnumerator Sucsess(int value)
    {
        suseccPs[value].gameObject.SetActive(true);
        SoundManager.inst.F_Get_ControllSoundPreFabs_ETC_PlaySFX(1, 1);
        yield return new WaitForSeconds(1);
        suseccPs[value].gameObject.SetActive(false);
    }



}
