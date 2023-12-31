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
    [Space]
    [SerializeField] float Player_CurHP;
    [SerializeField] float Player_MaxHP;
    [SerializeField] float LevelUp_Plus_Hp;
    [Header("# Hp Info")]
    [Space]
    [SerializeField] bool player_On_Hit;
    [SerializeField] float noDMG_Time;
    [SerializeField] Color noDMG_Color;
    int Passive_2_Lv;
    float ReceveryDealyTime;
    float ReceveryHp;
    float RecoveryCount;
    SpriteRenderer sr;
    ParticleSystem getItemPs;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        getItemPs = transform.Find("PS/GetEXP").GetComponent<ParticleSystem>(); 
    }
    void Start()
    {
        Origin_MaxHP = Player_MaxHP; ;
    }

    // Update is called once per frame
    void Update()
    {
        Show_NoDMG_Alpah_A_Zero();
        HpRecoveryPassive();
    }

    float Origin_MaxHP;

    // 페시브 체력증가 
    public void F_Set_Add_Hp_Passiveskill(float value)
    {
        Player_MaxHP = Origin_MaxHP * value;
    }

    // 페시브 체력회복시스템

    public void F_HpRecoveryPassive_LvUp(float Time, float Hp)
    {
        Passive_2_Lv++;
        ReceveryDealyTime =  Time;
        ReceveryHp = Hp;
    }

    public void HpRecoveryPassive()
    {
        if (Passive_2_Lv == 0) { return; }

        RecoveryCount += Time.deltaTime;

        if (RecoveryCount > ReceveryDealyTime)
        {
            RecoveryCount = 0;
            Player_CurHP += ReceveryHp;
        }
    }


  
 
    public void F_GetExp_LevelupSystem(float EXP)
    {
        if(Player_Cur_Lv ==  Player_Max_Lv) { return; }
        Debug.Log("진입");
        float CashEXP = EXP + Cur_Exp;
        getItemPs.Play();
        // 현재 레벨에 필요한 경험치보다 많이 획득했을 때
        if (CashEXP >= NextLv_Need_Exp[Player_Cur_Lv - 1])
        {
            // 레벨업 처리
            while (CashEXP >= NextLv_Need_Exp[Player_Cur_Lv - 1])
            {
                CashEXP -= NextLv_Need_Exp[Player_Cur_Lv - 1];
                Player_Cur_Lv++;
                Player_MaxHP += LevelUp_Plus_Hp;
                Origin_MaxHP = Player_MaxHP;
            }

            // 남은 경험치를 현재 경험치로 설정
            Cur_Exp = CashEXP;
            UnitFrame_Updater.inst.F_ExpFillAmountReset();

            Player_CurHP = Player_MaxHP; // 현재 체력 초기화
            LvUp_Ui_Manager.Inst.F_LvUP_SelectSkill(); // 레벨업 스킬포인트
        }
        else
        {
            // 레벨업이 아닌 경우 현재 경험치 갱신
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
            UnitFrame_Updater.inst.F_Set_Unitframe_DMG(DMG);
            Invoke("Player_OnHit_False", noDMG_Time);

            if (Player_CurHP <= 0)
            {
                GameManager.Inst.IsPlayer_Dead = true;
                //죽음 애니메이션

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
    /// 1 현재 체력/ 2 총 체력 // 레벨 // 4 경험치 // 현재 레벨 경험치
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
     
    public int F_Get_Player_LV()
    {
        return Player_Cur_Lv;
    }
 
}
