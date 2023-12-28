using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Skill_UI_Icon : MonoBehaviour
{
    Player_Skill_System sc;
    TMP_Text Lv_text;
    [SerializeField] int Lv;
    public enum SkillType
    {
        Active_0, Active_1, Active_2, Active_3, Active_4, Active_5, Passive_0, Passive_1, Passive_2, Passive_3, Passive_4, Passive_5
    }

    public SkillType type;
    void Start()
    {
        Lv_text = GetComponentInChildren<TMP_Text>();
        sc = Hub.Inst.player_skill_system_sc;   
    }

    private void Update()
    {
        switch(type)
        {
            case SkillType.Active_0:
                if(Lv == sc.F_Get_Attack_Skill_Lv(0))
                {
                    return;
                }

                Lv = sc.F_Get_Attack_Skill_Lv(0);
                Lv_text.text = Lv.ToString();
                break;

            case SkillType.Active_1:
                if (Lv == sc.F_Get_Attack_Skill_Lv(1))
                {
                    return;
                }
                    Lv = sc.F_Get_Attack_Skill_Lv(1);
                Lv_text.text = Lv.ToString();
                break;

            case SkillType.Active_2:
                if (Lv == sc.F_Get_Attack_Skill_Lv(2))
                {
                    return;
                }
                Lv = sc.F_Get_Attack_Skill_Lv(2);
                Lv_text.text = Lv.ToString();
                break;

            case SkillType.Passive_0:
                if (Lv == sc.F_Get_Passive_Skill_Lv(0))
                {
                    return;
                }
                Lv = sc.F_Get_Passive_Skill_Lv(0);
                Lv_text.text = Lv.ToString();
                break;
        }    
    }



}
