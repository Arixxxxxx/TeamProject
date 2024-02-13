using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using TMPro;
using PimDeWitte.UnityMainThreadDispatcher;
using UnityEngine.EventSystems;
using Firebase.Extensions;


public class LogInSystem : MonoBehaviour
{
    [SerializeField] GameObject LoginPanner;
    [SerializeField] GameObject SignInPanner;

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

    FirebaseAuth auth;
    FirebaseUser user;
    Animator signUpCompleteAnim;

    private void Awake()
    {
        CheakFireBaseVision();

        openingManager = GetComponent<OpeningManager>();
        auth = FirebaseAuth.DefaultInstance;
        if (auth == null)
        {
            Debug.LogError("널");
        }

        signUpBtn = LoginPanner.transform.Find("SignIn/Button").GetComponent<Button>();
        signUpBtnAnim = signUpBtn.transform.parent.GetComponent<Animator>();

        emailField = LoginPanner.transform.Find("InputEmail").GetComponent<TMP_InputField>();

        passwardField = LoginPanner.transform.Find("InputPW").GetComponent<TMP_InputField>();
        loginBtn = LoginPanner.transform.Find("LoginBtn/Button").GetComponent<Button>();
        loginErrorText = LoginPanner.transform.Find("ErrorText").GetComponent<TMP_Text>();


        backBtn = SignInPanner.transform.Find("BackBtn/Button").GetComponent<Button>();
        signinEmailField = SignInPanner.transform.Find("InputField_Email").GetComponent<TMP_InputField>();
        signinPasswardField = SignInPanner.transform.Find("InputField_Passward").GetComponent<TMP_InputField>();
        createBtn = SignInPanner.transform.Find("CreateBtn/Button").GetComponent<Button>();
        sinUpErrorText = SignInPanner.transform.Find("ErrorText").GetComponent<TMP_Text>();
        signUpCompleteAnim = LoginPanner.transform.parent.Find("SignUpComplete").GetComponent<Animator>();

    }
    void Start()
    {
        signUpBtn.onClick.AddListener(() =>
        {
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
            signinEmailField.text = string.Empty;
            signinPasswardField.text = string.Empty;

            LoginPanner.SetActive(true);
            SignInPanner.SetActive(false);
            emailField.Select();
            emailField.ActivateInputField();
        });




        loginBtn.onClick.AddListener(() => { Login(); });
        createBtn.onClick.AddListener(() => { CreateID(); });
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
        if (Input.GetKeyDown(KeyCode.Return))
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

    private void CheakFireBaseVision()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                var app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }
    public void LogOut()
    {
        auth.SignOut();
        Debug.LogError("로그인 완료");
    }
}
