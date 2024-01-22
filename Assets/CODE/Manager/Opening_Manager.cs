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
    [SerializeField] Transform camtransform; // ī�޶� ������ġ
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
    IEnumerator IntroStoryAction0()
    {
        photoFrameAnim.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f); // �׸����� �ִϸ��̼� �۵��ɶ����� ��ٸ���
        mask.SetTrigger("FadeOn"); // �ڽ� ��������Ʈ ����
        backgroundPs[0].gameObject.SetActive(true);
        backgroundPs[1].gameObject.SetActive(true);
        nextOk = true;
        maintext.F_Set_TalkBox_Main_Text(textBox[0]); // ù��° �����׽��丮 ���� �־��ֱ�
        yield return null;

        while(nextOk == true) //���������� ��ٸ��� 
        {
            yield return null; 
        }
        mask.SetTrigger("FadeOff"); // ù��° �׸� ���̵� �ƿ�
        yield return new WaitForSeconds(0.5f); 
        maintext.F_TextEmpty(); // ��ȭâ �ѹ������

        yield return new WaitForSeconds(2f); // ��񽬰�
        
        maskIMG.sprite = introSprite[0]; // �ι��� �������� ����
        mask.SetTrigger("FadeOn2"); // �ִϸ��̼� ���

        nextOk = true; // �޼��� ��� �����ٸ�
        maintext.F_Set_TalkBox_Main_Text(textBox[1]); // �ι��� �����׽��丮 ���� �־��ֱ�
      
        yield return null;

        while (nextOk == true) // �޽��� ��µ��� ���
        {
            yield return null;
        }
        mask.SetTrigger("FadeOff2"); // �̹��� ���̵�ƿ�
        photoFrameAnim.SetTrigger("FadeOut");
        yield return new WaitForSeconds(0.5f); // ��� �����ٰ�
        maintext.F_HideText(); // �ؽ�Ʈ ���̵�ƿ�
        
      
        Action0 = false; // ���� ���� ����
    }

    IEnumerator CuttonFade()
    {
        while(Action0 == true)
        {
            yield return null;
        }

        cm.F_OP_CamTargetSetting(cameraTarget.transform, 27, false, camtransform); // ī�޶� ��ġ �� �ޱ۰� �ʱ�ȭ

        yield return new WaitForSeconds(CuttonFadeOffTime);
        bgAnim.SetTrigger("Off"); // ����ȭ�� ����
        for (int i = 0; i < backgroundPs.Length; i++)
        {
            backgroundPs[i].gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(Action1_0_DelayTime);
        bgAnim.transform.parent.gameObject.SetActive(false);  //  ����ȭ�� Enable false

        // Action 1 ���� (���Ŀ� �Լ� ���� ��ġ �̵�)
        cm.F_OP_CamTargetSetting(cameraTarget.transform, 9.5f, true, camtransform); // fade in + Camera �̵� ����

       
    }
    

    // �׼� 2���� ����Ű ���� => ȣ���� ī�޶�Ŵ������� ��

    public void F_Action2Start()
    {
        StartCoroutine(Action2_Start());
    }

    IEnumerator Action2_Start()
    {
        GameManager.Inst.MoveStop = false; // ĳ���� �����̰� ����
        mainUiCanvas.gameObject.SetActive(true); // ���� UI ����

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

        yield return new WaitForSeconds(2f);
        Action2_1.gameObject.SetActive(false); // ����



    }

}
