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







    private void returnUpdate() //�������� üũ �� �˸�
    {
        if (bossBattleStart == false)
        {
            bossBattleStart = GameManager.Inst.BossBattleStart;
        }

        if (!bossBattleStart) { return; }
    }
}
