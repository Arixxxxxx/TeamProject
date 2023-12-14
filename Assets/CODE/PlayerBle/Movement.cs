using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("# PlayerObj Insert stats    =>    (¿¹Áø) ")]
    [Space]
    [SerializeField] float CharMove_Speed;
    [SerializeField] float Force;


    Rigidbody2D rb;
    SpriteRenderer sr;
    Vector2 moveVec;
    Animator anim;
    bool InputSpaceBar;


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
    private void Move_Character()
    {
        moveVec = moveVec.normalized;
        rb.MovePosition(rb.position + moveVec * CharMove_Speed * Time.deltaTime);
    }
}
