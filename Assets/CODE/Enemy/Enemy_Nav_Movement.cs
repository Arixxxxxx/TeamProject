using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Nav_Movement : MonoBehaviour
{
    Camera cam;
    Vector2 navTargetVec;
    NavMeshAgent nav;
    SpriteRenderer sr;

    [SerializeField] bool TestClickMoveEnemy;
    [SerializeField] bool isEnemyDead;

     void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        cam = Camera.main;
        nav = GetComponent<NavMeshAgent>();
        nav.updateRotation = false;
        nav.updateUpAxis = false;
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


        Sprite_FilpX_Changer();


        if (TestClickMoveEnemy == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                navTargetVec = cam.ScreenToWorldPoint(Input.mousePosition);
                nav.SetDestination(navTargetVec);
            }
        }
        else
        {
            navTargetVec = GameManager.Inst.F_Get_PlayerObj();
            nav.SetDestination(navTargetVec);
        }
    }

    private void Sprite_FilpX_Changer()
    {
        if(nav.velocity.x < 0)
        {
            sr.flipX = false;
        }
        else if(nav.velocity.x > 0)
        {
            sr.flipX = true;
        }
    }
    public void F_Dead(bool value)
    {
        isEnemyDead = value;
    }
}
