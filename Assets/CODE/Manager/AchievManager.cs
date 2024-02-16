using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievManager : MonoBehaviour
{
    public static AchievManager inst;

    Transform mainUITrs;

    Button[] popupBtn = new Button[2];
    Button exitBtn;

    GameObject AchievMainOBJ;

    GameObject boxTrs;
    GameObject[] boxCase = new GameObject[3];
    GameObject[] medal = new GameObject[3];
    Image[] fillBar = new Image[3];
    TMP_Text[] textbar = new TMP_Text[3];
    TMP_Text LoginID_Text;

    bool[] AchievComplete = new bool[3];
    [SerializeField] Sprite[] CompleteIMG;

    Animator completeObj;
    Image completeWindowIMG;
    TMP_Text completeText;


    GameObject btnCircle; // 버튼 알림생성 숫자
    TMP_Text CircleText;
    int alramTextNum;

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
    }
    void Start()
    {
        mainUITrs = GameUIManager.Inst.F_GetMainCanvasTRS();

        // 컴플리트 상자 액션 변수 초기화
        completeObj = mainUITrs.transform.Find("AchievComplete").GetComponent<Animator>();
        completeWindowIMG = completeObj.transform.Find("Box/IMG").GetComponent<Image>();
        completeText = completeWindowIMG.transform.parent.Find("InfoText").GetComponent<TMP_Text>();

        // 버튼 초기화
        popupBtn[0] = mainUITrs.transform.Find("AchievBtn").GetComponent<Button>();
        popupBtn[1] = mainUITrs.transform.Find("BattleStatsBtn").GetComponent<Button>();

        btnCircle = popupBtn[0].transform.Find("Circle").gameObject;
        CircleText = btnCircle.GetComponentInChildren<TMP_Text>();


        // 업적창 초기화
        AchievMainOBJ = mainUITrs.transform.Find("AchievWindow").gameObject;
        exitBtn = AchievMainOBJ.transform.Find("Case/ExitBtn").GetComponent<Button>();
        LoginID_Text = AchievMainOBJ.transform.Find("Case/LoginID_Text").GetComponent<TMP_Text>();

        boxTrs = AchievMainOBJ.transform.Find("Case/Vertical_Group").gameObject;
        boxCase[0] = boxTrs.transform.Find("Box0").gameObject;
        boxCase[1] = boxTrs.transform.Find("Box1").gameObject;
        boxCase[2] = boxTrs.transform.Find("Box2").gameObject;

        medal[0] = boxCase[0].transform.Find("Medal").gameObject;
        medal[1] = boxCase[1].transform.Find("Medal").gameObject;
        medal[2] = boxCase[2].transform.Find("Medal").gameObject;

        fillBar[0] = boxCase[0].transform.Find("FillBar").GetComponent<Image>();
        fillBar[1] = boxCase[1].transform.Find("FillBar").GetComponent<Image>();
        fillBar[2] = boxCase[2].transform.Find("FillBar").GetComponent<Image>();



        textbar[0] = fillBar[0].transform.Find("Text").GetComponent<TMP_Text>();
        textbar[1] = fillBar[1].transform.Find("Text").GetComponent<TMP_Text>();
        textbar[2] = fillBar[2].transform.Find("Text").GetComponent<TMP_Text>();

        Init_Btn();
        InitData();
    }

   
    void Update()
    {
        F_UpdateData(0); // 보스킬 
        F_UpdateData(1); // 킬카운트
        F_UpdateData(2); // 레벨업

    }
    private void Init_Btn()
    {
        popupBtn[0].onClick.AddListener(() =>  // 게임화면 버튼 초기화
        {
            LvUp_Ui_Manager.Inst.F_PlayerRunSounStop(); // 혹시 뛰고잇다면 소리종료
            AchievMainOBJ.SetActive(true);
            GameManager.Inst.F_TimeSclaeController(true);
            SoundManager.inst.F_Get_ControllSoundPreFabs_ETC_PlaySFX(3, 1);
            BtnLock(false);

            if (btnCircle.activeSelf == true) // 알림 서클 켜져잇으면 꺼줌
            {
                btnCircle.SetActive(false);
                alramTextNum = 0;
            }

        });

        exitBtn.onClick.AddListener(() =>
        {
            BtnLock(true);
            AchievMainOBJ.SetActive(false);
            GameManager.Inst.F_TimeSclaeController(false);
            SoundManager.inst.F_Get_ControllSoundPreFabs_ETC_PlaySFX(3, 1);
        });
    }

    public void InitData()
    {
        for (int i = 0; i < fillBar.Length; i++)
        {
            fillBar[i].fillAmount = 0;
            medal[i].gameObject.SetActive(false);
            textbar[i].color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
        LoginID_Text.text = string.Empty;


        UserGameInfo UserGameData = DataManager.inst.F_GetUserData();
        // User ID 초기화

        string UserID = DataManager.inst.F_GetUserID();

        if (UserID != null)
        {
            LoginID_Text.text = $"< LogIn ID : {UserID} >"; // 데이터매니저에서 로그인 내역 받아오기
        }
        else
        {
            LoginID_Text.text = $"< LogIn ID : NotLogIn >"; // 데이터매니저에서 로그인 내역 받아오기
        }

        // fillbar 초기화 

        fillBar[0].fillAmount = (float)UserGameData.bossKillCount / 1;  // 보스1회킬 (변경)
        fillBar[1].fillAmount = (float)UserGameData.totalKillEnemy / 1000;  // 킬카운트업적
        fillBar[2].fillAmount = (float)UserGameData.LevelUpCount / 50;  // 누적 레벨업업적

        for (int i = 0; i < fillBar.Length; i++) // 메달 카운트 초기화
        {
            if (fillBar[i].fillAmount == 1)
            {
                AchievComplete[i] = true;
                medal[i].gameObject.SetActive(true);

                switch (i) //  Text 초기화
                {
                    case 0:
                        textbar[i].text = $" 1 / 1 (완료)";
                        textbar[i].color = Color.white;
                        break;

                    case 1:
                        textbar[i].text = $" 1000 / 1000 (완료)";
                        textbar[i].color = Color.white;
                        break;

                    case 2:
                        textbar[i].text = $" 50 / 50 (완료)";
                        textbar[i].color = Color.white;
                        break;

                }


            }
        }
    }

    int[] countValue = new int[3];

    public void F_UpdateData(int value)
    {
        countValue = GameManager.Inst.F_GetUserGameCountData();

        if (AchievComplete[value] == false) // 보스킬 업적창
        {

            if (fillBar[value].fillAmount == 1) // 완료시 
            {
                AchievComplete[value] = true;

                fillBar[value].fillAmount = 1;
                textbar[value].color = Color.white;

                switch (value)
                {
                    case 0:
                        textbar[value].text = $"1 / 1 (완료)";
                        break;

                    case 1:
                        textbar[1].text = $"1000 / 1000 (완료)";
                        break;

                    case 2:
                        textbar[2].text = $"50 / 50 (완료)";
                        break;

                }
                medal[value].gameObject.SetActive(true);

                CompleteAction(value); // 업적 액션 틀어줌
                CreateAlramBtn();

            }
            else if (fillBar[value].fillAmount < 1) // 미완료시 업데이트
            {
                switch (value)
                {
                    case 0:
                        fillBar[value].fillAmount = (float)countValue[value] / 1;
                        textbar[value].text = $"{countValue[value]} / 3 (진행중)";
                        break;

                    case 1:
                        fillBar[value].fillAmount = (float)countValue[value] / 1000;
                        textbar[value].text = $"{countValue[value]} / 1000 (진행중)";
                        break;

                    case 2:
                        fillBar[value].fillAmount = (float)countValue[value] / 50;
                        textbar[value].text = $"{countValue[value]} / 50 (진행중)";

                        break;

                }
            }
        }
    }

    private void CompleteAction(int value)
    {
        completeWindowIMG.sprite = CompleteIMG[value];
        
        string text = string.Empty;

        switch (value)
        {
            case 0:
                text = "어둠 파훼 - 첫 보스 처치";
                break;

            case 1:
                text = "기사의자격 - 1000킬 달성";
                break;

            case 2:
                text = "왕궁 수호자 - 누적 레벨 50 달성";
                break;
        }

        completeText.text = text;
        StopCoroutine(PlayAnim());
        StartCoroutine(PlayAnim());
    }

    IEnumerator PlayAnim()
    {
        completeObj.gameObject.SetActive(true);
        completeObj.SetTrigger("Play");
        SoundManager.inst.F_Get_ControllSoundPreFabs_ETC_PlaySFX(13, 1);
        yield return new WaitForSeconds(3f);
        completeObj.SetTrigger("End");
        yield return new WaitForSeconds(2f);
        completeObj.gameObject.SetActive(false);
    }

    /// <summary>
    /// 메인화면 버튼에 알림갯수 생성해줌
    /// </summary>
    private void CreateAlramBtn()
    {
        alramTextNum++;
        CircleText.text = alramTextNum.ToString();
        btnCircle.gameObject.SetActive(true);
    }

    public void BtnLock(bool value)
    {
        for (int i = 0; i < popupBtn.Length; i++)
        {
            popupBtn[i].interactable = value;
        }
    }
}
