using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
  
    [Header("# PlayerObj Insert stats    =>    (¿¹Áø) ")]
    [Space]
    [SerializeField] float CharMove_Speed;


    Rigidbody2D rb;
    SpriteRenderer sr;
    Vector2 moveVec;
    Vector3 sclaeX_Vec;

    

    private void Awake()
    {
        sclaeX_Vec = new Vector3(-1, 1, 1);
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        
    }
    void Update()
    {
        Input_Key_Funtion();
        Change_Sclae_Xvalue();


        Move_Character();
    }


    private void Input_Key_Funtion()
    {
        moveVec.x = Input.GetAxisRaw("Horizontal") ;
        moveVec.y = Input.GetAxisRaw("Vertical");
    }

    private void Change_Sclae_Xvalue()
    {
        if(moveVec.x < 0)
        {
            //transform.localScale = sclaeX_Vec;
            sr.flipX = true;
        }
        else if(moveVec.x > 0) 
        {
            //transform.localScale = Vector3.one;
            sr.flipX = false;
        }
    }

    private void Move_Character()
    {
        moveVec = moveVec.normalized;
        rb.velocity = moveVec * CharMove_Speed;
    }
}
