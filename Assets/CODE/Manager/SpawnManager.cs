using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("# Spawn Setting")]
    [Space]
    [SerializeField] int spawn_EnemyType;
    [SerializeField] int spawn_EnemyCount;
    [SerializeField] float spawn_Interval;
    [SerializeField] float[] spawn_TimeTable;
    [SerializeField] int spawn_PointNumber;
    [SerializeField] Transform[] spawn_PointTrs;
    [Header("# Cheak Value")]
    [Space]
    [SerializeField] float GameTime;

    GameManager gm;

    bool pattan_00;
        
    void Start()
    {
        gm = GameManager.Inst;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnStart();
    }
    private void SpawnStart()
    {
        if(gm.MainGameStart == true)
        {

        }
    }
}


