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
    [Space]
    [SerializeField] float Player_CurHP;
    [SerializeField] float Player_MaxHP;
    [SerializeField] float LevelUp_Plus_Hp;
    [Header("# Hp Info")]
    [Space]
    [SerializeField] bool player_On_Hit;
    [SerializeField] float noDMG_Time;
    [SerializeField] Color noDMG_Color;

    SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Show_NoDMG_Alpah_A_Zero();
    }

    

    public void F_GetExp_LevelupSystem(float EXP)
    {
        float CashEXP = EXP + Cur_Exp;

        // ���� ������ �ʿ��� ����ġ���� ���� ȹ������ ��
        if (CashEXP >= NextLv_Need_Exp[Player_Cur_Lv - 1])
        {
            // ������ ó��
            while (CashEXP >= NextLv_Need_Exp[Player_Cur_Lv - 1])
            {
                CashEXP -= NextLv_Need_Exp[Player_Cur_Lv - 1];
                Player_Cur_Lv++;
                Player_MaxHP += LevelUp_Plus_Hp;
            }

            // ���� ����ġ�� ���� ����ġ�� ����
            Cur_Exp = CashEXP;
            UnitFrame_Updater.inst.F_ExpFillAmountReset();

            Player_CurHP = Player_MaxHP; // ���� ü�� �ʱ�ȭ
        }
        else
        {
            // �������� �ƴ� ��� ���� ����ġ ����
            Cur_Exp = CashEXP;
        }

       
    }
    public void F_Player_On_Hit(float DMG)
    {
        if(player_On_Hit == true) { return; }

        if (Player_CurHP > 0)
        {
            player_On_Hit = true;
            Player_CurHP -= DMG;

            Invoke("Player_OnHit_False", noDMG_Time);

            if (Player_CurHP <= 0)
            {
                GameManager.Inst.IsPlayer_Dead = true;
                //���� �ִϸ��̼�

            }
        }
    }

    private void Player_OnHit_False()
    {
        player_On_Hit = false;
    }

    private void Show_NoDMG_Alpah_A_Zero()
    {
        if (player_On_Hit == true && sr.color.a == 1)
        {
            sr.color = noDMG_Color;
        }
        else if (player_On_Hit == false && sr.color.a == noDMG_Color.a)
        {
            sr.color = Color.white;
        }
    }

    float[] OutPut_PlayerInfo = new float[4];
    /// <summary>
    /// 1 ���� ü��/ 2 �� ü�� // ���� // 4 ����ġ // ���� ���� ����ġ
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public float F_GetPlayerInfo(int value)
    {
        OutPut_PlayerInfo[0] = Player_CurHP;
        OutPut_PlayerInfo[1] = Player_MaxHP;
        OutPut_PlayerInfo[2] = Player_Cur_Lv;
        OutPut_PlayerInfo[3] = Cur_Exp;

        if(value == 4)
        {
            return NextLv_Need_Exp[Player_Cur_Lv - 1];
        }

        return OutPut_PlayerInfo[value];
    }

 
}
