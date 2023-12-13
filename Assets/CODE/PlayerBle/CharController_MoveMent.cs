using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController_MoveMent : MonoBehaviour
{
    CharacterController cCon;
    Vector2 moveVec;
    [Header("# Player Move Stats Info  =>  [ ¿¹Áø ] ")]
    [SerializeField] float moveSpeed;

     private void Awake()
    {
        cCon = GetComponent<CharacterController>();
    }


    void Update()
    {
        Input_Funtion();
        MoveChar();

    }

    private void Input_Funtion()
    {
        moveVec.x = Input.GetAxisRaw("Horizontal");
        moveVec.y = Input.GetAxisRaw("Vertical");

        if(moveVec.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if(moveVec.x > 0) 
        {
            transform.localScale = Vector3.one;
        }
        
    }

    private void MoveChar()
    {
        cCon.Move(moveVec * moveSpeed * Time.deltaTime);
    }
    
    
}
