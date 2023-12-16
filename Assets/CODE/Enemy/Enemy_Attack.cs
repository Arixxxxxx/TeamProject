using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Attack : MonoBehaviour
{
    public enum EnemyName { Melee, Range, Mushroom }
    public EnemyName type;
    private Animator anim;
    private GameObject attackCollider;
    [Header("# Attack Speed // 필수 입력  ==>   #예진")]
    [Space]
    [SerializeField] float AttackDMG;
    [SerializeField] float attackSpeed;
    [Space]
    [Space]
    [SerializeField] float mushroomForce;
    [Space]
    [Space]
    [SerializeField] bool isAttack;
    SpriteRenderer sr;


    NavMeshAgent nav;
    Rigidbody2D rb;

    private void Awake()
    {

        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        switch (type)
        {

            case EnemyName.Melee: case EnemyName.Mushroom:
                attackCollider = transform.Find("AttackCollider").gameObject;
                attackCollider.GetComponent<AttackCollider>().F_SetAttackDMG(AttackDMG);
                if (attackCollider.gameObject.activeSelf)
                {
                    attackCollider.SetActive(false);
                }
              break;


            case EnemyName.Range:
                break;
        }


    }

    void Update()
    {
        Attack_Anim();
        anim.SetFloat("AttackSpeed", attackSpeed);


        
    }

    public void Set_Attack_Bool_Changer(bool value)
    {
        isAttack = value;
    }


    private void Attack_Anim()
    {
        if (isAttack == true && anim.GetBool("Attack") == false)
        {
            anim.SetBool("Attack", true);
        }
        else if (isAttack == false)
        {
            anim.SetBool("Attack", false);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run") && attackCollider.activeSelf == true)
        {
            attackCollider.gameObject.SetActive(false);
        }

    }
    bool once;
    private void A_MushroomAttackMoving()
    {
        if(type == EnemyName.Mushroom && anim.GetBool("Attack") == true && once == false)
        {
            once= true;
            Debug.Log("11");
            Vector2 AttackVec = GameManager.Inst.F_Enemy_BulletTargetPos(transform.position);
            AttackVec = AttackVec.normalized;
            rb.AddForce(AttackVec * mushroomForce, ForceMode2D.Impulse);
        }
    }

    private void A_attackCollider_ON()
    {
        switch (type)
        {
            case EnemyName.Melee:

                if (attackCollider.gameObject.activeSelf == false)
                {
                    attackCollider.gameObject.SetActive(true);
                }
                break;

            case EnemyName.Range:


                break;

            case EnemyName.Mushroom:

                if (attackCollider.gameObject.activeSelf == false)
                {
                    attackCollider.gameObject.SetActive(true);
                }
                break;
        }


    }

    private void ArrowSpawn()
    {
        GameObject obj = PoolManager.Inst.F_GetObj(0);
        obj.GetComponent<AttackCollider>().F_SetAttackDMG(AttackDMG);
        switch(sr.flipX)
        {
            case true:
                obj.transform.position = transform.Find("R").transform.position;
                break;
                case false:
                obj.transform.position = transform.Find("L").transform.position;
                break;
        }
        obj.gameObject.SetActive(true);
    }

    private void A_attackCollider_OFF()
    {
        switch (type)
        {
            case EnemyName.Melee:

                if (attackCollider.gameObject.activeSelf == true)
                {
                    attackCollider.gameObject.SetActive(false);
                }
                isAttack = false;
                break;

            case EnemyName.Range:
                isAttack = false;
                anim.SetBool("Attack", false);
                break;

            case EnemyName.Mushroom:
                isAttack = false;
                once = false;
                rb.velocity = Vector2.zero;
                anim.SetBool("Attack", false);
                break;

              
        }
    }
}
