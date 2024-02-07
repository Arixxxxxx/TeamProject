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
        GameManager.Inst.F_PlayerTransformMove(playerStartPos.position); // �÷��̾� ��ġ �ʱ�ȭ
        
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
        inputText = true;

        yield return null;

        while(nextOk == true) //���������� ��ٸ��� 
        {
            yield return null; 
        }
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
      
        yield return null;

        while (nextOk == true) // �޽��� ��µ��� ���
        {
            yield return null;
        }
        inputText = false;
        fastInfo.gameObject.SetActive(false); // Spacebar Fast ���� ���ֱ�
        yield return new WaitForSeconds(2f);// �� �ٳ������� 2�� ���
        photoFrameAnim.SetTrigger("FadeOff2"); // �̹��� ���̵�ƿ�
        
        yield return new WaitForSeconds(0.5f); // ��� �����ٰ�
        maintext.F_HideText(1.2f); // �ؽ�Ʈ ���̵�ƿ�

       
          Action0 = false; // ���� ���� ����
        SoundManager.inst.F_Bgm_Player(1,0.25f);
    }

    IEnumerator CuttonFade()
    {
        while(Action0 == true)
        {
            yield return null;
        }

        cm.F_OP_CamTargetSetting(GameManager.Inst.F_GetPalyerTargetPoint(), 25, false, camtransform); // ī�޶� ��ġ �� �ޱ۰� �ʱ�ȭ

        yield return new WaitForSeconds(CuttonFadeOffTime);
        bgAnim.SetTrigger("Off"); // ����ȭ�� ����

        //for (int i = 0; i < backgroundPs.Length; i++) // �����ƼŬ �̻��
        //{
        //    backgroundPs[i].gameObject.SetActive(false);
        //}

        yield return new WaitForSeconds(Action1_0_DelayTime);

        //bgAnim.SetTrigger("On");  // ���̵�ƿ� �� ĳ���� ���� ����
        //yield return new WaitForSeconds(1);
        //cm.F_DirectAction();
        //yield return new WaitForSeconds(0.5f);
        //bgAnim.SetTrigger("Off");
        //yield return new WaitForSeconds(2.1f);
        
        bgAnim.transform.parent.gameObject.SetActive(false);  //  ����ȭ�� Enable false
        //F_Action2Start();
        // Action 1 ���� (���Ŀ� �Լ� ���� ��ġ �̵�)

        cm.F_OP_CamTargetSetting(GameManager.Inst.F_GetPalyerTargetPoint(), 9.5f, true, camtransform); // fade in + Camera �̵� ����


    }
    

    // �׼� 2���� ����Ű ���� => ȣ���� ī�޶�Ŵ������� ��

    public void F_Action2Start()
    {
        StartCoroutine(Action2_Start());
    }

    IEnumerator Action2_Start()
    {
        GameManager.Inst.MoveStop = false; // ĳ���� �����̰� ����
 
        yield return new WaitForSeconds(Action2_DelayTime);

        Action2_0.gameObject.SetActive(true); // WASD ���ۺ� ���� UI ����
        Action2_0.SetTrigger("In"); // ���̵��� ���
        yield return new WaitForSeconds(3f);

        Action2_0.SetTrigger("Out"); // ���̵� �ƿ�
        

        yield return new WaitForSeconds(2f);

        Action2_0.gameObject.SetActive(false); 
        Action2_1.gameObject.SetActive(true);// ����Ʈ, �����̽��� ���� UI ����
        Action2_1.SetTrigger("In"); // ���̵��� ���
        yield return new WaitForSeconds(3f);

        Action2_1.SetTrigger("Out"); // �ƿ�

        yield return new WaitForSeconds(2f); //
        Action2_1.gameObject.SetActive(false); // ����

        // ���Ӽ���
        howtoPlayEnd = true; // ����üũ �Ҹ��� ����
        GameManager.Inst.F_TimeSclaeController(true);  // �ð� ����

        howToPlay_MainObj.SetActive(true); //  ����â ����
        if(howToPlay_Action0.activeSelf == false)
        {
            howToPlay_Action0.SetActive(true);
        }

        while(howtoPlayEnd == true) //���������� ���
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(1f); 

        GameUIManager.Inst.F_GameUIActive(true); // UI���ְ�

        yield return new WaitForSeconds(1.5f);
        GameManager.Inst.MainGameStart = true; // ���ӽ���
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

        if (howtoPlayWaittime) // ������ �ɾ��ִ� bool ����
        {
            count += Time.fixedUnscaledDeltaTime; // �����ϵ�ŸŸ��

            if(count > 1)
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
}
