using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitFrame_Updater : MonoBehaviour
{
    [Header("#  Insert Obj In Hiearchy")]
    [Space]
    [SerializeField] GameObject UnitFrame;
    [SerializeField] Image Front_HP_Bar;
    [SerializeField] Image Middle_HP_Bar;
    [SerializeField] float HpBar_Down_Speed;
    [Header("#  Cheak Status")]
    [Space]
    [SerializeField] float CurHP;
    [SerializeField] float MaxHp;


    GameManager gm;
    TMP_Text Hp_Bar_Info_Text;
    TMP_Text Lv_text;
    void Start()
    {
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
    }

    private void Image_FillAmount_Updater()
    {
        Front_HP_Bar.fillAmount = CurHP / MaxHp;
        Hp_Bar_Info_Text.text = $"{CurHP} / {MaxHp}";

        if (Front_HP_Bar.fillAmount < Middle_HP_Bar.fillAmount)
        {
            Middle_HP_Bar.fillAmount -= Time.deltaTime * HpBar_Down_Speed;
        }
        else if (Front_HP_Bar.fillAmount > Middle_HP_Bar.fillAmount)
        {
            Middle_HP_Bar.fillAmount = Front_HP_Bar.fillAmount;
        }

    }
}
