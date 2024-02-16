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

    //�ΰ��� ��ũ��UI ��ư��
     Button OptionOpenBtn;

    // �ɼ�â ��ư��
     Button[] MainOption_Btn = new Button[3];
     

    /// Yes or No ��ư 
    GameObject selectWindow;
    TMP_Text optionText;
    Button yesBtn;
    Button noBtn;
    GameObject backLight;
    GameObject OptionWindow;
    GameObject SoundOption;

    //�ɼ� ���� ����
    Slider[] OptionSlider = new Slider[3];
    Button BackBtn;

    GameObject AchiveWindow; // ����â
    GameObject BattleWindow; // ���â
    
    void Start()
    {
        // �⺻ UI ref ������ �ʱ�ȭ
        MainCanvas = Ui_Parent_Obj.transform.Find("Main_Canvas").gameObject;
        SeletWindowList = Ui_Parent_Obj.transform.Find("IngameSelectWindow").gameObject;

        //���ν�ũ�� ��ư �ʱ�ȭ
        OptionWindow = SeletWindowList.transform.Find("OptionWindow").gameObject;
        SoundOption = SeletWindowList.transform.Find("SoundOption").gameObject;
        OptionSlider = SoundOption.GetComponentsInChildren<Slider>();
        BackBtn = SoundOption.transform.Find("BackBtn").GetComponent<Button>();

        OptionSlider[0].onValueChanged.AddListener(AudioMixer_Controller.inst.F_SetMasterVolume);
        OptionSlider[1].onValueChanged.AddListener(AudioMixer_Controller.inst.F_SetMusicVolume);
        OptionSlider[2].onValueChanged.AddListener(AudioMixer_Controller.inst.F_SetSFXVolume);


        // 1��â (�ΰ��� �ܹ��Ÿ�� ������ ������ �ɼ�â)
        Window_0 = SeletWindowList.transform.GetChild(0).gameObject;
        MainOption_Btn = Window_0.transform.GetComponentsInChildren<Button>();
        

        //����â �ʱ�ȭ
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
    /// ����â �ʱ�ȭ �Լ�
    /// </summary>
    /// <param name="value">0 ������ ���� / </param>
    public void F_SetSelectWindowUI_Open(int value)
    {
        yesBtn.onClick.RemoveAllListeners();
        noBtn.onClick.RemoveAllListeners();
        optionText.text = string.Empty;


        switch (value)
        {
            case 0:
                optionText.text = "����� (����) ���� �̵��Ͻðڽ��ϱ�?";

                yesBtn.onClick.AddListener(() =>  //Yes ��ư
                {
                    backLight.SetActive(false);
                    GameManager.Inst.TelePort(1);
                    // ����

                    //â�ݱ�
                    selectWindow.gameObject.SetActive(false);
                });

                noBtn.onClick.AddListener(() =>  // No ��ư
                {
                    backLight.SetActive(false);
                    selectWindow.gameObject.SetActive(false);
                    GameManager.Inst.MoveStop = false;
                  //â�ݱ�

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

        // �����ɼ�â ��ư
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
