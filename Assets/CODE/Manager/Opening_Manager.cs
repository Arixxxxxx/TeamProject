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
    [SerializeField] Transform camtransform; // ī�޶� ������ġ
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
    //WASD Ʃ�丮�� �ʱ�ȭ
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
        GameManager.Inst.F_PlayerTransformMove(playerStartPos.position); // �÷��̾� ��ġ �ʱ�ȭ

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
        // ��Ʈ�� ���丮 ����
        Action0 = true;
        StartCoroutine(IntroStoryAction0());


        //Action0 ȭ�� ���̵����
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
        yield return new WaitForSeconds(1f); // �׸����� �ִϸ��̼� �۵��ɶ����� ��ٸ���
        photoFrameAnim.SetTrigger("FadeOn"); // �ڽ� ��������Ʈ ����
        nextOk = true;
        yield return new WaitForSeconds(1.5f);
        fastInfo.gameObject.SetActive(true); // Spacebar Fast ���� ���ֱ�
        //backgroundPs[0].gameObject.SetActive(true); // ��ƼŬ ���(24/01.26)
        //backgroundPs[1].gameObject.SetActive(true);// ��ƼŬ ���(24/01.26)
        maintext.F_Set_TalkBox_Main_Text(textBox[0]); // ù��° �����׽��丮 ���� �־��ֱ�

        //
        SoundPreFabs sc = SoundManager.inst.F_Get_ControllSoundPreFabs_ETC_PlaySFX(0, 1);
        sc.F_SetVolume(0.7f);
        sc.F_SetSoundLoop(true);

        inputText = true;

        yield return null;

        while (nextOk == true) //���������� ��ٸ��� 
        {
            yield return null;
        }

        sc.F_QuickEndSound();

        yield return new WaitForSeconds(2f); // �� �ٳ������� 2�� ���
        photoFrameAnim.SetTrigger("FadeOff"); // ù��° �׸� ���̵� �ƿ�
        yield return new WaitForSeconds(0.5f);
        //maintext.F_TextEmpty(); // ��ȭâ �ѹ������
        maintext.F_HideText(5.5f);

        yield return new WaitForSeconds(0.7f); // ��񽬰�

        frameIMG.sprite = introSprite[0]; // �ι��� �������� ����
        photoFrameAnim.SetTrigger("FadeOn2"); // �ִϸ��̼� ���

        nextOk = true; // �޼��� ��� �����ٸ�
        yield return new WaitForSeconds(1f);
        maintext.F_Set_TalkBox_Main_Text(textBox[1]); // �ι��� �����׽��丮 ���� �־��ֱ�

        //
        SoundPreFabs sc1 = SoundManager.inst.F_Get_ControllSoundPreFabs_ETC_PlaySFX(0, 1);
        sc1.F_SetVolume(0.7f);
        sc1.F_SetSoundLoop(true);
        yield return null;

        while (nextOk == true) // �޽��� ��µ��� ���
        {
            yield return null;
        }
        sc1.F_QuickEndSound(); // ���� ����
        inputText = false;
        fastInfo.gameObject.SetActive(false); // Spacebar Fast ���� ���ֱ�
        yield return new WaitForSeconds(2f);// �� �ٳ������� 2�� ���
        photoFrameAnim.SetTrigger("FadeOff2"); // �̹��� ���̵�ƿ�

        yield return new WaitForSeconds(0.5f); // ��� �����ٰ�
        maintext.F_HideText(1.2f); // �ؽ�Ʈ ���̵�ƿ�


        Action0 = false; // ���� ���� ����


    }

    IEnumerator CuttonFade()
    {
        while (Action0 == true)
        {
            yield return null;
        }

        cm.F_OP_CamTargetSetting(GameManager.Inst.F_GetPalyerTargetPoint(), 25, false, camtransform); // ī�޶� ��ġ �� �ޱ۰� �ʱ�ȭ

        yield return new WaitForSeconds(CuttonFadeOffTime);

        bgAnim.SetTrigger("Off"); // ����ȭ�� ����

        yield return new WaitForSeconds(Action1_0_DelayTime);

        bgAnim.transform.parent.gameObject.SetActive(false);  //  ����ȭ�� Enable false
        cm.F_OP_CamTargetSetting(GameManager.Inst.F_GetPalyerTargetPoint(), 11f, true, camtransform); // fade in + Camera �̵� ����


    }


    // �׼� 2���� ����Ű ���� => ȣ���� ī�޶�Ŵ������� ��

    public void F_Action2Start()
    {
        StartCoroutine(Action2_Start());
      
        
    }
    SoundPreFabs sc;
    IEnumerator Action2_Start()
    {
        

        if (GameManager.Inst.TotalkillCount == 0) // ���� �����̶��
        {
            SoundManager.inst.F_SetBGMVolume(0.4f, 0.5f); // ������� ���� ���߱�

            yield return new WaitForSeconds(2f);
            sc = SoundManager.inst.F_Get_ControllSoundPreFabs_InfoNarrtionSFX(0,1);
            ActionInfo[0].gameObject.SetActive(true);
            GameManager.Inst.MoveStop = false;
            ActionInfo[0].SetTrigger("In");
            yield return new WaitForSeconds(5f);

            ActionInfo[0].SetTrigger("Out"); // ���̵� �ƿ�
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

            ActionInfo[1].gameObject.SetActive(true); // WASD ���ۺ� ���� UI ����
            GameManager.Inst.MoveStop = false; // ĳ���� �����̰� ����
            ActionInfo[1].SetTrigger("In"); // ���̵��� ���

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

            ActionInfo[1].SetTrigger("Out"); // ���̵� �ƿ�
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

            ActionInfo[2].gameObject.SetActive(true);// ����Ʈ, �����̽��� ���� UI ����
            ActionInfo[2].SetTrigger("In"); // ���̵��� ���


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
            ActionInfo[2].SetTrigger("Out"); // �ƿ�
            tutorialBarAnim.SetTrigger("Close");
            yield return new WaitForSeconds(2f);
            ActionInfo[2].gameObject.SetActive(false); // ����
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

            // �����̵� Ʃ�丮��
            yield return new WaitForSeconds(0.5f);
            sc = SoundManager.inst.F_Get_ControllSoundPreFabs_InfoNarrtionSFX(4,1);
            yield return new WaitForSeconds(0.5f);

            ActionInfo[3].gameObject.SetActive(true);// �����̽��� ���� UI ����
            ActionInfo[3].SetTrigger("In"); // ���̵��� ���
                                            //���� 

            tutorialAction[2] = true;

            yield return null;

            while (tutorialAction[2])
            {
                yield return null;
            }
            //

            yield return new WaitForSeconds(1f);
            ActionInfo[3].SetTrigger("Out"); // �ƿ�
            yield return new WaitForSeconds(2f); //
            ActionInfo[3].gameObject.SetActive(false); // ����
            yield return new WaitForSeconds(1f);
            // �����̵� Ʃ�丮�� ����

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

            howtoPlayEnd = true; // ����üũ �Ҹ��� ����
            GameManager.Inst.F_TimeSclaeController(true);  // �ð� ����

            howToPlay_MainObj.SetActive(true); //  ����â ����
            if (howToPlay_Action0.activeSelf == false)
            {
                howToPlay_Action0.SetActive(true);
            }

            while (howtoPlayEnd == true) //���������� ���
            {
                yield return null;
            }

            yield return new WaitForSeconds(1.5f);
            sc = SoundManager.inst.F_Get_ControllSoundPreFabs_InfoNarrtionSFX(6, 1);
            yield return null;

        }


        // �ٷν���
        GameManager.Inst.MoveStop = false;
        yield return new WaitForSeconds(1f);
        SoundManager.inst.F_Bgm_Player(1, 0.5f, 0.7f);
        GameUIManager.Inst.F_GameUIActive(true); // UI���ְ�

        if(sc != null)
        {
            while (sc.soundEnd)
            {
                yield return null;
            }
        }
        

        yield return new WaitForSeconds(1.5f);
        GameManager.Inst.MainGameStart = true; // ���ӽ���

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

        if (howtoPlayWaittime) // ������ �ɾ��ִ� bool ����
        {
            count += Time.fixedUnscaledDeltaTime; // �����ϵ�ŸŸ��

            if (count > 1)
            {
                count = 0;
                howtoPlayWaittime = false;
            }
        }

        if (howToPlay_Action1.activeSelf == true && Input.GetKeyDown(KeyCode.Space) && howtoPlayWaittime == false) // �ٷ� �Է� �ȵǰ� ������ ��
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
                tutorialCountText.text = $"{count}ȸ / 3ȸ (������)";
            }
            else if (count == 3)
            {
                tutorialAction[2] = false;
                GameManager.Inst.TotorialCountStart = false;
                StartCoroutine(Sucsess(1));
                tutorialCountText.text = $"{count}ȸ / 3ȸ (�Ϸ�)";
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
