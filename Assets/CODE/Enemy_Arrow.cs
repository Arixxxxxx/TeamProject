using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Enemy_Arrow : MonoBehaviour
{
    public enum bulletType { Arrow, Slime_Poison}
    public bulletType type;

    [SerializeField] Vector2 Target;
    [SerializeField] float Arrow_Speed;
    Rigidbody2D Rb;
    GameManager gm;
    float RotAngle;
    Animator anim;
    SpriteRenderer sr;
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        gm = GameManager.Inst;
        sr = gameObject.GetComponent<SpriteRenderer>();
        Arrow_Init();
    }

    private void OnEnable()
    {
        if(gm == null)
        {
            gm = GameManager.Inst;
        }
        
        Arrow_Init();
    }
    private void OnBecameInvisible()
    {
        switch (type)
        {
            case bulletType.Arrow:

                Target = Vector2.zero;
                RotAngle = 0;
                transform.rotation = Quaternion.identity;
                PoolManager.Inst.F_ReturnObj(gameObject, 0);

                break;

                case bulletType.Slime_Poison:
                Posion_Stop = false;
                break;
        }
    }
    private void FixedUpdate()
    {
        Target = Target.normalized;

        switch (type)
        {
            case bulletType.Arrow:
                Rb.MovePosition(Rb.position + Target * Arrow_Speed * Time.deltaTime);
                break;

            case bulletType.Slime_Poison:
                {
                   if(Posion_Stop == true) { return; }

                   if (Target.x > 0)
                    {
                        sr.flipX = true;
                    }
                   else if(Target.x < 0)
                    {
                        sr.flipX = false; 
                    }

                    Rb.MovePosition(Rb.position + Target * Arrow_Speed * Time.deltaTime);
                }
            break;

        }
       
       
    }

    private void Arrow_Init()
    {
        switch(type)
        {
            case bulletType.Arrow:
                Target = gm.F_Enemy_BulletTargetPos(transform.position);
                RotAngle = gm.F_EnemyBulletRotation(transform.position);
                transform.rotation = Quaternion.AngleAxis(RotAngle * -1, Vector3.back);
                break;

            case bulletType.Slime_Poison:
                Target = gm.F_Enemy_BulletTargetPos(transform.position);
                anim = GetComponent<Animator>();
                anim.SetTrigger("hit");
                break;
        }
    }
    
    void Update()
    {
        
    }
    bool Posion_Stop;
    public void A_SilmePoison_Posion_Stop()
    {
        Posion_Stop = true;
    }
    public void A_SilmePoison_return()
    {
        GameObject obj = PoolManager.Inst.F_GetObj(4);
        obj.transform.position = transform.position + new Vector3(0.2f, -0.5f);
        obj.gameObject.SetActive(true);
        PoolManager.Inst.F_ReturnObj(gameObject, 3);

    }

   
}
