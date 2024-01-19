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
    int Passive_2_Lv;
    float ReceveryDealyTime;
    float ReceveryHp;
    float RecoveryCount;
    SpriteRenderer sr;
    ParticleSystem getItemPs;
    ParticleSystem useHpPoition;
    
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        getItemPs = transform.Find("PS/GetEXP").GetComponent<ParticleSystem>();
        useHpPoition = transform.Find("PS/HP_Potion_Particle").GetComponent<ParticleSystem>();
    }
    void Start()
    {
        Origin_MaxHP = Player_MaxHP;
        
    }

    // Update is called once per frame
    void Update()
    {
        Show_NoDMG_Alpah_A_Zero();
        HpRecoveryPassive();
    }

    float Origin_MaxHP;

    // ��ú� ü������ 
    public void F_Set_Add_Hp_Passiveskill(float value)
    {
        Player_MaxHP = Origin_MaxHP * value;
    }

    // ��ú� ü��ȸ���ý���

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
        float CashEXP = EXP + Cur_Exp;
        getItemPs.Play();
        // ���� ������ �ʿ��� ����ġ���� ���� ȹ������ ��
        if (CashEXP >= NextLv_Need_Exp[Player_Cur_Lv - 1])
        {
            // ������ ó��
            while (CashEXP >= NextLv_Need_Exp[Player_Cur_Lv - 1])
            {
                CashEXP -= NextLv_Need_Exp[Player_Cur_Lv - 1];
                Player_Cur_Lv++;
                Player_MaxHP += LevelUp_Plus_Hp;
                Origin_MaxHP = Player_MaxHP;
            }

            // ���� ����ġ�� ���� ����ġ�� ����
            Cur_Exp = CashEXP;
            UnitFrame_Updater.inst.F_ExpFillAmountReset();

            Player_CurHP = Player_MaxHP; // ���� ü�� �ʱ�ȭ
            LvUp_Ui_Manager.Inst.F_LvUP_SelectSkill(); // ������ ��ų����Ʈ
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
            UnitFrame_Updater.inst.F_Set_Unitframe_DMG(DMG,0);
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
     
    /// <summary>
    /// HP ȸ������ ��� �Լ�
    /// </summary>
    /// <param name="value"> ȸ���ϰ��� �ϴ� �����% �Է�</param>
    public void F_Use_HP_Potion(float value)
    {
        float mathfPercentValue = value * 0.01f; // 30���� �������� 0.3ó�� �Ҽ��� ������ ���� ��ȯ 
        float HPRecovery = Player_MaxHP * mathfPercentValue; // ȸ�������� ȸ�������� �� ���� ����
        float CanRecoveryHP = Player_MaxHP - Player_CurHP; // ���� ȸ�������� �絵 üũ

        if(HPRecovery > CanRecoveryHP) // ȸ���ؾ��� �纸�� ���Ǿ��� �� ũ�ٸ�
        {
            Player_CurHP = Player_MaxHP; // Ǯ�Ǹ������
            UnitFrame_Updater.inst.F_Set_Unitframe_DMG(CanRecoveryHP, 1);
        }
        else if (HPRecovery < CanRecoveryHP) // �װ� �ƴ϶�� 
        {
            Player_CurHP += HPRecovery; // ȸ�� ���Ǹ�ŭ��
            UnitFrame_Updater.inst.F_Set_Unitframe_DMG(HPRecovery, 1);
        }

        useHpPoition.gameObject.SetActive(true);
        
        // ��ƼŬ ���
    }
    public int F_Get_Player_LV()
    {
        return Player_Cur_Lv;
    }
 
}
