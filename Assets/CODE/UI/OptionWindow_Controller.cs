using System.Collections;
using System.Collections.Generic;
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



    void Start()
    {
        // �⺻ UI ref ������ �ʱ�ȭ
        MainCanvas = Ui_Parent_Obj.transform.Find("Main_Canvas").gameObject;
        SeletWindowList = Ui_Parent_Obj.transform.Find("IngameSelectWindow").gameObject;

        //���ν�ũ�� ��ư �ʱ�ȭ
        OptionOpenBtn = MainCanvas.transform.Find("Option_OpenBtn").GetComponent<Button>(); 

        // 1��â (�ΰ��� �ܹ��Ÿ�� ������ ������ �ɼ�â)
        Window_0 = SeletWindowList.transform.GetChild(0).gameObject;
        MainWindow_0_Btn_0 = Window_0.transform.Find("PlayBtn").GetComponent<Button>();
        MainWindow_0_Btn_1 = Window_0.transform.Find("ExitBtn").GetComponent<Button>();

        Btn_Init();
    }

    // Update is called once per frame
    void Update()
    {
        
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

            if (Application.isEditor)
            {
                EditorApplication.ExitPlaymode();
            }
            else 
            {
                Application.Quit();
            }

        });
    }
}
