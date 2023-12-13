using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stats : MonoBehaviour
{
    [Header("# Player Input Stats Field  ==>   ����")]
    [Space]
    [Header("# Level Up Info")]
    [SerializeField] int Player_Cur_Lv;
    [SerializeField] int Player_Max_Lv;
    [SerializeField] float Cur_Exp;
    [SerializeField] float[] NextLv_Need_Exp;
    
    [Header("# Hp Info")]
    [SerializeField] float Player_CurHP;
    [SerializeField] float Player_MaxHP;
    [SerializeField] float LevelUp_Plus_Hp;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void F_GetExp_LevelupSystem(float EXP)
    {
        float CashEXP = EXP + Cur_Exp;

        if(CashEXP  > NextLv_Need_Exp[Player_Cur_Lv])
        {
            float MinusEXP = NextLv_Need_Exp[Player_Cur_Lv] - CashEXP; //���� �ʿ����ġ - �ջ����ġ = ������ �ʿ��� �ܿ�����ġ
            Cur_Exp += MinusEXP; // �ܿ�����ġ �ְ� �������� ������
            Player_Cur_Lv++; //������
            Player_MaxHP += LevelUp_Plus_Hp; //HP �ִ�ġ ����
            Player_CurHP = Player_MaxHP; // ����ü�� �ʱ�ȭ
            Cur_Exp = CashEXP - MinusEXP; // ������ �ٽ�ä����
        }
        else if(CashEXP < NextLv_Need_Exp[Player_Cur_Lv])
        {
            Cur_Exp = EXP;
        }
    }
    public void F_Player_On_Hit(float DMG)
    {
        if (Player_CurHP > 0)
        {
            Player_CurHP -= DMG;

            if (Player_CurHP <= 0)
            {
                GameManager.Inst.IsPlayer_Dead = true;
                //���� �ִϸ��̼�

            }
        }
    }
}
