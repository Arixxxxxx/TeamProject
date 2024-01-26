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
     Button MainWindow_0_Btn_0;
     Button MainWindow_0_Btn_1;

    /// Yes or No 버튼 
    GameObject selectWindow;
    TMP_Text optionText;
    Button yesBtn;
    Button noBtn;
    GameObject backLight;
    GameObject OptionWindow;

    void Start()
    {
        // 기본 UI ref 참조용 초기화
        MainCanvas = Ui_Parent_Obj.transform.Find("Main_Canvas").gameObject;
        SeletWindowList = Ui_Parent_Obj.transform.Find("IngameSelectWindow").gameObject;

        //메인스크린 버튼 초기화
        OptionOpenBtn = MainCanvas.transform.Find("Option_OpenBtn").GetComponent<Button>();
        OptionWindow = SeletWindowList.transform.Find("OptionWindow").gameObject;

        // 1번창 (인게임 햄버거모양 누르면 나오는 옵션창)
        Window_0 = SeletWindowList.transform.GetChild(0).gameObject;
        MainWindow_0_Btn_0 = Window_0.transform.Find("PlayBtn").GetComponent<Button>();
        MainWindow_0_Btn_1 = Window_0.transform.Find("ExitBtn").GetComponent<Button>();

        //선택창 초기화
        selectWindow = MainCanvas.transform.Find("SelectBtn").gameObject;
        optionText = selectWindow.transform.Find("Bg/Text").GetComponent<TMP_Text>();
        yesBtn = selectWindow.transform.Find("Bg/Yes").GetComponent <Button>();
        noBtn = selectWindow.transform.Find("Bg/No").GetComponent <Button>();
        backLight = selectWindow.transform.Find("Bg/Light2").gameObject;

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (OptionWindow.gameObject.activeSelf == false)
            {
                OptionWindow.gameObject.SetActive(true);
            }
            else
            {
                OptionWindow.gameObject.SetActive(false);
            }
        }
    }
    private void Btn_Init()
    {
        //인게임 일시정지 버튼
        OptionOpenBtn.onClick.AddListener(() => 
        {
            if(Window_0.gameObject.activeSelf == false) 
            {
                Window_0.gameObject.SetActive(true);
            }
        });

        // 열린옵션창 버튼
        MainWindow_0_Btn_0.onClick.AddListener(() => {

            if (Window_0.gameObject.activeSelf == true)
            {
                Window_0.gameObject.SetActive(false);
            }
        });

        MainWindow_0_Btn_1.onClick.AddListener(() => {
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
