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

    // ��ȸ�� �ʿ��Ѱ͵�
    // �� �ð� , �������� ���� List, 
    // �Լ� : �ѽð� üũ���ִ� �Լ�
    // �������� ���� -> left�� ����

    private void Awake()
    {
        playOne = transform.Find("PatternOne").GetComponentsInChildren<BossSpinLaserAttackCollider>();
        playTwo = transform.Find("PatternTwo").GetComponentsInChildren<BossSpinLaserAttackCollider>();
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        RotationSpinAction();

        if (Input.GetKeyDown(KeyCode.F))
        {
            F_ActionPattern(0);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            F_ActionPattern(1);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
           
        }
    }

    /// <summary>
    /// ���� ��ȸ������
    /// </summary>
    public void F_PatternReverse()
    {
         left = !left;
    }

    bool left;
  
    private void RotationSpinAction()
    {
        if(spinStart == false) { return; }
        
        else
        {

            switch (left)
            {
                case false:
                    transform.Rotate(Vector3.back * Time.deltaTime * Speed);
                    break;

                case true:
                    transform.Rotate(-Vector3.back * Time.deltaTime * Speed);
                    break;

            }
            
        }
    }

    public void F_SetSpeedValue(float Value)
    {
        Speed = Value;
    }

    bool patternActionOk; // 2���� ������ Ȯ���ϴ� ����

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

        switch (value)
        {
            case 1:

                for (int i = 0; i < playTwo.Length; i++)
                {
                    playTwo[i].F_ActionStart();
                }
                patternActionOk = true;
                break;
        }
    }

    public void F_ActionEnd()
    {
        for (int i = 0; i < playOne.Length; i++)
        {
            playOne[i].F_ActionEnd();
        }

        for (int i = 0; i < playTwo.Length; i++)
        {
            playTwo[i].F_ActionEnd();
        }
    }
}
