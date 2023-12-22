using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class OpeningManager : MonoBehaviour
{
    public static OpeningManager inst;

    [SerializeField] GameObject Ui_Canvas;
    [Header("Action_1 = Eblem FadeOn")]
    [Space]
    [SerializeField] float Action_01_Delay;
    [Header("Action_2 = Eblem Fade Off")]
    [Space]
    [SerializeField] float Action_02_Delay;
    [Header("Action_3 = BlackGround Fade On")]
    [Space]
    [SerializeField] float Dealy;
    [Header("Main_Action_1 = Logo_On")]
    [Space]
    [SerializeField] float Main_Action_0_Delay;
    [Header("Action_2 = PressAnyKey Popup")]
    [Space]
    [SerializeField] float Main_Action_1_Delay;
    [Header("Action_3 = Btn Fade On")]
    [Space]
    [SerializeField] float Main_Action_2_Delay;


    Animator emblem_Anim;
    Animator main_UI_anim;

    WaitForSeconds Action_01;
    WaitForSeconds Action_02;
    WaitForSeconds Action_03;

    WaitForSeconds Main_UI_Action_0_Dealy;
    WaitForSeconds Main_UI_Action_1_Dealy;
    WaitForSeconds Main_UI_Action_2_Dealy;

    GameObject PressAnyKeyObj;
    Image MainLogo;
    [SerializeField] Button StartBtn;
    [SerializeField] Button ExitBtn;

    [SerializeField] bool Action_01_End;
    [SerializeField] bool Action_02_End;

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
        Action_01 = new WaitForSeconds(Action_01_Delay);
        Action_02 = new WaitForSeconds(Action_02_Delay);
        Action_03 = new WaitForSeconds(Dealy);

        Main_UI_Action_0_Dealy = new WaitForSeconds(Main_Action_0_Delay);
        Main_UI_Action_1_Dealy = new WaitForSeconds(Main_Action_1_Delay);
        Main_UI_Action_2_Dealy = new WaitForSeconds(Main_Action_2_Delay);



        emblem_Anim = Ui_Canvas.transform.Find("Open_Cutton").GetComponent<Animator>();
        main_UI_anim = Ui_Canvas.GetComponent<Animator>();
        MainLogo = Ui_Canvas.transform.Find("Main_UI/Sample_2/Logo").GetComponent <Image>();
        PressAnyKeyObj = Ui_Canvas.transform.Find("Main_UI/Sample_2/PressAnyKey").gameObject;
        StartBtn = Ui_Canvas.transform.Find("Main_UI/Start_Btn").GetComponent<Button>();
        StartBtn.onClick.AddListener(() => { main_UI_anim.SetTrigger("End");  });
        ExitBtn = Ui_Canvas.transform.Find("Main_UI/Exit_Btn").GetComponent<Button>();
        ExitBtn.onClick.AddListener(()=> 
        {
            if (Application.isEditor)
            {
                EditorApplication.ExitPlaymode();
            }
            else
            {
                Application.Quit();
            }

        });

        StartCoroutine(Opening_Start());

    }


    bool once;
    void Update()
    {
        if (Action_01_End == true && once == false)
        {
            once = true;
            StartCoroutine(Main_UI_Start());
        }

        OffPressAnyKeyAndActiveBtn();
    }


    // 스타트 오프닝 절차적 애니메이션
    IEnumerator Opening_Start()
    {
        yield return Action_01;
        emblem_Anim.SetTrigger("Action_1");

        yield return Dealy;

        while (emblem_Anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1 && emblem_Anim.GetCurrentAnimatorStateInfo(0).IsName("Emblem_Open") == true)
        {
            yield return null;
        }
        yield return Action_02;

         
        emblem_Anim.SetTrigger("Action_2");
    }


    // MainUI 코루틴
    IEnumerator Main_UI_Start()
    {
        yield return Main_UI_Action_0_Dealy;
        MainLogo.gameObject.SetActive(true);
        main_UI_anim.SetTrigger("Action_0");

    }

    private void OffPressAnyKeyAndActiveBtn()
    {
        if (Input.anyKey && Action_02_End == true)
        {
            PressAnyKeyObj.gameObject.SetActive(false);
            StartBtn.gameObject.SetActive(true);
            ExitBtn.gameObject.SetActive(true);
            main_UI_anim.SetTrigger("Action_2");
           
        }
        
    }

    public void F_ActionEnd(int value, bool B_value)
    {
        switch (value)
        {
            case 0:
                Action_01_End = B_value;
                break;

            case 1:
                Action_02_End = B_value;
                break;
        }
    }

    public void F_PressObjActiveTrue()
    {
            
            MainLogo.color = Color.white;
            PressAnyKeyObj.gameObject.SetActive(true);
            Action_02_End = true;
    }

}
