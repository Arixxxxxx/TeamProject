using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager inst;
    

    [Header("# Spawn Setting  == >   # 예진")]
    [Space]
    [SerializeField] int Spawn_Level; // 레벨
    [Header("# 스폰레벨 오르는 시간(초) 적어야함  == >   # 예진")]
    [Space]
    [SerializeField] List<int> LevelTable = new List<int>(); // 레벨당 시간
  

    [SerializeField] int SpawnTrs0; // 현재 소환해야할 포인트
    [SerializeField] int SpawnTrs1; // 현재 소환해야할 포인트
    [SerializeField] int SpawnTrs2; // 현재 소환해야할 포인트
    [SerializeField] Transform[] spawn_PointTrs; // 스폰포인트 리스트

    [SerializeField] SpawnData[] spawnData;

    [Header("# Cheak Value")]
    [Space]
    [SerializeField] int Player_Area_num;
    [SerializeField] float GameTime;
    [SerializeField] float[] count = new float[3];
    


    GameManager gm;
    UnitFrame_Updater TimeSc;

    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        gm = GameManager.Inst;
        TimeSc = transform.parent.GetComponentInChildren<UnitFrame_Updater>();
    }

    // Update is called once per frame
    void Update()
    {
        GameTime = TimeSc.F_Get_GameTime();
        SpawnStart();
        Level_Updater();
    }
    private void SpawnStart()
    {
        SpawnTrs0 = (int)Mathf.Repeat(SpawnTrs0, spawn_PointTrs.Length); // 스폰포인트 맥시멈 제한
        SpawnTrs1 = (int)Mathf.Repeat(SpawnTrs1, spawn_PointTrs.Length); 
        SpawnTrs2 = (int)Mathf.Repeat(SpawnTrs2, spawn_PointTrs.Length); 

        if (gm.MainGameStart == true)
        {
             if(spawnData[Spawn_Level] == null ) { Debug.LogError("SpawnData 리스트 추가작성 필요"); return; }

            //오크 스폰 여부
            if ( spawnData[Spawn_Level].isSpawn_Orc == true) //클래스에서 스폰여부 읽어옴
            {
                count[0] += Time.deltaTime;
                
                if (count[0] > spawnData[Spawn_Level].spawn_0_Interval)
                {
                    EnemySpawnPointSc sc = spawn_PointTrs[SpawnTrs0].GetComponent<EnemySpawnPointSc>();
                    int cheakAreaNumber = sc.AreaNumber; //포인트의 장소번호 가져옴
             
                    if (cheakAreaNumber == Player_Area_num) // 플레이어 장소와 스폰장소가 같을시만 스폰
                    {
                        count[0] = 0;
                        for(int i = 0; i < spawnData[Spawn_Level].spawn_0_EnemyCount; i++)
                        {
                            GameObject obj = PoolManager.Inst.F_GetEnemyObj(0);
                            obj.transform.position = spawn_PointTrs[SpawnTrs0].transform.position;
                            obj.gameObject.SetActive(true);
                        }
                    }

                    SpawnTrs0++;
                }
              
            }

            //버섯 스폰 여부
            if (spawnData[Spawn_Level].isSpawn_Mushroom == true) 
            {
                count[1] += Time.deltaTime;

                if (count[1] > spawnData[Spawn_Level].spawn_1_Interval)
                {

                    EnemySpawnPointSc sc = spawn_PointTrs[SpawnTrs1].GetComponent<EnemySpawnPointSc>();
                    int cheakAreaNumber = sc.AreaNumber;

                    

                    if (cheakAreaNumber == Player_Area_num) // 플레이어 장소와 스폰장소가 같을시만 스폰
                    {
                        count[1] = 0;

                        for (int i = 0; i < spawnData[Spawn_Level].spawn_1_EnemyCount; i++)
                        {
                            GameObject obj = PoolManager.Inst.F_GetEnemyObj(1);
                            obj.transform.position = spawn_PointTrs[SpawnTrs1].transform.position;
                            obj.gameObject.SetActive(true);
                        }

                    }
                    SpawnTrs1++;
                }
                
            }

            //궁수 스폰 여부
            if (spawnData[Spawn_Level].isSpanw_Skeleton == true)
            {
                count[2] += Time.deltaTime;

                if (count[2] > spawnData[Spawn_Level].spawn_2_Interval)
                {

                    EnemySpawnPointSc sc = spawn_PointTrs[SpawnTrs2].GetComponent<EnemySpawnPointSc>();
                    int cheakAreaNumber = sc.AreaNumber;

                    if (cheakAreaNumber == Player_Area_num) // 플레이어 장소와 스폰장소가 같을시만 스폰
                    {
                        count[2] = 0;
                        for (int i = 0; i < spawnData[Spawn_Level].spawn_2_EnemyCount; i++)
                        {
                            GameObject obj = PoolManager.Inst.F_GetEnemyObj(2);
                            obj.transform.position = spawn_PointTrs[SpawnTrs2].transform.position;
                            obj.gameObject.SetActive(true);
                        }

                    }
                    SpawnTrs2++;
                }
        
            }

           
        }
    }


    private void EnemySpone(int type)
    {
        PoolManager.Inst.F_GetEnemyObj(type);
    }



    //플레이어 장소기록
    public void F_PlayerAreaValue_Changer(int value)
    {
        Player_Area_num = value;
    }

    private void Level_Updater()
    {
        if (LevelTable.Count == Spawn_Level) { return; }

        if (GameTime > LevelTable[Spawn_Level])
        {
            Spawn_Level++;
        }
    }
}

[System.Serializable]
public class SpawnData
{
    public bool isSpawn_Orc;
    public float spawn_0_Interval;
    public int spawn_0_EnemyCount;
    [Space]
    [Space]
    public bool isSpawn_Mushroom;
    public float spawn_1_Interval;
    public int spawn_1_EnemyCount;
    [Space]
    [Space]
    public bool isSpanw_Skeleton;
    public float spawn_2_Interval;
    public int spawn_2_EnemyCount;
}
