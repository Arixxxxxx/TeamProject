using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stats : MonoBehaviour
{
    [Header("# Player Input Stats Field  ==>   예진")]
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
            float MinusEXP = NextLv_Need_Exp[Player_Cur_Lv] - CashEXP; //레벨 필요경험치 - 합산경험치 = 렙업에 필요한 잔여경험치
            Cur_Exp += MinusEXP; // 잔여경험치 주고 레벨업만 시켜줌
            Player_Cur_Lv++; //레벨업
            Player_MaxHP += LevelUp_Plus_Hp; //HP 최대치 증가
            Player_CurHP = Player_MaxHP; // 현재체력 초기화
            Cur_Exp = CashEXP - MinusEXP; // 나머지 다시채워줌
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
                //죽음 애니메이션

            }
        }
    }
}
