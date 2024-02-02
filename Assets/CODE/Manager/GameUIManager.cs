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
    Dead_Counter_Contoller deadCon;
    Animator bombEffectAnim;
    GameObject HideObj0, HideObj1, HideObj2;
    [SerializeField] GameObject[] HideObj;
    bool skillEffectStop;
    public bool SkillEffectStop { get { return skillEffectStop; } set { skillEffectStop = value; } }

    bool respawning; // 리스폰중 체크용
    public bool Respawning { get { return respawning; } set { respawning = value; } }


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
        HideObj0 = UI.transform.Find("BattleTime").gameObject;
        HideObj1 = UI.transform.Find("Count_Info").gameObject;
        HideObj2 = UI.transform.Find("UnitFrame").gameObject;

        deadCon = GetComponent<Dead_Counter_Contoller>();
    }
    void Start()
    {
        InfoObj = UI.transform.Find("GameInfoMSG").gameObject;
        infoText = InfoObj.GetComponentInChildren<TMP_Text>();
        infoAnim = infoText.GetComponent<Animator>();
        infoBg = InfoObj.transform.Find("Bg").GetComponent<Image>();
        
        inDarkCloudWarrningWindow = UI.transform.Find("WarningMSG").gameObject;

        msgUI_Bg = msgUI.transform.GetChild(0).GetComponent<Image>();
        Img_0 = msgUI_Bg.transform.GetChild(0).GetComponent<Image>();
        Img_1 = msgUI_Bg.transform.GetChild(1).GetComponent<Image>();
        msgtext = msgUI.transform.Find("Bg/Text").GetComponent<TMP_Text>();
        bossHpUI = UI.transform.Find("Boos_Hp_Bar").gameObject;

        bombEffectAnim = UI.transform.Find("BombEffect").GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
  
    }

    /// <summary>
    ///  게임안내 문구 소환
    /// </summary>
    /// <param name="value"></param>
    public void F_GameInfoOpen(int value)
    {
        string alramText = string.Empty;
        
        switch (value)
        {
            case 0:
                infoText.fontSize = 30;
                alramText = "<b>< 교회 ></b> 로 가는 길이 열렸습니다.\n 맵 우측으로 이동하세요!";
                F_NextMapArrowActiveSec(true);
                DarkCloud_Controller.inst.F_darkCloudeSpeedUp(0, 0,2.5f);
                break;

            case 1:
                infoText.fontSize = 30;
                alramText = "<b>< 숲 ></b> 으로 가는 길이 열렸습니다. \n 맵 우측으로 이동하세요!";
                F_NextMapArrowActiveSec(true);
                DarkCloud_Controller.inst.F_darkCloudeSpeedUp(0,1, 2.5f);
                break;

            case 2:
                infoText.fontSize = 30;
                alramText = "< 어두운 숲 > 으로 향하는 길이 열렸습니다. \n 포탈로 이동해주세요!";
                DarkCloud_Controller.inst.F_Pattern2Active(true); // 어둠구름 이동
                DarkCloud_Controller.inst.F_darkCloudeSpeedUp(0, 2, 2.5f);
                F_NextMapArrowActiveSec(true);
                SpawnManager.inst.F_spawnstartActiveOff();
                break;

            case 4:
                alramText = "";
                break;

            case -1:
                infoText.fontSize = 38;
                alramText = "게임이 시작되었습니다.";

               

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
        

        while (infoText.color.a < 0.95f)
        {
            infoBg.color += aColorUP * Time.deltaTime * 5;
            infoText.color += aColorUP * Time.deltaTime * 5;
            yield return null;
        }
        

        infoAnim.enabled = true;
        yield return new WaitForSeconds(5);
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
    /// 이동화살표
    /// </summary>
    /// <param name="value"></param>
    public void F_SetNextMapArrow(int value)
    {
        MapArrow.F_SetTarget(value);
        MapArrow.gameObject.SetActive(true);    
    }

    public void F_NextMapArrowActiveSec(bool value)
    {
        MapArrow.gameObject.SetActive(value);
    }

      
    public void F_SetMSGUI(int value, bool BValue)
    {
        string textValue = string.Empty;

        // 해골마커 표시유무
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
                textValue = "서쪽에서부터 어둠의 기운이 몰려옵니다. 마녀를 찾아 무찔러야 합니다.";
                break;

            case 1:
                textValue = "감히 여기까지 쫓아오다니.. 너를 처단해주겠다..";
                break;

            case 2:
                textValue = "하늘에서 번개 떨어집니다";
                break;

            case 3:
                textValue = "바닥 조심하세요";
                break;

            case 4:
                textValue = "구슬공격할꺼에요";
                break;

        }

        msgtext.text = textValue;
        msgUI_Bg.gameObject.SetActive(true);
      

        msgUI.SetTrigger("On");
       
    }


    /// <summary>
    /// 셀렉트 옵션 활성화
    /// </summary>
    /// <param name="value"> 0 보스방 / 대기 </param>
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
        
        HideObj1.gameObject.SetActive(value);
        HideObj2.gameObject.SetActive(value);
        HideObj[0].gameObject.SetActive(value); // 스킬 버프창
        HideObj[1].gameObject.SetActive(value);
        HideObj[2].gameObject.SetActive(value);
        
    }

    public void F_BombEffectOn()
    {
        bombEffectAnim.SetTrigger("Bomb");
    }

    
    public void F_CallRespawn_Counter_UI()
    {
        deadCon.F_ActiveDeadCounter();
    }
    
  
}
