using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using TMPro;
using PimDeWitte.UnityMainThreadDispatcher;
using UnityEngine.EventSystems;
using Firebase.Extensions;
using Firebase;


public class LogInSystem : MonoBehaviour
{
    [SerializeField] GameObject LoginPanner;
    [SerializeField] GameObject SignInPanner;

    FirebaseApp app;
    OpeningManager openingManager;
    Button loginBtn;
    Button signUpBtn;

    Animator signUpBtnAnim;

    [SerializeField]
    TMP_InputField emailField;
    [SerializeField]
    TMP_InputField passwardField;

    TMP_Text loginErrorText;
    TMP_Text sinUpErrorText;

    TMP_InputField signinEmailField;
    TMP_InputField signinPasswardField;

    Button createBtn;
    Button backBtn;
    Button exitBtn;

    FirebaseAuth auth;
    FirebaseUser user;
    Animator signUpCompleteAnim;

    private void Awake()
    {
      
       
        openingManager = GetComponent<OpeningManager>();

       

        signUpBtn = LoginPanner.transform.Find("SignIn/Button").GetComponent<Button>();
        signUpBtnAnim = signUpBtn.transform.parent.GetComponent<Animator>();

        emailField = LoginPanner.transform.Find("InputEmail").GetComponent<TMP_InputField>();

        passwardField = LoginPanner.transform.Find("InputPW").GetComponent<TMP_InputField>();
        loginBtn = LoginPanner.transform.Find("LoginBtn/Button").GetComponent<Button>();
        loginErrorText = LoginPanner.transform.Find("ErrorText").GetComponent<TMP_Text>();
        exitBtn = LoginPanner.transform.Find("ExitBtn/Button").GetComponent<Button>();
        exitBtn.onClick.AddListener(() => { Application.Quit(); });

        backBtn = SignInPanner.transform.Find("BackBtn/Button").GetComponent<Button>();
        

        signinEmailField = SignInPanner.transform.Find("InputField_Email").GetComponent<TMP_InputField>();
        signinPasswardField = SignInPanner.transform.Find("InputField_Passward").GetComponent<TMP_InputField>();
        createBtn = SignInPanner.transform.Find("CreateBtn/Button").GetComponent<Button>();
        sinUpErrorText = SignInPanner.transform.Find("ErrorText").GetComponent<TMP_Text>();
        signUpCompleteAnim = LoginPanner.transform.parent.Find("SignUpComplete").GetComponent<Animator>();

    }
    void Start()
    {
        FirebaseInit();

        signUpBtn.onClick.AddListener(() =>
        {
            SoundManager.inst.F_Get_SoundPreFabs_PlaySFX(3, 1);
            emailField.text = string.Empty;
            passwardField.text = string.Empty;
            signUpBtnAnim.SetTrigger("Off");
            LoginPanner.SetActive(false);
            SignInPanner.SetActive(true);
            signinEmailField.Select();
            signinEmailField.ActivateInputField();
        });


        backBtn.onClick.AddListener(() =>
        {
            SoundManager.inst.F_Get_SoundPreFabs_PlaySFX(3, 1);
            signinEmailField.text = string.Empty;
            signinPasswardField.text = string.Empty;

            LoginPanner.SetActive(true);
            SignInPanner.SetActive(false);
            emailField.Select();
            emailField.ActivateInputField();
        });




        loginBtn.onClick.AddListener(() => { SoundManager.inst.F_Get_SoundPreFabs_PlaySFX(3, 1); Login(); });
        createBtn.onClick.AddListener(() => { SoundManager.inst.F_Get_SoundPreFabs_PlaySFX(3, 1); CreateID(); });
    }

    private void Update()
    {
        InputFieldEnterKey();
    }

    public void CreateID()
    {
        auth.CreateUserWithEmailAndPasswordAsync(signinEmailField.text, signinPasswardField.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    SignFair();
                });
                return;
            }
            if (task.IsFaulted)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    SignFair();
                });
                return;
            }

            AuthResult newUser = task.Result;

            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                DataManager.inst.F_NewUserSet(signinEmailField.text);
                SignUpCompletes();
            });
            //회원가입완료 
        });
    }

    public void Login()
    {
        auth.SignInWithEmailAndPasswordAsync(emailField.text, passwardField.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    LoginFair();
                });
                return;


            }
            if (task.IsFaulted)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    LoginFair();
                    Debug.LogError("Login failed: " + task.Exception.ToString());
                });

                return;
            }




            AuthResult newUser = task.Result;

            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                DataManager.inst.F_UserLoginAndPoolServerData(emailField.text); // 서버에서 데이터 가져옴

                LoginPanner.SetActive(false);

                if (loginErrorText.gameObject.activeSelf)
                {
                    loginErrorText.text = string.Empty;
                    loginErrorText.gameObject.SetActive(false);
                }
                openingManager.F_GameStartBtnActive();

            });

        });

    }

    private void LoginFair()
    {
        StartCoroutine(TextHide());
    }

    IEnumerator TextHide()
    {
        loginErrorText.gameObject.SetActive(true);
        loginErrorText.text = "로그인에 실패하였습니다.\n이메일과 비밀번호를 확인해주세요.";
        yield return new WaitForSeconds(3);
        loginErrorText.gameObject.SetActive(false);
    }

    private void SignFair()
    {
        StartCoroutine(TextHides());
    }

    IEnumerator TextHides()
    {
        sinUpErrorText.gameObject.SetActive(true);
        sinUpErrorText.text = "회원가입에 실패하였습니다. 이메일과 비밀번호를 확인해주세요.";
        yield return new WaitForSeconds(3);
        sinUpErrorText.gameObject.SetActive(false);
    }


    private void SignUpCompletes()
    {
        StartCoroutine(SignupAction());
    }

    IEnumerator SignupAction()
    {
        signinEmailField.text = string.Empty;
        signinPasswardField.text = string.Empty;

        SignInPanner.gameObject.SetActive(false);
        LoginPanner.gameObject.SetActive(true);
        signUpCompleteAnim.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        signUpCompleteAnim.SetTrigger("Close");
        yield return new WaitForSeconds(1.5f);
        signUpCompleteAnim.gameObject.SetActive(false);
    }

    private void InputFieldEnterKey()
    {
        if (Input.GetKeyDown(KeyCode.Return) && EventSystem.current.currentSelectedGameObject != null)
        {
            switch (EventSystem.current.currentSelectedGameObject.name)
            {
                case "InputPW":
                    Login();
                    break;

                case "InputField_Passward":
                    CreateID();
                    break;
            }

        }
    }

    private void FirebaseInit()
    {
        //FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        //{
        //    app = FirebaseApp.DefaultInstance;

        //});

        auth = FirebaseAuth.DefaultInstance;
    }

  
    public void LogOut()
    {
        auth.SignOut();
        Debug.LogError("로그인 완료");
    }
}
