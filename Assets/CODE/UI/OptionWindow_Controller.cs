using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class OptionWindow_Controller : MonoBehaviour
{
    [Header("# Insert Main UI GameOBJ")]
    [Space]
    [SerializeField] GameObject Ui_Parent_Obj;
        
    GameObject MainCanvas;
    GameObject SeletWindowList;
    GameObject Window_0;

    //인게임 스크린UI 버튼들
     Button OptionOpenBtn;

    // 옵션창 버튼들
     Button[] MainOption_Btn = new Button[3];
     

    /// Yes or No 버튼 
    GameObject selectWindow;
    TMP_Text optionText;
    Button yesBtn;
    Button noBtn;
    GameObject backLight;
    GameObject OptionWindow;
    GameObject SoundOption;

    //옵션 설정 변수
    Slider[] OptionSlider = new Slider[3];
    Button BackBtn;

    GameObject AchiveWindow; // 업적창
    GameObject BattleWindow; // 기록창
    
    void Start()
    {
        // 기본 UI ref 참조용 초기화
        MainCanvas = Ui_Parent_Obj.transform.Find("Main_Canvas").gameObject;
        SeletWindowList = Ui_Parent_Obj.transform.Find("IngameSelectWindow").gameObject;

        //메인스크린 버튼 초기화
        OptionWindow = SeletWindowList.transform.Find("OptionWindow").gameObject;
        SoundOption = SeletWindowList.transform.Find("SoundOption").gameObject;
        OptionSlider = SoundOption.GetComponentsInChildren<Slider>();
        BackBtn = SoundOption.transform.Find("BackBtn").GetComponent<Button>();

        OptionSlider[0].onValueChanged.AddListener(AudioMixer_Controller.inst.F_SetMasterVolume);
        OptionSlider[1].onValueChanged.AddListener(AudioMixer_Controller.inst.F_SetMusicVolume);
        OptionSlider[2].onValueChanged.AddListener(AudioMixer_Controller.inst.F_SetSFXVolume);


        // 1번창 (인게임 햄버거모양 누르면 나오는 옵션창)
        Window_0 = SeletWindowList.transform.GetChild(0).gameObject;
        MainOption_Btn = Window_0.transform.GetComponentsInChildren<Button>();
        

        //선택창 초기화
        selectWindow = MainCanvas.transform.Find("SelectBtn").gameObject;
        optionText = selectWindow.transform.Find("Bg/Text").GetComponent<TMP_Text>();
        yesBtn = selectWindow.transform.Find("Bg/Yes").GetComponent <Button>();
        noBtn = selectWindow.transform.Find("Bg/No").GetComponent <Button>();
        
        backLight = selectWindow.transform.Find("Bg/Light2").gameObject;

        AchiveWindow = MainCanvas.transform.Find("AchievWindow").gameObject;
        BattleWindow = MainCanvas.transform.Find("BattaleStatsWindow").gameObject;
        

        Btn_Init();
    }

    /// <summary>
    /// 선택창 초기화 함수
    /// </summary>
    /// <param name="value">0 보스방 출입 / </param>
    public void F_SetSelectWindowUI_Open(int value)
    {
        yesBtn.onClick.RemoveAllListeners();
        noBtn.onClick.RemoveAllListeners();
        optionText.text = string.Empty;


        switch (value)
        {
            case 0:
                optionText.text = "마녀방 (보스) 으로 이동하시겠습니까?";

                yesBtn.onClick.AddListener(() =>  //Yes 버튼
                {
                    backLight.SetActive(false);
                    GameManager.Inst.TelePort(1);
                    // 연출

                    //창닫기
                    selectWindow.gameObject.SetActive(false);
                });

                noBtn.onClick.AddListener(() =>  // No 버튼
                {
                    backLight.SetActive(false);
                    selectWindow.gameObject.SetActive(false);
                    GameManager.Inst.MoveStop = false;
                  //창닫기

                });
                break;
        }

        selectWindow.gameObject.SetActive(true);

    }
    // Update is called once per frame
    void Update()
    {
        ActiveEscKeyOption();
    }

    private void ActiveEscKeyOption()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && AchiveWindow.activeSelf == false && BattleWindow.activeSelf == false)
        {
            SoundManager.inst.F_Get_ControllSoundPreFabs_ETC_PlaySFX(3, 1);
            if (OptionWindow.gameObject.activeSelf == false && SoundOption.activeSelf == false)
            {
                OptionWindow.gameObject.SetActive(true);
                GameManager.Inst.F_TimeSclaeController(true);
            }
            else if(OptionWindow.gameObject.activeSelf == true && SoundOption.activeSelf == false)
            {
                OptionWindow.gameObject.SetActive(false);
                GameManager.Inst.F_TimeSclaeController(false);
            }
            else if(OptionWindow.gameObject.activeSelf == false && SoundOption.activeSelf == true)
            {
                OptionWindow.gameObject.SetActive(true);
                SoundOption.gameObject.SetActive(false);
            }
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && AchiveWindow.activeSelf == true)
        {
            AchievManager.inst.BtnLock(true);
            AchiveWindow.SetActive(false);
            GameManager.Inst.F_TimeSclaeController(false);
            SoundManager.inst.F_Get_ControllSoundPreFabs_ETC_PlaySFX(3, 1);
            
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && BattleWindow.activeSelf == true)
        {
            AchievManager.inst.BtnLock(true);
            BattleWindow.SetActive(false);
            GameManager.Inst.F_TimeSclaeController(false);
            SoundManager.inst.F_Get_ControllSoundPreFabs_ETC_PlaySFX(3, 1);
        }
    }
    private void Btn_Init()
    {

        // 열린옵션창 버튼
        MainOption_Btn[0].onClick.AddListener(() => {
            SoundManager.inst.F_Get_ControllSoundPreFabs_ETC_PlaySFX(3, 1);

            if (Window_0.gameObject.activeSelf == true)
            {
                Window_0.gameObject.SetActive(false);
                GameManager.Inst.F_TimeSclaeController(false);
            }
        });

        MainOption_Btn[1].onClick.AddListener(() =>
        {
            SoundManager.inst.F_Get_ControllSoundPreFabs_ETC_PlaySFX(3, 1);
            OptionWindow.gameObject.SetActive(false);
            SoundOption.gameObject.SetActive(true);

        });

        BackBtn.onClick.AddListener(() => {
            SoundManager.inst.F_Get_ControllSoundPreFabs_ETC_PlaySFX(3, 1);
            OptionWindow.gameObject.SetActive(true);
            SoundOption.gameObject.SetActive(false);
        });

        MainOption_Btn[2].onClick.AddListener(() => {
#if UNITY_EDITOR
            if (Application.isEditor)
            {
                EditorApplication.ExitPlaymode();
            }

            else 
            {
                Application.Quit();
            }
#endif
            Application.Quit();

        });
    }
}
