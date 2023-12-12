using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
  
    [Header("# PlayerObj Insert stats    =>    (¿¹Áø) ")]
    [Space]
    [SerializeField] float CharMove_Speed;


    Rigidbody2D rb;
    Vector2 moveVec;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        
    }
        
    void Update()
    {
        Input_Key_Funtion();

        Move_Character();
    }


    private void Input_Key_Funtion()
    {
        moveVec.x = Input.GetAxisRaw("Horizontal");
        moveVec.y = Input.GetAxisRaw("Vertical");
    }

    private void Move_Character()
    {
        moveVec = moveVec.normalized;
        rb.velocity = moveVec * CharMove_Speed;
    }
}
