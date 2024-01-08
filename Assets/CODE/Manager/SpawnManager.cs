using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager inst;
    

    [Header("# Spawn Setting  == >   # 예진")]
    [Space]
    [SerializeField] int Spawn_Level; // 레벨
    [SerializeField] int stageLv; // 레벨
    [SerializeField] int[] AddCount; // 스폰레벨 혹은 시간당 증가되는 몬스터량
    [SerializeField] float startDealy;
    public int StageLv { get { return stageLv; } }
    [Header("# 스폰레벨 오르는 시간(초) 적어야함  == >   # 예진")]
    [Space]
    [SerializeField] List<int> StageLevelupTime = new List<int>(); // 레벨당 시간
    [SerializeField] GameObject[] CloudeGroup;
    [SerializeField] BoxCollider2D[] noEntryColl;
    [Header("# Cheak Value")]
    [Space]
    [SerializeField] int Player_Area_num;
    [SerializeField] float GameTime;
    [SerializeField] float[] count = new float[2];
    float spawnTimeCounter;
    bool spawnstart;
    

    // 현재 소환해야할 포인트
    int EnemyA_SpawnTrs;
    int EnemyB_SpawnTrs;

    //int SpawnTrs0; 
    //int SpawnTrs1; 
    //int SpawnTrs2;
    //int SpawnTrs3;
    //int SpawnTrs4;

    [SerializeField] EnemySpawnPointSc[] spawn_PointTrs; // 스폰포인트 리스트

    // 지역에따른 몬스터 스폰유형, 간격, 카운트
    [SerializeField][Tooltip("오크,버섯,해골궁수,슬라임,오크레인저,나무")] SpawnData[] spawnData;
    [SerializeField][Tooltip("바뀔시간 ,카운트증가량, 스폰간격 단축")] LvCountUp[] lvCounUpData;




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
        SpawnStart_Dealy();

        //SpawnStart(); 구버전 시간단위
        New_SpawnStart();
        SpawnLvUpdater();

        Level_Updater();

        if (Input.GetKeyDown(KeyCode.L)) // 0번 벽제거 테스트
        {
            F_BlockTriggerOn(0);
        }
        if (Input.GetKeyDown(KeyCode.K)) // 1번 벽 제거 테스트
        {
            F_BlockTriggerOn(1);
        }
    }
    /// <summary>
    /// 시간에 따른 카운트 증가량
    /// </summary>
    private void SpawnLvUpdater()
    {
        if(gm.MainGameStart == false) { return; }
        if(Spawn_Level == lvCounUpData.Length) { return; } //최대 설정값보다 많아지면 리턴


        spawnTimeCounter += Time.deltaTime;
        if(spawnTimeCounter > lvCounUpData[Spawn_Level].nextSpawnLvtime)
        {
            Spawn_Level++;
        }
        
    }
    private void Level_Updater()
    {
        // 스테이지 3레벨이 보스
        if (StageLv == 3) { return; }
       
        GameTime = TimeSc.F_Get_GameTime();

        if(GameTime > StageLevelupTime[stageLv])
        {
            stageLv++;
            CameraManager.inst.F_CameraZoomOut(stageLv); // 카메라 줌아웃
            GameUIManager.Inst.F_GameInfoOpen(stageLv);

            if (stageLv == 1) // 벽제거
            {
                // 여기서 UI 해금되었다는 Text만들어서 연출해줘야함
                
                F_BlockTriggerOn(0);
            }
            else if(stageLv == 2)
            {
                // 여기서 UI 해금되었다는 Text만들어서 연출해줘야함
                F_BlockTriggerOn(1);
            }
        }
        
    }
    int addCountA, addCountB;
    private void New_SpawnStart()
    {
        if (gm.MainGameStart == false || spawnstart == false) { return; }

        //스폰 트랜스폼 순차적으로 돌게 작업
        EnemyA_SpawnTrs = (int)Mathf.Repeat(EnemyA_SpawnTrs, spawn_PointTrs.Length);
        EnemyB_SpawnTrs = (int)Mathf.Repeat(EnemyB_SpawnTrs, spawn_PointTrs.Length);
        
        // 현재 스폰지역의 넘버확인
        int cheakValueA = spawn_PointTrs[EnemyA_SpawnTrs].AreaNumber;
        int cheakValueB = spawn_PointTrs[EnemyB_SpawnTrs].AreaNumber;

        // A타입 스폰준비
        // 현재 해금 안된지역이라면 다음 으로 넘어감
        if (cheakValueA > stageLv)
        {
            EnemyA_SpawnTrs++;
        }
        else if(cheakValueA <= stageLv) // 해금지역이라면 스폰
        {
            count[0] += Time.deltaTime;
            
            //기존 스폰시간 - 스폰레벨당 단축시간
            if (count[0] > spawnData[Player_Area_num].interval[0] - lvCounUpData[Spawn_Level].IntervalDown)
            {
                count[0] = 0;

                

                if (Spawn_Level == lvCounUpData.Length) 
                {
                    addCountA = 0;
                }
                else if (Spawn_Level < lvCounUpData.Length)
                {
                    addCountA = lvCounUpData[Spawn_Level].addCount;
                }

                    //기본스폰데이터 카운트량  + 스폰레벨에 따른 추가증가량
                    for (int i = 0; i < spawnData[Player_Area_num].count[0] + addCountA; i++) 
                {
                                                                                           //스폰데이터의 넘버에 있는 몬스터 풀링
                    GameObject obj =  pm.F_GetEnemyObj(spawnData[cheakValueA].enemy_ID[0]);
                    obj.transform.position = spawn_PointTrs[EnemyA_SpawnTrs].transform.position 
                                                               + new Vector3(Random.Range(0.5f,1f), Random.Range(0.5f, 1f));
                                                               // A,B 타입이 겹쳐나오는것을 방지=> 랜덤카운트
                          
                    obj.gameObject.SetActive(true);
                    gm.SpawnCount++;
                }

                EnemyA_SpawnTrs++;
                addCountA = 0;
            }
            
        }

        // B타입 스폰준비
        //위와 동일
        if (cheakValueB > stageLv)
        {
            EnemyB_SpawnTrs++;
        }
        else if (cheakValueB <= stageLv)
        {
            count[1] += Time.deltaTime;

            if (count[1] > spawnData[Player_Area_num].interval[1] - lvCounUpData[Spawn_Level].IntervalDown)
            {
                count[1] = 0;

                if (Spawn_Level == lvCounUpData.Length)
                {
                    addCountB = 0;
                }
                else if (Spawn_Level < lvCounUpData.Length)
                {
                    addCountB = lvCounUpData[Spawn_Level].addCount;
                }

                for (int i = 0; i < spawnData[Player_Area_num].count[1] + addCountB; i++) 
                {
                    GameObject obj  =  pm.F_GetEnemyObj(spawnData[cheakValueB].enemy_ID[1]);
                    obj.transform.position = spawn_PointTrs[EnemyB_SpawnTrs].transform.position + new Vector3(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f)); ;
                    obj.gameObject.SetActive(true);
                }

                EnemyB_SpawnTrs++;
                addCountB = 0;
                gm.SpawnCount++;
            }
        
        }
    }

    float dealycount;
    private void SpawnStart_Dealy()
    {
        if (spawnstart) { return; }

        dealycount += Time.deltaTime;
        if( dealycount > startDealy)
        {
            spawnstart = true;
        }
    }

    private void SpawnStart() // 구형방식 시간별 스포너
    {
        //SpawnTrs0 = (int)Mathf.Repeat(SpawnTrs0, spawn_PointTrs.Length); // 스폰포인트 맥시멈 제한
        //SpawnTrs1 = (int)Mathf.Repeat(SpawnTrs1, spawn_PointTrs.Length); 
        //SpawnTrs2 = (int)Mathf.Repeat(SpawnTrs2, spawn_PointTrs.Length); 
        //SpawnTrs3 = (int)Mathf.Repeat(SpawnTrs3, spawn_PointTrs.Length); 
        //SpawnTrs4 = (int)Mathf.Repeat(SpawnTrs4, spawn_PointTrs.Length); 

        //if (gm.MainGameStart == true)
        //{
        //     if(spawnData[Spawn_Level] == null ) {  return; }

        //    //오크 스폰 여부
        //    if ( spawnData[Spawn_Level].isSpawn_Orc == true) //클래스에서 스폰여부 읽어옴
        //    {
        //        count[0] += Time.deltaTime;
                
        //        if (count[0] > spawnData[Spawn_Level]._0_Interval)
        //        {
        //            EnemySpawnPointSc sc = spawn_PointTrs[SpawnTrs0].GetComponent<EnemySpawnPointSc>();
        //            int cheakAreaNumber = sc.AreaNumber; //포인트의 장소번호 가져옴
             
        //            if (cheakAreaNumber == Player_Area_num) // 플레이어 장소와 스폰장소가 같을시만 스폰
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

        //    //버섯 스폰 여부
        //    if (spawnData[Spawn_Level].isSpawn_Mushroom == true) 
        //    {
        //        count[1] += Time.deltaTime;

        //        if (count[1] > spawnData[Spawn_Level]._1_Interval)
        //        {

        //            EnemySpawnPointSc sc = spawn_PointTrs[SpawnTrs1].GetComponent<EnemySpawnPointSc>();
        //            int cheakAreaNumber = sc.AreaNumber;

                    

        //            if (cheakAreaNumber == Player_Area_num) // 플레이어 장소와 스폰장소가 같을시만 스폰
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

        //    //궁수 스폰 여부
        //    if (spawnData[Spawn_Level].isSpawn_Skeleton == true)
        //    {
        //        count[2] += Time.deltaTime;

        //        if (count[2] > spawnData[Spawn_Level]._2_Interval)
        //        {

        //            EnemySpawnPointSc sc = spawn_PointTrs[SpawnTrs2].GetComponent<EnemySpawnPointSc>();
        //            int cheakAreaNumber = sc.AreaNumber;

        //            if (cheakAreaNumber == Player_Area_num) // 플레이어 장소와 스폰장소가 같을시만 스폰
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

        //    //슬라임 스폰 여부
        //    if (spawnData[Spawn_Level].isSpawn_Silme == true) //클래스에서 스폰여부 읽어옴
        //    {
        //        count[3] += Time.deltaTime;

        //        if (count[3] > spawnData[Spawn_Level]._3_Interval)
        //        {
        //            EnemySpawnPointSc sc = spawn_PointTrs[SpawnTrs3].GetComponent<EnemySpawnPointSc>();
        //            int cheakAreaNumber = sc.AreaNumber; //포인트의 장소번호 가져옴

        //            if (cheakAreaNumber == Player_Area_num) // 플레이어 장소와 스폰장소가 같을시만 스폰
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

        //    //오크레인저 스폰 여부
        //    if (spawnData[Spawn_Level].isSpawn_OrcRanager == true) //클래스에서 스폰여부 읽어옴
        //    {
        //        count[4] += Time.deltaTime;

        //        if (count[4] > spawnData[Spawn_Level]._4_Interval)
        //        {
        //            EnemySpawnPointSc sc = spawn_PointTrs[SpawnTrs4].GetComponent<EnemySpawnPointSc>();
        //            int cheakAreaNumber = sc.AreaNumber; //포인트의 장소번호 가져옴

        //            if (cheakAreaNumber == Player_Area_num) // 플레이어 장소와 스폰장소가 같을시만 스폰
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




    //플레이어 장소기록
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
        // 추후에 애니메이션 페이드처리로 변경예정
    }


}

[System.Serializable]
public class SpawnData
{
    public string nameText;
    public int[] enemy_ID;
    public float[] interval;
    public int[] count;
}

[System.Serializable]
public class LvCountUp
{
    public string nameText;
    public float nextSpawnLvtime;
    public int addCount;
    public int IntervalDown;
}

