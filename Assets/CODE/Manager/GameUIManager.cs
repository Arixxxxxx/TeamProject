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
    [SerializeField] GameObject msgUI;
    TMP_Text msgtext;


    GameObject InfoObj;
    Animator infoAnim;
    Image infoBg;
    TMP_Text infoText;
    Color aZeroColor = new Color(1, 1, 1, 0);
    Color aColorUP = new Color(0, 0, 0, 0.3f);
    WaitForSeconds infoBarColse;
    
    bool playerInDarkCloud;
    GameObject inDarkCloudWarrningWindow;

   
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
        
    }
    void Start()
    {
        InfoObj = UI.transform.Find("GameInfoMSG").gameObject;
        infoText = InfoObj.GetComponentInChildren<TMP_Text>();
        infoAnim = infoText.GetComponent<Animator>();
        infoBg = InfoObj.transform.Find("Bg").GetComponent<Image>();
        infoBarColse = new WaitForSeconds(closeTime);
        inDarkCloudWarrningWindow = UI.transform.Find("WarningMSG").gameObject;
        msgtext = msgUI.transform.GetComponentInChildren<TMP_Text>();
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
            case 1:
                alramText = "< 성당 > 으로 가는 길이 열렸습니다.\n 맵 우측으로 이동하세요!";
                F_SetNextMapArrow(1);
                break;

            case 2:
                alramText = "< 숲 > 지형이 해금되었습니다. \n 맵 우측으로 이동하세요!";
               
                break;

            case 3:
                alramText = "< 어두운 숲 > 으로 향하는 포탈이 생성되었습니다.";
                break;

            case -1:
                alramText = "게임이 시작되었습니다.";
                F_SetNextMapArrow(0);
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
     
    public void F_SetNextMapArrow(int value)
    {
        MapArrow.F_SetTarget(value);
        MapArrow.gameObject.SetActive(true);    
    }
    
}
