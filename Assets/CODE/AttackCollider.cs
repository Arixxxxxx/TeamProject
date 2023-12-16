using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public enum WhoAreYou
    {
        Player, Enemy_Melee, Enemy_Range
    }
    [SerializeField] float myDMG;
    [Header("# 꼭 체크 확인하세요")]

    public WhoAreYou type;

    EnemyStats enemySC;
    


    private void Start()
    {
        switch (type)
        {
            case WhoAreYou.Player:

                break;

            case WhoAreYou.Enemy_Melee:

                break;
        }
    }




    //풀링시 본체에서 데미지 받아옴
    public void F_SetAttackDMG(float DMG)
    {
        myDMG = DMG;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.GetComponent<Player_Stats>() != null)
        {
            collision.GetComponent<Player_Stats>().F_Player_On_Hit(myDMG);

            switch
                (type)
            {
                case WhoAreYou.Enemy_Range:
                    PoolManager.Inst.F_ReturnObj(gameObject, 0);
                    break;
            }
        }
        if (collision.CompareTag("Object"))
        {
            
            switch
                (type)
            {
                case WhoAreYou.Enemy_Range:
                    PoolManager.Inst.F_ReturnObj(gameObject, 0);
                    break;
            }
        }

    }

 
}
