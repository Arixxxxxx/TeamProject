using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public enum WhoAreYou
    {
        Player, Enemy_Melee, Skel_Arrow, PosionBadak , Orc_Stone
    }
    [SerializeField] float myDMG;
    [Header("# �� üũ Ȯ���ϼ���")]

    public WhoAreYou type;

    EnemyStats enemySC;
    Animator anim;
    Enemy_Arrow enemy_Arrow;


    private void Start()
    {
        switch (type)
        {
            case WhoAreYou.PosionBadak:
                   anim = GetComponent<Animator>();
                break;

            case WhoAreYou.Skel_Arrow:
            case WhoAreYou.Orc_Stone:
                if (enemy_Arrow == null) { enemy_Arrow = GetComponent<Enemy_Arrow>(); }
                break;
        }
    }




    //Ǯ���� ��ü���� ������ �޾ƿ�
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
                case WhoAreYou.Skel_Arrow:
                case WhoAreYou.Orc_Stone:
                    
                    if (enemy_Arrow == null)  { enemy_Arrow = GetComponent<Enemy_Arrow>(); }
                    enemy_Arrow.F_Return_Obj();

                    break;
            }
        }


        if (collision.CompareTag("Object"))
        {
            
            switch
                (type)
            {
                case WhoAreYou.Skel_Arrow:
                    PoolManager.Inst.F_ReturnObj(gameObject, 0);
                    break;

                case WhoAreYou.Orc_Stone:
                    PoolManager.Inst.F_ReturnObj(gameObject, 5);
                    break;
            }
        }

    }

    public void A_PoisonBadak_return()
    {
        PoolManager.Inst.F_ReturnObj(gameObject, 4);
    }
 
}
