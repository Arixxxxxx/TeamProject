using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Attack : MonoBehaviour
{
    public enum EnemyName { Orc, Mushroom }
    public EnemyName type;
    private Animator anim;
    [SerializeField] bool isAttack;
    public bool IsAttack { get { return isAttack; } }

    NavMeshAgent nav;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
    }

    void Start()
    {

    }

    void Update()
    {
        Cheak_Distance();
        Attack_Anim();
    }
    bool doAttack;
    private void Attack_Anim()
    {
        if(isAttack == true && anim.GetBool("Attack")== false) 
        {

        }
    }
    private void Cheak_Distance()
    {

        if (nav.remainingDistance < 1.6f)
        {
            isAttack = true; 
        }
        else if (nav.remainingDistance > 1.6f)
        {
            isAttack = false; 
        }
    }

    IEnumerator AttackAnim()
    {
        anim.SetBool("Attack", true);
        yield return null;
        while (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") == false)
        {
            yield return null;
        }
        doAttack = false;
    }

}
