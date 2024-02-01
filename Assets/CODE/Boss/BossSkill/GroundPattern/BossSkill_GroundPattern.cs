using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossSkill_GroundPattern : MonoBehaviour
{
    Player_Stats playerSc;
    SpriteRenderer sr;
    PolygonCollider2D collider;
    Animator anim;
    Ground_LightningEffect[] Ps;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        collider = GetComponent<PolygonCollider2D>();
        collider.enabled = false;
        anim = GetComponent<Animator>();
        Ps = transform.GetComponentsInChildren<Ground_LightningEffect>();
    }


    private void OnEnable()
    {
        


    }
    void Start()
    {
        playerSc = GameManager.Inst.F_Get_PlayerSc();
    }

    
    void Update()
    {
    
      
    }

    public void F_StartAction()
    {
        anim.SetTrigger("Attack");
    }

    public void A_AttackColliderOn()
    {
        StartCoroutine(ColliderOn());
        
    }

    IEnumerator ColliderOn()
    {
        for(int i = 0; i < Ps.Length; i++)
        {
            Ps[i].F_PlayEffect();
        }
        sr.color -= new Color(0, 0, 0, 1); 
        collider.enabled = true;
        yield return new WaitForSeconds(0.3f);
        collider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") &&  collision.name != "Dragon") 
        {
            playerSc.F_Player_On_Hit(1);
        }
    }
}
