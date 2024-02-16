using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dmg_Object : MonoBehaviour
{
    public enum SkillType { Skill_0, Skill_1, Skill_2, Skill_3, Skill_4 }
    public SkillType type;

    [Header(" # Input Object DMG !! ")]
    [SerializeField] float DMG;
    [SerializeField] float critical_Value;
    [SerializeField] List<GameObject> EnemyList = new List<GameObject>();
    
    float s2_SkillCoolTime;

    Player_Skill_System skill;

    [SerializeField] GameObject bossObj;
    Boss_Status boss_stat_sc;
    void Start()
    {
        

    }

    private void OnEnable()
    {

        if(skill == null)
        {
            skill = Player_Skill_System.Inst;
        }
        critical_Value = skill.F_Get_Player_Critical();

    }
    // Update is called once per frame
    void Update()
    {

        
        Dmg_Updater();


        if (type == SkillType.Skill_2)
        {
            S2_Attack_Function();
        }
        
    }

    float count, count1;
    float dice1;

    // È­¿°º¸È£¸· Æ½ ´ë¹ÌÁö
    private void S2_Attack_Function()
    {
        count += Time.deltaTime;
        count1 += Time.deltaTime;

        //ÂÌ¸÷ °ø°Ý
        if (type == SkillType.Skill_2 && EnemyList.Count != 0)
        {
            if (s2_SkillCoolTime != skill.F_Get_Skill2_CoolTime()) // ½ºÅ³·¹º§º° ÄðÅ¸ÀÓ°¡Á®¿È
            {
                s2_SkillCoolTime = skill.F_Get_Skill2_CoolTime();
            }

            if (count > s2_SkillCoolTime)
            {
                count = 0;

                for (int i = 0; i < EnemyList.Count; i++)
                {
                    EnemyStats sc = EnemyList[i].GetComponent<EnemyStats>();

                    dice = Random.Range(0, 100);

                    if (dice < critical_Value)
                    {
                        sc.F_Enemy_On_Hit(DMG, true, true);
                    }
                    else
                    {
                        sc.F_Enemy_On_Hit(DMG, false, true);
                    }
                }
            }
        }


            // º¸½º°ø°Ý
            if (type == SkillType.Skill_2 && bossObj != null)
            {
                if (s2_SkillCoolTime != skill.F_Get_Skill2_CoolTime()) // ½ºÅ³·¹º§º° ÄðÅ¸ÀÓ°¡Á®¿È
                {
                    s2_SkillCoolTime = skill.F_Get_Skill2_CoolTime();
                }

                if (count1 > s2_SkillCoolTime)
                {
                    count1 = 0;
                    dice1 = Random.Range(0, 100);

                    if (dice1 < critical_Value)
                    {
                        boss_stat_sc.F_Enemy_On_Hit(DMG, true);
                    }
                    else
                    {
                        boss_stat_sc.F_Enemy_On_Hit(DMG, false);

                    }
                }

            }
    }


    private void Dmg_Updater()
    {
        switch (type)
        {
            case SkillType.Skill_0:
                DMG = skill.F_GetSkillDMG(0);
                break;


            case SkillType.Skill_1:
                DMG = skill.F_GetSkillDMG(1);
                break;

            case SkillType.Skill_2:
                DMG = skill.F_GetSkillDMG(2);
                break;

            case SkillType.Skill_3:
                DMG = skill.F_GetSkillDMG(3);
                break;

            case SkillType.Skill_4:
                DMG = skill.F_GetSkillDMG(4);
                break;
        }
    }

    float dice;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (type == SkillType.Skill_2 && EnemyList.Contains(collision.gameObject) == false && collision.CompareTag("Enemy") && collision.gameObject.name != "Boss")
        {
            EnemyList.Add(collision.gameObject);
        }

        if (type == SkillType.Skill_2 && bossObj == null && collision.CompareTag("Enemy") && collision.gameObject.name == "Boss")
        {
            if (boss_stat_sc == null)
            {
                boss_stat_sc = collision.GetComponent<Boss_Status>();
            }

            bossObj = collision.gameObject;
        }


        // ÂÌ¸÷
        if (collision.CompareTag("Enemy") && collision.GetComponent<EnemyStats>() != null && type != SkillType.Skill_2)
        {
            if(DMG == 0)
            {
                Dmg_Updater();
            }
            dice = Random.Range(0, 100);

            if (dice < critical_Value)
            {
                collision.GetComponent<EnemyStats>().F_Enemy_On_Hit(DMG, true, false);
            }
            else
            {
                collision.GetComponent<EnemyStats>().F_Enemy_On_Hit(DMG, false, false);
            }
        }

        //º¸½º
        if (collision.CompareTag("Enemy") && collision.GetComponent<Boss_Status>() != null && type != SkillType.Skill_2)
        {
            if (DMG == 0)
            {
                Dmg_Updater();
            }
            dice = Random.Range(0, 100);

            if (dice < critical_Value)
            {
                collision.GetComponent<Boss_Status>().F_Enemy_On_Hit(DMG, true);
            }
            else
            {
                collision.GetComponent<Boss_Status>().F_Enemy_On_Hit(DMG, false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (type == SkillType.Skill_2 && EnemyList.Contains(collision.gameObject) == true)
        {
            EnemyList.Remove(collision.gameObject);
        }

        if (type == SkillType.Skill_2 && bossObj != null && collision.CompareTag("Enemy") && collision.gameObject.name == "Boss")
        {
            bossObj = null;
        }
    }

}
