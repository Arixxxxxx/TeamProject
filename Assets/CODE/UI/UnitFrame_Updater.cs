using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UnitFrame_Updater : MonoBehaviour
{
    public static UnitFrame_Updater inst;

    [Header("#  Insert UI In Hiearchy")]
    [SerializeField] GameObject Ui_Canvas;
    [Space]
    [Space]
    [Header("#  UnitFrame Setting")]
    [Space]
    [SerializeField] GameObject UnitFrame;
    [SerializeField] float HpBar_Down_Speed;
    [SerializeField] float Exp_Circle_Speen_Speed;
    TMP_Text battle_Time_Text;
    [SerializeField] float timecheck , min, sec;

    [SerializeField] GameObject bossHpBar_obj;
    [SerializeField] GameObject BattleTime_obj;

    GameObject Hp_Bar;
    Image Middle_HP_Bar, Front_HP_Bar, ExpCicle;
    float CurHP, MaxHp, CurExp, LevelupExp;

    GameManager gm;
    TMP_Text Hp_Bar_Info_Text;
    TMP_Text Lv_text;
    TMP_Text KillCount;
    TMP_Text Unitframe_Dmg_Font;
    Animator Unitframe_Dmg_Anim;
    private void Awake()
    {
        if(inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }

    }

    void Start()
    {
        ExpCicle = UnitFrame.transform.Find("ExP_Circle").GetComponent<Image>();
        Hp_Bar = UnitFrame.transform.Find("HP").gameObject;
        Middle_HP_Bar = Hp_Bar.transform.GetChild(0).GetComponent<Image>();
        Front_HP_Bar = Hp_Bar.transform.GetChild(1).GetComponent<Image>();
        gm = GameManager.Inst;
        
        Hp_Bar_Info_Text = Front_HP_Bar.GetComponentInChildren<TMP_Text>();
        Lv_text = UnitFrame.transform.Find("Lv/LvText").GetComponent<TMP_Text>();

        battle_Time_Text = Ui_Canvas.transform.Find("Main_Canvas/BattleTime").GetComponentInChildren<TMP_Text>();
        KillCount = Ui_Canvas.transform.Find("Main_Canvas/Count_Info/Kill").GetComponentInChildren<TMP_Text>();

        Unitframe_Dmg_Font = UnitFrame.transform.Find("Get_Dmg(Text)").GetComponent<TMP_Text>();
        Unitframe_Dmg_Anim = UnitFrame.GetComponent<Animator>();

        bossHpBar_obj = Ui_Canvas.transform.Find("Main_Canvas/Boos_Hp_Bar").gameObject;
        BattleTime_obj = Ui_Canvas.transform.Find("Main_Canvas/BattleTime").gameObject;
    }


   
    // Update is called once per frame
    void Update()
    {
        PlayerInfo_Updater();
        Image_FillAmount_Updater();
        BattleTime_Updater();
        KillCount_Updater();
        BossHpbarPopup_BattleTime_Hide();
    }

    private void PlayerInfo_Updater()
    {
        CurHP = gm.F_Get_PlayerSc().F_GetPlayerInfo(0);
        MaxHp = gm.F_Get_PlayerSc().F_GetPlayerInfo(1);
        Lv_text.text = gm.F_Get_PlayerSc().F_GetPlayerInfo(2).ToString();
        CurExp = gm.F_Get_PlayerSc().F_GetPlayerInfo(3);
        LevelupExp = gm.F_Get_PlayerSc().F_GetPlayerInfo(4);

    }

    private void BossHpbarPopup_BattleTime_Hide()
    {
        if (bossHpBar_obj.activeSelf == true && BattleTime_obj == true)
        {
            BattleTime_obj.gameObject.SetActive(false);

        }

        if (BattleTime_obj.activeSelf == false && bossHpBar_obj.activeSelf == false)
        {
            BattleTime_obj.gameObject.SetActive(true);
        }
    }

    bool once;
    private void Image_FillAmount_Updater()
    {
        Front_HP_Bar.fillAmount = CurHP / MaxHp;
        Hp_Bar_Info_Text.text = $"{CurHP} / {MaxHp}";

        float CashEXP = CurExp / LevelupExp;

        if(ExpCicle.fillAmount < CashEXP)
        {
            ExpCicle.fillAmount += Exp_Circle_Speen_Speed * Time.deltaTime;
        }
       

        if (Front_HP_Bar.fillAmount < Middle_HP_Bar.fillAmount)
        {
            Middle_HP_Bar.fillAmount -= Time.deltaTime * HpBar_Down_Speed;
        }
        else if (Front_HP_Bar.fillAmount > Middle_HP_Bar.fillAmount)
        {
            Middle_HP_Bar.fillAmount = Front_HP_Bar.fillAmount;
        }
    }

    public void F_ExpFillAmountReset()
    {
        ExpCicle.fillAmount = 0;
    }


    private void KillCount_Updater()
    {
        KillCount.text = GameManager.Inst.KillCount.ToString();
    }
    private void BattleTime_Updater()
    {
        if(gm.MainGameStart == false && timecheck != 0) 
        {
            timecheck = 0;
            battle_Time_Text.text = string.Empty;
            return; 
        }
        
        else if(gm.MainGameStart == true)
        {
            if(GameManager.Inst.UiOpen_EveryObecjtStop == true) { return; }

            timecheck +=Time.deltaTime;
                                    
            min = (int)timecheck / 60;
            sec = (int)timecheck % 60;
                        
            battle_Time_Text.text = $"{min.ToString("00")} : {sec.ToString("00")}";
        }
    }


    public void F_Set_Unitframe_DMG(float DMG)
    {
        Unitframe_Dmg_Font.text = DMG.ToSafeString();
        Unitframe_Dmg_Anim.SetTrigger("hit");
    }
    public float F_Get_GameTime()
    {
        return timecheck;
    }

    public Transform F_Get_UI_Tranfrom()
    {
        return Ui_Canvas.transform;
    }
    
}
