using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("# PlayerObj Insert stats    =>    (예진) ")]
    [Space]
    [SerializeField] float CharMove_Speed;
    [SerializeField] float Sprint_Speed;
    [SerializeField] float Force;


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

    //[SerializeField] float MoveCheak; // 스프린트 작동 시간 제어
    //private void Sprint_Time_Updater()
    //{
    //    if(InputLshift && )
    //}
    private void Move_Character()
    {
        moveVec = moveVec.normalized;

        if (InputLshift)
        {
            rb.MovePosition(rb.position + moveVec * (CharMove_Speed  + Sprint_Speed ) * Time.deltaTime);
        }
        else
        {
            rb.MovePosition(rb.position + moveVec * CharMove_Speed * Time.deltaTime);
        }
    }
}
