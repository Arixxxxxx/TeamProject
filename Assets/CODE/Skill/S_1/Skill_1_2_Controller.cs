using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_1_2_Controller : MonoBehaviour
{
    Animator anim;
    BoxCollider2D boxColl;

    private void Awake()
    {
        anim = GetComponent<Animator>();    
        boxColl = GetComponent<BoxCollider2D>();
    }


    private void A_BoxColloider_On()
    {
        if(boxColl.enabled == false)
        {
            boxColl.enabled = true; 
        }
    }

    private void A_ReturnObj_And_BoxColl_Off()
    {
        if (boxColl.enabled == true)
        {
            boxColl.enabled = false;
        }
        PoolManager.Inst.F_Return_PlayerBullet(gameObject, 1);
    }
}
