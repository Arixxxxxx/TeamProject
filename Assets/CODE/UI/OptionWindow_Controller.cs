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
     Button MainWindow_0_Btn_0;
     Button MainWindow_0_Btn_1;

    /// Yes or No ��ư 
    GameObject selectWindow;
    TMP_Text optionText;
    Button yesBtn;
    Button noBtn;
    GameObject backLight;
    GameObject OptionWindow;

    void Start()
    {
        // �⺻ UI ref ������ �ʱ�ȭ
        MainCanvas = Ui_Parent_Obj.transform.Find("Main_Canvas").gameObject;
        SeletWindowList = Ui_Parent_Obj.transform.Find("IngameSelectWindow").gameObject;

        //���ν�ũ�� ��ư �ʱ�ȭ
        OptionOpenBtn = MainCanvas.transform.Find("Option_OpenBtn").GetComponent<Button>();
        OptionWindow = SeletWindowList.transform.Find("OptionWindow").gameObject;

        // 1��â (�ΰ��� �ܹ��Ÿ�� ������ ������ �ɼ�â)
        Window_0 = SeletWindowList.transform.GetChild(0).gameObject;
        MainWindow_0_Btn_0 = Window_0.transform.Find("PlayBtn").GetComponent<Button>();
        MainWindow_0_Btn_1 = Window_0.transform.Find("ExitBtn").GetComponent<Button>();

        //����â �ʱ�ȭ
        selectWindow = MainCanvas.transform.Find("SelectBtn").gameObject;
        optionText = selectWindow.transform.Find("Bg/Text").GetComponent<TMP_Text>();
        yesBtn = selectWindow.transform.Find("Bg/Yes").GetComponent <Button>();
        noBtn = selectWindow.transform.Find("Bg/No").GetComponent <Button>();
        backLight = selectWindow.transform.Find("Bg/Light2").gameObject;

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
        //�ΰ��� �Ͻ����� ��ư
        OptionOpenBtn.onClick.AddListener(() => 
        {
            if(Window_0.gameObject.activeSelf == false) 
            {
                Window_0.gameObject.SetActive(true);
            }
        });

        // �����ɼ�â ��ư
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
