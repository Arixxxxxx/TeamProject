using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enter_Trigger : MonoBehaviour
{
    [Header("# Cheak Enter Object")]
    [Space]
    [SerializeField] GameObject Player;
    [Header("# Insert Hide Object Animator in Hiearchy")]
    [Space]
    [SerializeField] Animator anim;
    
    
    
    void Start()
    {

    }

    void Update()
    {
        if (Player != null && anim.GetBool("On") == false)
        {
            anim.SetBool("On", true);
            GameManager.Inst.F_Light_On_Off(false);
        }
        else if (Player == null && anim.GetBool("On") == true)
        {
            anim.SetBool("On", false);
            GameManager.Inst.F_Light_On_Off(true);
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Player == null)
        {
            Player = collision.gameObject;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Player != null && collision.transform.position.x > transform.position.x)
        {
            Player = null;
        }
    }
}
