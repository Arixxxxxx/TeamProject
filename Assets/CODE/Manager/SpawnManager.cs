using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager inst;
    

    [Header("# Spawn Setting  == >   # ����")]
    [Space]
    [SerializeField] int Spawn_Level; // ����
    [Header("# �������� ������ �ð�(��) �������  == >   # ����")]
    [Space]
    [SerializeField] List<int> LevelTable = new List<int>(); // ������ �ð�
  

    [SerializeField] int SpawnTrs0; // ���� ��ȯ�ؾ��� ����Ʈ
    [SerializeField] int SpawnTrs1; // ���� ��ȯ�ؾ��� ����Ʈ
    [SerializeField] int SpawnTrs2; // ���� ��ȯ�ؾ��� ����Ʈ
    [SerializeField] Transform[] spawn_PointTrs; // ��������Ʈ ����Ʈ

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
        SpawnTrs0 = (int)Mathf.Repeat(SpawnTrs0, spawn_PointTrs.Length); // ��������Ʈ �ƽø� ����
        SpawnTrs1 = (int)Mathf.Repeat(SpawnTrs1, spawn_PointTrs.Length); 
        SpawnTrs2 = (int)Mathf.Repeat(SpawnTrs2, spawn_PointTrs.Length); 

        if (gm.MainGameStart == true)
        {
             if(spawnData[Spawn_Level] == null ) { Debug.LogError("SpawnData ����Ʈ �߰��ۼ� �ʿ�"); return; }

            //��ũ ���� ����
            if ( spawnData[Spawn_Level].isSpawn_Orc == true) //Ŭ�������� �������� �о��
            {
                count[0] += Time.deltaTime;
                
                if (count[0] > spawnData[Spawn_Level].spawn_0_Interval)
                {
                    EnemySpawnPointSc sc = spawn_PointTrs[SpawnTrs0].GetComponent<EnemySpawnPointSc>();
                    int cheakAreaNumber = sc.AreaNumber; //����Ʈ�� ��ҹ�ȣ ������
             
                    if (cheakAreaNumber == Player_Area_num) // �÷��̾� ��ҿ� ������Ұ� �����ø� ����
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

            //���� ���� ����
            if (spawnData[Spawn_Level].isSpawn_Mushroom == true) 
            {
                count[1] += Time.deltaTime;

                if (count[1] > spawnData[Spawn_Level].spawn_1_Interval)
                {

                    EnemySpawnPointSc sc = spawn_PointTrs[SpawnTrs1].GetComponent<EnemySpawnPointSc>();
                    int cheakAreaNumber = sc.AreaNumber;

                    

                    if (cheakAreaNumber == Player_Area_num) // �÷��̾� ��ҿ� ������Ұ� �����ø� ����
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

            //�ü� ���� ����
            if (spawnData[Spawn_Level].isSpanw_Skeleton == true)
            {
                count[2] += Time.deltaTime;

                if (count[2] > spawnData[Spawn_Level].spawn_2_Interval)
                {

                    EnemySpawnPointSc sc = spawn_PointTrs[SpawnTrs2].GetComponent<EnemySpawnPointSc>();
                    int cheakAreaNumber = sc.AreaNumber;

                    if (cheakAreaNumber == Player_Area_num) // �÷��̾� ��ҿ� ������Ұ� �����ø� ����
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



    //�÷��̾� ��ұ��
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
