using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossSpinLaser : MonoBehaviour
{
    BossSpinLaserAttackCollider[] playOne;
    BossSpinLaserAttackCollider[] playTwo;

    bool spinStart;
    public bool SpinStart { set { spinStart = value; } }

    [Header("# Input Value")]
    [Space]
    [SerializeField] float Speed;

    private void Awake()
    {
        playOne = transform.Find("PatternOne").GetComponentsInChildren<BossSpinLaserAttackCollider>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotationSpinAction();

        if (Input.GetKeyDown(KeyCode.F))
        {
            F_ActionPattern(0);
        }
    }

  
    private void RotationSpinAction()
    {
        if(spinStart == false) { return; }
        
        else
        {
            transform.Rotate(Vector3.back * Time.deltaTime * Speed)  ;
        }
    }

    public void F_ActionPattern(int value)
    {
        switch (value) 
        {
            case 0:

                for (int i = 0; i < playOne.Length; i++)
                {
                    playOne[i].F_ActionStart();
                }
                break;
        }
    }
}
