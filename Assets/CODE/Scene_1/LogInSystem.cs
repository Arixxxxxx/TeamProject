using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using TMPro;
using PimDeWitte.UnityMainThreadDispatcher;



public class LogInSystem : MonoBehaviour
{
    [SerializeField] GameObject LoginPanner;
    [SerializeField] GameObject SignInPanner;

    OpeningManager openingManager;
     Button loginBtn;
     Button signInBtn;
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
        openingManager = GetComponent<OpeningManager>();
        auth = FirebaseAuth.DefaultInstance;
        if(auth == null)
        {
            Debug.LogError("널");
        }

        signInBtn = LoginPanner.transform.Find("SignIn/Button").GetComponent<Button>();
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
        signInBtn.onClick.AddListener(() => { LoginPanner.SetActive(false); SignInPanner.SetActive(true); });
        backBtn.onClick.AddListener(() => { LoginPanner.SetActive(true); SignInPanner.SetActive(false); });

       

        loginBtn.onClick.AddListener(() => { Login(); });
        createBtn.onClick.AddListener(() => { CreateID(); });
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
        SignInPanner.gameObject.SetActive(false);
        LoginPanner.gameObject.SetActive(true);
        signUpCompleteAnim.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        signUpCompleteAnim.SetTrigger("Close");
        yield return new WaitForSeconds(1.5f);
        signUpCompleteAnim.gameObject.SetActive(false);
    }
    public void LogOut()
    {
        auth.SignOut();
        Debug.LogError("로그인 완료");
    }
}
