using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("# PlayerObj Insert stats    =>    (예진) ")]
    [Space]
    [SerializeField] float CharMove_Speed;
    [SerializeField] float Sprint_Speed;


    Rigidbody2D rb;
    SpriteRenderer sr;
    Vector2 moveVec;
    Animator anim;
    [SerializeField] bool InputSpaceBar;
    [SerializeField] bool InputLshift;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        curSpintTime = maxSpintTime;
    }
    private void FixedUpdate()
    {
        Move_Character();
    }

    void Update()
    {
        Input_Key_Funtion();
        Change_Sclae_Xvalue();
        Animator_Updater();

        Sprint_Time_Updater();
        FrontTeleport();
    }


    private void Input_Key_Funtion()
    {
        moveVec.x = Input.GetAxisRaw("Horizontal");
        moveVec.y = Input.GetAxisRaw("Vertical");
        InputLshift = Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift);
        InputSpaceBar = Input.GetKeyDown(KeyCode.Space);
    }

    private void Change_Sclae_Xvalue()
    {
        if (moveVec.x < 0)
        {
            sr.flipX = true;
        }
        else if (moveVec.x > 0)
        {
            sr.flipX = false;
        }
    }

    float animMoveCeahkFloat;
    private void Animator_Updater()
    {
        float horizontalCheck = Mathf.Abs(moveVec.x);
        float verticalCheck = Mathf.Abs(moveVec.y);
        animMoveCeahkFloat = horizontalCheck + verticalCheck;

        if (animMoveCeahkFloat > 0)
        {
            anim.SetBool("Run", true);
        }
        else if (animMoveCeahkFloat >= 0)
        {
            anim.SetBool("Run", false);
        }
    }

    float MoveCheakX; // 스프린트 작동 시간 제어
    float MoveCheakY; // 스프린트 작동 시간 제어
    float CheakMoveFloat; // 스프린트 작동 시간 제어
    [SerializeField] float curSpintTime;
    [SerializeField] float maxSpintTime;
    private void Sprint_Time_Updater()
    {
        MoveCheakX = Mathf.Abs(moveVec.x);
        MoveCheakY = Mathf.Abs(moveVec.y);

        CheakMoveFloat = MoveCheakX + MoveCheakY;

        if (InputLshift && CheakMoveFloat > 0) // 소모
        {
            if (curSpintTime > 0)
            {
                curSpintTime -= Time.deltaTime;
            }
            else if (curSpintTime <= 0)
            {
                curSpintTime = 0;
            }
        }
        else // 회복
        {
            curSpintTime += Time.deltaTime;

            if (curSpintTime > maxSpintTime)
            {
                curSpintTime = maxSpintTime;
            }
        }
    }

    [SerializeField] bool isRun;
    private void Move_Character()
    {
        moveVec = moveVec.normalized;

        if (InputLshift == true && curSpintTime > 0)
        {
            isRun = true;
        }
        else if (InputLshift == false || curSpintTime <= 0)
        {
            isRun = false;
        }


        if (isRun)
        {
            rb.MovePosition(rb.position + moveVec * (CharMove_Speed + Sprint_Speed) * Time.deltaTime);
        }
        else
        {
            rb.MovePosition(rb.position + moveVec * CharMove_Speed * Time.deltaTime);
        }
    }

    [SerializeField] float TeleportDistance;
    [SerializeField] float TeleportCoolTime;
    [SerializeField] float Count;
    bool doTeleport , once;

    private void FrontTeleport()
    {
        if (InputSpaceBar == true && doTeleport == false && CheakMoveFloat > 0)
        {
            doTeleport = true;
            anim.SetTrigger("Tel");
            rb.position = rb.position + moveVec * TeleportDistance;
        }

        if (doTeleport)
        {
            if (Count != TeleportCoolTime && once == false)
            {
                once = true;
                Count = TeleportCoolTime;
            }

            Count -= Time.deltaTime;

            if (Count <= 0)
            {
                doTeleport = false;
                once = false;

            }

        }
    }
}
