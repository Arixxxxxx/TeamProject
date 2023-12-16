using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Nav_Movement : MonoBehaviour
{
    [SerializeField] bool TestClickMoveEnemy;
    [SerializeField] bool isEnemyDead;
    [SerializeField] float Dis;
    [SerializeField] float AttackDis;
    [SerializeField] float FilpxFloat;
    Camera cam;
    Vector2 navTargetVec;
    NavMeshAgent nav;
    SpriteRenderer sr;
    Enemy_Attack attackSC;
    GameManager gm;
    Animator anim;

     void Start()
    {
        anim  = GetComponent<Animator>();
        attackSC = GetComponent<Enemy_Attack>();
        sr = GetComponent<SpriteRenderer>();
        cam = Camera.main;
        nav = GetComponent<NavMeshAgent>();
        nav.updateRotation = false;
        nav.updateUpAxis = false;
        gm = GameManager.Inst;
    }

    // Update is called once per frame
    void Update()
    {
        if(isEnemyDead) 
        { 
            if(nav.isStopped == false)
            {
                nav.isStopped = true;
            }
            return;
        }

        PlayerAndMeDistance();
        Attack();
        Sprite_FilpX_Changer();
        MoveEnemy();


    }


    private void PlayerAndMeDistance()
    {
        FilpxFloat = gm.F_Get_Filpx_Value(transform.position);
        Dis = gm.F_Get_PlyerPos(transform.position);
    }

    private void Attack()
    {
        if(Dis < AttackDis)
        {
            attackSC.Set_Attack_Bool_Changer(true);
        }
    }


    private void Sprite_FilpX_Changer()
    {
      
        if(FilpxFloat > 0 && sr.flipX == true)
        {
            sr.flipX = false;
        }
        else if(FilpxFloat < 0 && sr.flipX == false)
        {
            sr.flipX = true;
        }
    }

    private void MoveEnemy()
    {
        navTargetVec = GameManager.Inst.F_Get_PlayerObj();

        if (anim.GetBool("Attack") == true)
        {
            nav.isStopped = true;
        }
        else if(anim.GetBool("Attack") == false)
        {
            if (nav.isStopped == true)
            {
                nav.isStopped = false;
            }
            
            nav.SetDestination(navTargetVec);
        }
    }


    public void F_Dead(bool value)
    {
        isEnemyDead = value;
        
    }

    
}
