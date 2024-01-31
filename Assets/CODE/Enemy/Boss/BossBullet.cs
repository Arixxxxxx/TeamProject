using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    Player_Stats sc;
    bool onceAttack;
    [SerializeField] float InputDMG;
    float DMG;

    private void OnEnable()
    {
        if(onceAttack)
        {
            onceAttack = false;
        }


        
    }
    void Start()
    {
        sc = GameManager.Inst.F_Get_PlayerSc();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void F_SetDMG(float value)
    {
        DMG = value;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && collision.name != "Dragon" && onceAttack == false)
        {
            onceAttack = true;
            
            if(DMG == 0)
            {
                sc.F_Player_On_Hit(InputDMG);
            }
            else
            {
                sc.F_Player_On_Hit(DMG);
            }
        }
    }
}
