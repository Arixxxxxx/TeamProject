using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class OpeningManager : MonoBehaviour
{

    Animator cuttonAnim;
    GameObject[] actionScene = new GameObject[2];
    [SerializeField] GameObject actionSceneBase;
    GameObject worldCanvas;
    Button[] mainBtn = new Button[2];

    GameObject loginTitle;
    GameObject signUpObj;

    Animator pressAnykeyAnim;
    [SerializeField] TMP_InputField emilInput;
    [SerializeField] TMP_InputField PWInput;
    [SerializeField] TMP_InputField signEmailInput;
    [SerializeField] TMP_InputField signPwInput;

    WaitForSeconds[] waitSec05 = new WaitForSeconds[5]; // 배열당 0.5초씩 증가
    float InitSec = 0.5f;

    bool thisSceneEnd;
    public bool ThisSceneEnd { get { return thisSceneEnd; } set { thisSceneEnd = value; } }
    private void Awake()
    {
        cuttonAnim = GameObject.Find("UI_Canvas/Cutton").GetComponent<Animator>();

        worldCanvas = GameObject.Find("WorldCanvas").gameObject;
        actionScene[0] = GameObject.Find("Action0").gameObject;
        actionScene[1] = worldCanvas.transform.Find("Action1_Main").gameObject;
        mainBtn[0] = actionScene[1].transform.Find("StartBtn/Button").GetComponent<Button>();
        mainBtn[1] = actionScene[1].transform.Find("ExitBtn/Button").GetComponent<Button>();
        loginTitle = actionScene[1].transform.Find("LoginPanner").gameObject;
        signUpObj = worldCanvas.transform.Find("Action1_Main/SignInPanner").gameObject;

        mainBtn[0].onClick.AddListener(() => { StartCoroutine(NextScene()); });
        mainBtn[1].onClick.AddListener(() => { Application.Quit(); });

        for (int i = 0; i < waitSec05.Length; i++)
        {
            InitSec += 0.5f;
            waitSec05[i] = new WaitForSeconds(InitSec);
        }
        pressAnykeyAnim = actionScene[1].transform.Find("PreeAnyKey").GetComponent<Animator>();

    }

    private void Start()
    {
        StartCoroutine(Action());

    }

    private void Update()
    {
        if (doPreeAnyKey == true && Input.anyKey)
        {
            doPreeAnyKey = false;
            pressAnykeyAnim.SetTrigger("Off");
        }

        NextArrowField();
    }
    bool doPreeAnyKey;
    IEnumerator Action()
    {
        yield return waitSec05[1];
        SoundManager.inst.F_Bgm_Player(0, 1f, 1);
        yield return waitSec05[1];
        cuttonAnim.SetTrigger("Off");

        yield return waitSec05[4];
        yield return waitSec05[2];

        cuttonAnim.SetTrigger("On"); // 페이드가려줌
        SoundManager.inst.F_Bgm_Player(1, 0.6f, 0.8f);
        yield return waitSec05[3];

        actionScene[0].gameObject.SetActive(false); // 화면 바꿔줌
        actionScene[1].gameObject.SetActive(true);
        actionSceneBase.gameObject.SetActive(true);

        doPreeAnyKey = true;

        yield return null;
        cuttonAnim.SetFloat("OffSpeed", 0.8f);
        cuttonAnim.SetTrigger("Off"); // 커튼 내림

        while (doPreeAnyKey == true)
        {
            yield return null;
        }

        yield return waitSec05[0];

        //
        loginTitle.gameObject.SetActive(true);

        emilInput.Select();
        emilInput.ActivateInputField();

        // 로그인 이후로 변경

    }

    public void F_GameStartBtnActive()
    {
        mainBtn[0].transform.parent.gameObject.SetActive(true);
        mainBtn[1].transform.parent.gameObject.SetActive(true);
    }

    IEnumerator NextScene()
    {
        if (thisSceneEnd == false)
        {
            thisSceneEnd = true;
            SoundManager.inst.F_Get_SoundPreFabs_PlaySFX(1, 1);
            cuttonAnim.SetFloat("OnSpeed", 0.7f);
            cuttonAnim.SetTrigger("On"); // 페이드가려줌
            SoundManager.inst.F_BgmEnd();
            yield return waitSec05[4];
            yield return waitSec05[4];
            SceneManager.LoadScene(1);
        }
    }

    private void NextArrowField()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(EventSystem.current.currentSelectedGameObject != null)
            {
                switch (EventSystem.current.currentSelectedGameObject.name)
                {
                    case "InputEmail":
                        TabToNext(PWInput);

                        break;

                    case "InputPW":
                        TabToNext(emilInput);
                        break;

                    case "InputField_Email":
                        TabToNext(signPwInput);
                        break;


                    case "InputField_Passward":
                        TabToNext(signEmailInput);
                        break;
                }
            }
           else
            {
                if (loginTitle.activeSelf)
                {
                    TabToNext(emilInput);
                }
                else if (signUpObj.activeSelf)
                {
                    TabToNext(signEmailInput);
                }
            }
        }
    }


    private void TabToNext(TMP_InputField nextInputField)
    {
        nextInputField.Select();
        nextInputField.ActivateInputField();
    }
}
