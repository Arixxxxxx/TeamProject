using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dmg_Object : MonoBehaviour
{
    [Header(" # Input Object DMG !! ")]
    [SerializeField] float DMG;
    [SerializeField] float critical_Value;

    Player_Skill_System skill;
    void Start()
    {
        skill = GetComponentInParent<Player_Skill_System>();      
    }

    // Update is called once per frame
    void Update()
    {
        if(skill != null)
        {
            critical_Value = skill.F_Get_Player_Critical();
        }
    }

    float dice;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision.GetComponent<EnemyStats>() != null)
        {
            dice = Random.Range(0, 100);
            
            if(dice < critical_Value)
            {
                collision.GetComponent<EnemyStats>().F_Enemy_On_Hit(DMG , true);
            }
            else
            {
                collision.GetComponent<EnemyStats>().F_Enemy_On_Hit(DMG, false);
            }
        }

        if (collision.CompareTag("Enemy") && collision.GetComponent<Boss_Status>() != null)
        {
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
}
