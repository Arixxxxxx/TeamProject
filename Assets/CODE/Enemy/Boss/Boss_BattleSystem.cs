using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_BattleSystem : MonoBehaviour
{
    [Header("# Cheak Value")]

    [SerializeField] bool bossBattleStart;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        returnUpdate();


    }







    private void returnUpdate() //전투시작 체크 및 알림
    {
        if (bossBattleStart == false)
        {
            bossBattleStart = GameManager.Inst.BossBattleStart;
        }

        if (!bossBattleStart) { return; }
    }
}
