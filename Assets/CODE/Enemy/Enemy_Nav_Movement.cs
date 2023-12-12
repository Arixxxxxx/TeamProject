using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Nav_Movement : MonoBehaviour
{
    Camera cam;
    Vector2 navTargetVec;
    NavMeshAgent nav;
    
    [SerializeField] bool TestClickMoveEnemy;
    void Start()
    {
        cam= Camera.main;
        nav = GetComponent<NavMeshAgent>();
        nav.updateRotation = false;
        nav.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
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
}
