using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UnitFrame_Updater : MonoBehaviour
{
    public static UnitFrame_Updater inst;

    [Header("#  Insert Obj In Hiearchy")]
    [Space]
    [SerializeField] GameObject UnitFrame;
    [SerializeField] float HpBar_Down_Speed;
    [SerializeField] float Exp_Circle_Speen_Speed;

    GameObject Hp_Bar;
    Image Middle_HP_Bar, Front_HP_Bar, ExpCicle;
    float CurHP, MaxHp, CurExp, LevelupExp;

    GameManager gm;
    TMP_Text Hp_Bar_Info_Text;
    TMP_Text Lv_text;
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
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInfo_Updater();
        Image_FillAmount_Updater();
    }

    private void PlayerInfo_Updater()
    {
        CurHP = gm.F_Get_PlayerSc().F_GetPlayerInfo(0);
        MaxHp = gm.F_Get_PlayerSc().F_GetPlayerInfo(1);
        Lv_text.text = gm.F_Get_PlayerSc().F_GetPlayerInfo(2).ToString();
        CurExp = gm.F_Get_PlayerSc().F_GetPlayerInfo(3);
        LevelupExp = gm.F_Get_PlayerSc().F_GetPlayerInfo(4);

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
}
