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
    [SerializeField] int stageLv; // ����
    [SerializeField] int[] AddCount; // �������� Ȥ�� �ð��� �����Ǵ� ���ͷ�
    public int StageLv { get { return stageLv; } }
    [Header("# �������� ������ �ð�(��) �������  == >   # ����")]
    [Space]
    [SerializeField] List<int> StageLevelupTime = new List<int>(); // ������ �ð�
    [SerializeField] GameObject[] CloudeGroup;
    [SerializeField] BoxCollider2D[] noEntryColl;
    [Header("# Cheak Value")]
    [Space]
    [SerializeField] int Player_Area_num;
    [SerializeField] float GameTime;
    [SerializeField] float[] count = new float[2];




    // ���� ��ȯ�ؾ��� ����Ʈ
    int EnemyA_SpawnTrs;
    int EnemyB_SpawnTrs;

    //int SpawnTrs0; 
    //int SpawnTrs1; 
    //int SpawnTrs2;
    //int SpawnTrs3;
    //int SpawnTrs4;

    [SerializeField] EnemySpawnPointSc[] spawn_PointTrs; // ��������Ʈ ����Ʈ

    [SerializeField][Tooltip("��ũ,����,�ذ�ü�,������,��ũ������,����")] SpawnData[] spawnData;




    PoolManager pm;
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
        pm = PoolManager.Inst;
        TimeSc = transform.parent.GetComponentInChildren<UnitFrame_Updater>();
    }

    
    void Update()
    {

        //SpawnStart(); ������ �ð�����
        New_SpawnStart();


        Level_Updater();
        if (Input.GetKeyDown(KeyCode.L)) // 0�� ������ �׽�Ʈ
        {
            F_BlockTriggerOn(0);
        }
        if (Input.GetKeyDown(KeyCode.K)) // 1�� �� ���� �׽�Ʈ
        {
            F_BlockTriggerOn(1);
        }
    }

    private void Level_Updater()
    {
        // �������� 3������ ����
        if (StageLv == 3) { return; }

        {
            
        }
        GameTime = TimeSc.F_Get_GameTime();

        if(GameTime > StageLevelupTime[stageLv])
        {
            stageLv++;
            CameraManager.inst.F_CameraZoomOut(stageLv); // ī�޶� �ܾƿ�
            GameUIManager.Inst.F_GameInfoOpen(stageLv);

            if (stageLv == 1) // ������
            {
                // ���⼭ UI �رݵǾ��ٴ� Text���� �����������
                
                F_BlockTriggerOn(0);
            }
            else if(stageLv == 2)
            {
                // ���⼭ UI �رݵǾ��ٴ� Text���� �����������
                F_BlockTriggerOn(1);
            }
        }
        
    }

    private void New_SpawnStart()
    {
        if (gm.MainGameStart == false) { return; }

        EnemyA_SpawnTrs = (int)Mathf.Repeat(EnemyA_SpawnTrs, spawn_PointTrs.Length);
        EnemyB_SpawnTrs = (int)Mathf.Repeat(EnemyB_SpawnTrs, spawn_PointTrs.Length);
        
        int cheakValueA = spawn_PointTrs[EnemyA_SpawnTrs].AreaNumber;
        int cheakValueB = spawn_PointTrs[EnemyB_SpawnTrs].AreaNumber;
        
        if(cheakValueA > stageLv)
        {
            EnemyA_SpawnTrs++;
        }
        else if(cheakValueA <= stageLv)
        {
            count[0] += Time.deltaTime;

            if (count[0] > spawnData[Player_Area_num].interval[0])
            {
                count[0] = 0;

                for (int i = 0; i < spawnData[Player_Area_num].count[0]; i++) // �÷��̾� ��������� ù��° ī��Ʈ��ŭ
                {
                    GameObject obj =  pm.F_GetEnemyObj(spawnData[Player_Area_num].enemy_ID[0]);
                    obj.transform.position = spawn_PointTrs[EnemyA_SpawnTrs].transform.position;
                    obj.gameObject.SetActive(true);
                }

                Debug.Log(EnemyA_SpawnTrs);
                EnemyA_SpawnTrs++;
            }
            
        }

        if (cheakValueB > stageLv)
        {
            EnemyB_SpawnTrs++;
        }
        else if (cheakValueB <= stageLv)
        {
            count[1] += Time.deltaTime;

            if (count[1] > spawnData[Player_Area_num].interval[1])
            {
                count[1] = 0;

                for (int i = 0; i < spawnData[Player_Area_num].count[1]; i++) // �÷��̾� ��������� ù��° ī��Ʈ��ŭ
                {
                    GameObject obj  =  pm.F_GetEnemyObj(spawnData[Player_Area_num].enemy_ID[1]);
                    obj.transform.position = spawn_PointTrs[EnemyB_SpawnTrs].transform.position;
                    obj.gameObject.SetActive(true);
                }

                Debug.Log(EnemyB_SpawnTrs);
                EnemyB_SpawnTrs++;
            }
        
        }



        
        //spawn_PointTrs[EnemyA_SpawnTrs].transform.position;
    }


    private void SpawnStart() // ������� �ð��� ������
    {
        //SpawnTrs0 = (int)Mathf.Repeat(SpawnTrs0, spawn_PointTrs.Length); // ��������Ʈ �ƽø� ����
        //SpawnTrs1 = (int)Mathf.Repeat(SpawnTrs1, spawn_PointTrs.Length); 
        //SpawnTrs2 = (int)Mathf.Repeat(SpawnTrs2, spawn_PointTrs.Length); 
        //SpawnTrs3 = (int)Mathf.Repeat(SpawnTrs3, spawn_PointTrs.Length); 
        //SpawnTrs4 = (int)Mathf.Repeat(SpawnTrs4, spawn_PointTrs.Length); 

        //if (gm.MainGameStart == true)
        //{
        //     if(spawnData[Spawn_Level] == null ) {  return; }

        //    //��ũ ���� ����
        //    if ( spawnData[Spawn_Level].isSpawn_Orc == true) //Ŭ�������� �������� �о��
        //    {
        //        count[0] += Time.deltaTime;
                
        //        if (count[0] > spawnData[Spawn_Level]._0_Interval)
        //        {
        //            EnemySpawnPointSc sc = spawn_PointTrs[SpawnTrs0].GetComponent<EnemySpawnPointSc>();
        //            int cheakAreaNumber = sc.AreaNumber; //����Ʈ�� ��ҹ�ȣ ������
             
        //            if (cheakAreaNumber == Player_Area_num) // �÷��̾� ��ҿ� ������Ұ� �����ø� ����
        //            {
        //                count[0] = 0;
        //                for(int i = 0; i < spawnData[Spawn_Level]._0_EnemyCount; i++)
        //                {
        //                    GameObject obj = PoolManager.Inst.F_GetEnemyObj(0);
        //                    obj.transform.position = spawn_PointTrs[SpawnTrs0].transform.position;
        //                    obj.gameObject.SetActive(true);
        //                }
        //            }

        //            SpawnTrs0++;
        //        }
              
        //    }

        //    //���� ���� ����
        //    if (spawnData[Spawn_Level].isSpawn_Mushroom == true) 
        //    {
        //        count[1] += Time.deltaTime;

        //        if (count[1] > spawnData[Spawn_Level]._1_Interval)
        //        {

        //            EnemySpawnPointSc sc = spawn_PointTrs[SpawnTrs1].GetComponent<EnemySpawnPointSc>();
        //            int cheakAreaNumber = sc.AreaNumber;

                    

        //            if (cheakAreaNumber == Player_Area_num) // �÷��̾� ��ҿ� ������Ұ� �����ø� ����
        //            {
        //                count[1] = 0;

        //                for (int i = 0; i < spawnData[Spawn_Level]._1_EnemyCount; i++)
        //                {
        //                    GameObject obj = PoolManager.Inst.F_GetEnemyObj(1);
        //                    obj.transform.position = spawn_PointTrs[SpawnTrs1].transform.position;
        //                    obj.gameObject.SetActive(true);
        //                }

        //            }
        //            SpawnTrs1++;
        //        }
                
        //    }

        //    //�ü� ���� ����
        //    if (spawnData[Spawn_Level].isSpawn_Skeleton == true)
        //    {
        //        count[2] += Time.deltaTime;

        //        if (count[2] > spawnData[Spawn_Level]._2_Interval)
        //        {

        //            EnemySpawnPointSc sc = spawn_PointTrs[SpawnTrs2].GetComponent<EnemySpawnPointSc>();
        //            int cheakAreaNumber = sc.AreaNumber;

        //            if (cheakAreaNumber == Player_Area_num) // �÷��̾� ��ҿ� ������Ұ� �����ø� ����
        //            {
        //                count[2] = 0;
        //                for (int i = 0; i < spawnData[Spawn_Level]._2_EnemyCount; i++)
        //                {
        //                    GameObject obj = PoolManager.Inst.F_GetEnemyObj(2);
        //                    obj.transform.position = spawn_PointTrs[SpawnTrs2].transform.position;
        //                    obj.gameObject.SetActive(true);
        //                }

        //            }
        //            SpawnTrs2++;
        //        }
        
        //    }

        //    //������ ���� ����
        //    if (spawnData[Spawn_Level].isSpawn_Silme == true) //Ŭ�������� �������� �о��
        //    {
        //        count[3] += Time.deltaTime;

        //        if (count[3] > spawnData[Spawn_Level]._3_Interval)
        //        {
        //            EnemySpawnPointSc sc = spawn_PointTrs[SpawnTrs3].GetComponent<EnemySpawnPointSc>();
        //            int cheakAreaNumber = sc.AreaNumber; //����Ʈ�� ��ҹ�ȣ ������

        //            if (cheakAreaNumber == Player_Area_num) // �÷��̾� ��ҿ� ������Ұ� �����ø� ����
        //            {
        //                count[3] = 0;
        //                for (int i = 0; i < spawnData[Spawn_Level]._3_EnemyCount; i++)
        //                {
        //                    GameObject obj = PoolManager.Inst.F_GetEnemyObj(3);
        //                    obj.transform.position = spawn_PointTrs[SpawnTrs3].transform.position;
        //                    obj.gameObject.SetActive(true);
        //                }
        //            }

        //            SpawnTrs3++;
        //        }

        //    }

        //    //��ũ������ ���� ����
        //    if (spawnData[Spawn_Level].isSpawn_OrcRanager == true) //Ŭ�������� �������� �о��
        //    {
        //        count[4] += Time.deltaTime;

        //        if (count[4] > spawnData[Spawn_Level]._4_Interval)
        //        {
        //            EnemySpawnPointSc sc = spawn_PointTrs[SpawnTrs4].GetComponent<EnemySpawnPointSc>();
        //            int cheakAreaNumber = sc.AreaNumber; //����Ʈ�� ��ҹ�ȣ ������

        //            if (cheakAreaNumber == Player_Area_num) // �÷��̾� ��ҿ� ������Ұ� �����ø� ����
        //            {
        //                count[4] = 0;
        //                for (int i = 0; i < spawnData[Spawn_Level]._4_EnemyCount; i++)
        //                {
        //                    GameObject obj = PoolManager.Inst.F_GetEnemyObj(4);
        //                    obj.transform.position = spawn_PointTrs[SpawnTrs4].transform.position;
        //                    obj.gameObject.SetActive(true);
        //                }
        //            }

        //            SpawnTrs4++;
        //        }

        //    }
        //}
    }




    //�÷��̾� ��ұ��
    public void F_PlayerAreaValue_Changer(int value)
    {
        Player_Area_num = value;
    }

 

    public void F_BlockTriggerOn(int value)
    {
        noEntryColl[value].isTrigger = true;
    }

    public void F_DeleteCloud(int value)
    {
        CloudeGroup[value].gameObject.SetActive(false);
        // ���Ŀ� �ִϸ��̼� ���̵�ó���� ���濹��
    }


}

[System.Serializable]
public class SpawnData
{
    public int[] enemy_ID;
    public float[] interval;
    public int[] count;
   


    //������ ����  
    //public bool isSpawn_Orc; //�ʿ����,  ID , 
    //public float _0_Interval;
    //public int _0_EnemyCount;
    //[Space]
    //[Space]
    //public bool isSpawn_Mushroom;
    //public float _1_Interval;
    //public int _1_EnemyCount;
    //[Space]
    //[Space]
    //public bool isSpawn_Skeleton;
    //public float _2_Interval;
    //public int _2_EnemyCount;
    //[Space]
    //[Space]
    //public bool isSpawn_Silme;
    //public float _3_Interval;
    //public int _3_EnemyCount;
    //[Space]
    //[Space]
    //public bool isSpawn_OrcRanager;
    //public float _4_Interval;
    //public int _4_EnemyCount;
}
