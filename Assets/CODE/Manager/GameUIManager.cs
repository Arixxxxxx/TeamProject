using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Inst;

    [SerializeField] GameObject UI;

    // InGame Info Bar
    [Header("# Info Window Close Time")]
    [Space]
    [SerializeField] float closeTime;
    [Header("#Next Map Arrow Setting")]
    [Space]
    [SerializeField] NextMapArrow MapArrow;
    [Header("#Msg info")]
    [Space]
    [SerializeField] Animator msgUI;
    Image msgUI_Bg;
    Image Img_0;
    Image Img_1;
    TMP_Text msgtext;
    GameObject bossHpUI;

    GameObject InfoObj;
    Animator infoAnim;
    Image infoBg;
    TMP_Text infoText;
    Color aZeroColor = new Color(1, 1, 1, 0);
    Color aColorUP = new Color(0, 0, 0, 0.3f);
    WaitForSeconds infoBarColse;
    
    bool playerInDarkCloud;
    GameObject inDarkCloudWarrningWindow;
    OptionWindow_Controller optionWindow_Con;

    GameObject HideObj0, HideObj1, HideObj2;


    private void Awake()
    {
        if(Inst == null)
        {
            Inst = this;
        }
        else
        {
            Destroy(this);
        }

        optionWindow_Con = GetComponent<OptionWindow_Controller>();
    }
    void Start()
    {
        InfoObj = UI.transform.Find("GameInfoMSG").gameObject;
        infoText = InfoObj.GetComponentInChildren<TMP_Text>();
        infoAnim = infoText.GetComponent<Animator>();
        infoBg = InfoObj.transform.Find("Bg").GetComponent<Image>();
        infoBarColse = new WaitForSeconds(closeTime);
        inDarkCloudWarrningWindow = UI.transform.Find("WarningMSG").gameObject;

        msgUI_Bg = msgUI.transform.GetChild(0).GetComponent<Image>();
        Img_0 = msgUI_Bg.transform.GetChild(0).GetComponent<Image>();
        Img_1 = msgUI_Bg.transform.GetChild(1).GetComponent<Image>();
        msgtext = msgUI.transform.Find("Bg/Text").GetComponent<TMP_Text>();

        bossHpUI = UI.transform.Find("Boos_Hp_Bar").gameObject;

        HideObj0 = UI.transform.Find("BattleTime").gameObject;
        HideObj1 = UI.transform.Find("Count_Info").gameObject;
        HideObj2 = UI.transform.Find("UnitFrame").gameObject;
    }
    // Update is called once per frame
    void Update()
    {
  
    }

    /// <summary>
    ///  ���Ӿȳ� ���� ��ȯ
    /// </summary>
    /// <param name="value"></param>
    public void F_GameInfoOpen(int value)
    {
        string alramText = string.Empty;
        
        switch (value)
        {
            case 1:
                alramText = "<b>< ��ȸ ></b> �� ���� ���� ���Ƚ��ϴ�.\n �� �������� �̵��ϼ���!";
                F_SetNextMapArrow(1);
                break;

            case 2:
                alramText = "<b>< �� ></b> ���� ���� ���� ���Ƚ��ϴ�. \n �� �������� �̵��ϼ���!";
               
                break;

            case 3:
                alramText = "< ��ο� �� > ���� ���ϴ� ���� ���Ƚ��ϴ�. \n ��Ż�� �̵����ּ���!";
                F_SetNextMapArrow(2); // ȭ��ǥ �˾�
                break;

            case 4:
                alramText = "";
                break;

            case -1:
                alramText = "������ ���۵Ǿ����ϴ�.";

                F_SetNextMapArrow(0); // ȭ��ǥ �˾�

                break;
        }
        
        infoText.text = alramText;

       

        if (InfoObj.gameObject.activeSelf == false) 
        {
            InfoObj.gameObject.SetActive(true);
        }

        StopCoroutine(MsgInfoBar_Action());
        StartCoroutine(MsgInfoBar_Action());
    }

    
    IEnumerator MsgInfoBar_Action()
    {
        
        while(infoText.color.a < 0.95f)
        {
            infoBg.color += aColorUP * Time.deltaTime * 5;
            infoText.color += aColorUP * Time.deltaTime * 5;
            yield return null;
        }

        infoAnim.enabled = true;
        yield return infoBarColse;
        infoAnim.enabled = false;

        while (infoText.color.a > 0.05f)
        {
            infoBg.color -= aColorUP * Time.deltaTime * 5;
            infoText.color -= aColorUP * Time.deltaTime * 5;
            yield return null;
        }

        if (InfoObj.gameObject.activeSelf == true)
        {
            InfoObj.gameObject.SetActive(false);
        }

    }

    public void F_SetDarkCloudWindow_OnOff(bool value)
    {
        if (value == true && inDarkCloudWarrningWindow.activeSelf == false) 
        {
            inDarkCloudWarrningWindow.SetActive(true);
        }
        else if(value == false && inDarkCloudWarrningWindow.activeSelf == true)
        {
            inDarkCloudWarrningWindow.SetActive(false);
        }
    }
     
    /// <summary>
    /// �̵�ȭ��ǥ
    /// </summary>
    /// <param name="value"></param>
    public void F_SetNextMapArrow(int value)
    {
        MapArrow.F_SetTarget(value);
        MapArrow.gameObject.SetActive(true);    
    }
    
    public void F_SetMSGUI(int value, bool BValue)
    {
        string textValue = string.Empty;

        // �ذ�Ŀ ǥ������
        if (BValue)
        {
            Img_0.gameObject.SetActive(true);
            Img_1.gameObject.SetActive(true);
        }
        else
        {
            Img_0.gameObject.SetActive(false);
            Img_1.gameObject.SetActive(false);
        }

        switch (value)
        {
            case 0:
                textValue = "���ʿ������� ����� ����� �����ɴϴ�. ���ฦ ã�� ���񷯾� �մϴ�.";
                break;

            case 1:
                textValue = "���� ������� �Ѿƿ��ٴ�.. �ʸ� ó�����ְڴ�..";
                break;

        }

        msgtext.text = textValue;
        msgUI_Bg.gameObject.SetActive(true);
      

        msgUI.SetTrigger("On");
       
    }


    /// <summary>
    /// ����Ʈ �ɼ� Ȱ��ȭ
    /// </summary>
    /// <param name="value"> 0 ������ / ��� </param>
    public void F_OpenSelectWindow(int value)
    {
        optionWindow_Con.F_SetSelectWindowUI_Open(value);
        GameManager.Inst.MoveStop = true;
    }

    public void F_BossHpBarActive(bool value)
    {
        bossHpUI.gameObject.SetActive(value);
    }
    public void F_GameUIActive(bool value)
    {
        HideObj0.gameObject.SetActive(value);
        HideObj1.gameObject.SetActive(value);
        HideObj2.gameObject.SetActive(value);
    }
}
