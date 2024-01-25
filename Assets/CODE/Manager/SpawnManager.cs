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
    [SerializeField] int playerAreaNum;
    public int PlayerAreaNum { get { return playerAreaNum; } }

    [SerializeField] float GameTime;
    [SerializeField] float[] count = new float[2];
    float spawnTimeCounter;
    bool spawnstart;


    // 현재 소환해야할 포인트
    int EnemyA_SpawnTrs;
    int EnemyB_SpawnTrs;


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
        GameTime = TimeSc.F_Get_GameTime(); // 게임시간 체크

        SpawnStart_Dealy();

        //SpawnStart(); 구버전 시간단위
        New_SpawnStart();
        SpawnLvUpdater();
        Level_Updater();
        //MapInfoArrowActiveUpdater();

        if (Input.GetKeyDown(KeyCode.L)) // 2번 벽제거 테스트
        {
            F_BlockTriggerOn(2);
        }
     
    }
    /// <summary>
    /// 시간에 따른 카운트 증가량
    /// </summary>
    [SerializeField] float nextLvTime;
    private void SpawnLvUpdater()
    {
        if (gm.MainGameStart == false || Spawn_Level == 10) { return; }

        if (Spawn_Level == lvCounUpData.Length) { return; } //최대 설정값보다 많아지면 리턴

        nextLvTime = lvCounUpData[Spawn_Level].nextSpawnLvtime;

        spawnTimeCounter += Time.deltaTime;

        if (spawnTimeCounter > nextLvTime)
        {
            Spawn_Level++;
        }

    }

    bool arrowPopup0, arrowPopup1;

    /// <summary>
    /// 게임시간 체크해서 화살표 켜주는 함수
    /// </summary>
    private void MapInfoArrowActiveUpdater()
    {
        if(GameTime > StageLevelupTime[0] - 20 && arrowPopup0 == false)
        {
            arrowPopup0 = true;
            GameUIManager.Inst.F_SetNextMapArrow(0);
        }

        if (GameTime > StageLevelupTime[1] - 20 && arrowPopup1 == false)
        {
            arrowPopup1 = true;
            GameUIManager.Inst.F_SetNextMapArrow(1);
        }
    }

    public void F_StageLvUp()
    {
        stageLv++;

        if(stageLv < 2)
        {
            CameraManager.inst.F_CameraZoomOut(stageLv);
        }
    }

    int timeListValue = 0;
    private void Level_Updater()
    {
        // 스테이지 3레벨이 보스
        if (timeListValue == 3) { return; }

        if (GameTime > StageLevelupTime[timeListValue])
        {
        
            if (timeListValue == 0) // 벽 트리거 true (뚫고 지나갈수잇게)
            {
                
                GameUIManager.Inst.F_GameInfoOpen(timeListValue); // 해금안내문구
                F_BlockTriggerOn(0); // 트리거 작동
                timeListValue++;
            }
            else if (timeListValue == 1)
            {
                // 여기서 UI 해금되었다는 Text만들어서 연출해줘야함
                GameUIManager.Inst.F_GameInfoOpen(timeListValue);
                F_BlockTriggerOn(1);
                timeListValue++;
            }
            else if (timeListValue == 2)
            {
                stageLv++;
                GameUIManager.Inst.F_GameInfoOpen(timeListValue);
                F_BlockTriggerOn(2);
                
                
                timeListValue++;
            }
        }
    }

    // 구버전 (시간대비 레벨업 => 발동 트리거 변경 24.01.24)
    //private void Level_Updater() 
    //{
    //    // 스테이지 3레벨이 보스
    //    if (StageLv == 3) { return; }

    //    GameTime = TimeSc.F_Get_GameTime();

    //    if (GameTime > StageLevelupTime[stageLv])
    //    {
    //        stageLv++;
    //        CameraManager.inst.F_CameraZoomOut(stageLv); // 카메라 줌아웃
    //        GameUIManager.Inst.F_GameInfoOpen(stageLv);

    //        if (stageLv == 1) // 벽제거
    //        {
    //            // 여기서 UI 해금되었다는 Text만들어서 연출해줘야함

    //            F_BlockTriggerOn(0);
    //        }
    //        else if (stageLv == 2)
    //        {
    //            // 여기서 UI 해금되었다는 Text만들어서 연출해줘야함
    //            F_BlockTriggerOn(1);
    //        }
    //        else if(stageLv == 3)
    //        {
    //            F_BlockTriggerOn(2);
    //        }
    //    }
    //}
    int addCountA, addCountB;
    [SerializeField] float spawnTImeA, spawnTImeB;
    [SerializeField] int spawnEnemyA, spawnEnemyB;
    private void New_SpawnStart()
    {
        if (gm.MainGameStart == false || spawnstart == false || StageLv == 3 || Spawn_Level == 10) { return; }

        //순차적 스폰 로직

        //스폰 트랜스폼 순차적으로 돌게 작업 ( 개선전 버전 / 24.01.24 )
        //EnemyA_SpawnTrs = (int)Mathf.Repeat(EnemyA_SpawnTrs, spawn_PointTrs.Length);
        //EnemyB_SpawnTrs = (int)Mathf.Repeat(EnemyB_SpawnTrs, spawn_PointTrs.Length);

        // 현재 스폰지역의 넘버확인 
        //if (cheakEnemyAreaNumA > stageLv || cheakEnemyAreaNumA == -1)
        //{
        //     //EnemyA_SpawnTrs++;
        //}

        EnemyA_SpawnTrs = Random.Range(0, spawn_PointTrs.Length);
        EnemyB_SpawnTrs = Random.Range(0, spawn_PointTrs.Length);

        int cheakEnemyAreaNumA = spawn_PointTrs[EnemyA_SpawnTrs].AreaNumber;
        int cheakEnemyAreaNumB = spawn_PointTrs[EnemyB_SpawnTrs].AreaNumber;
        // A타입 스폰준비
        // 현재 해금 안된지역이라면 다음 으로 넘어감

        while (cheakEnemyAreaNumA > stageLv || cheakEnemyAreaNumA < stageLv || cheakEnemyAreaNumA == -1) // 계속 다시굴림
        {
            EnemyA_SpawnTrs = Random.Range(0, spawn_PointTrs.Length); // 위치선정
            cheakEnemyAreaNumA = spawn_PointTrs[EnemyA_SpawnTrs].AreaNumber; //위치 정보확인
        }
        
        if (cheakEnemyAreaNumA == stageLv) // 해금지역이라면 스폰
        {
            count[0] += Time.deltaTime;

            spawnTImeA = spawnData[playerAreaNum].interval[0] - lvCounUpData[Spawn_Level].IntervalDown;

            //기존 스폰시간 - 스폰레벨당 단축시간
            if (count[0] > spawnTImeA)
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

                spawnEnemyA = spawnData[playerAreaNum].count[0] + addCountA;

                //기본스폰데이터 카운트량  + 스폰레벨에 따른 추가증가량
                for (int i = 0; i < spawnEnemyA; i++)
                {
                    //스폰데이터의 넘버에 있는 몬스터 풀링
                    int PoolNum = spawnData[cheakEnemyAreaNumA].enemy_ID[0];
                    GameObject obj = pm.F_GetEnemyObj(PoolNum);
                    obj.transform.position = Vector3.zero;
                    obj.transform.position = spawn_PointTrs[EnemyA_SpawnTrs].transform.position
                                                               + new Vector3(Random.Range(0f, 0.1f), Random.Range(0f, 0.1f));
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
              
        while (cheakEnemyAreaNumB > stageLv || cheakEnemyAreaNumB < stageLv || cheakEnemyAreaNumB == -1) // 계속 다시굴림
        {
            EnemyB_SpawnTrs = Random.Range(0, spawn_PointTrs.Length);
            cheakEnemyAreaNumB = spawn_PointTrs[EnemyB_SpawnTrs].AreaNumber;
        }

        if (cheakEnemyAreaNumB == stageLv)
        {
            count[1] += Time.deltaTime;

            spawnTImeB = spawnData[playerAreaNum].interval[1] - lvCounUpData[Spawn_Level].IntervalDown;

            if (count[1] > spawnTImeB)
            {
                count[1] = 0;

                if (Spawn_Level == lvCounUpData.Length) // 추가로 풀링하는 갯수가 있는지  데이터에서 확인
                {
                    addCountB = 0;
                }
                else if (Spawn_Level < lvCounUpData.Length)
                {
                    addCountB = lvCounUpData[Spawn_Level].addCount;
                }

                spawnEnemyB = spawnData[playerAreaNum].count[1] + addCountB;  // 기존 횟수와 더해줌

                for (int i = 0; i < spawnEnemyB; i++)
                {
                    int poolID = spawnData[cheakEnemyAreaNumB].enemy_ID[1];
                    GameObject obj = pm.F_GetEnemyObj(poolID);
                    obj.transform.position = Vector3.zero;
                    obj.transform.position = spawn_PointTrs[EnemyB_SpawnTrs].transform.position + new Vector3(Random.Range(0f, 0.1f), Random.Range(0f, 0.1f));
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
        if (dealycount > startDealy)
        {
            spawnstart = true;
        }
    }


    //플레이어 장소기록
    public void F_PlayerAreaValue_Changer(int value)
    {
        playerAreaNum = value;
    }



    public void F_BlockTriggerOn(int value)
    {
        noEntryColl[value].isTrigger = true;
    }

    public void F_DeleteCloud(int value)
    {
        ParticleSystem[] firstDoorPs = CloudeGroup[value].GetComponentsInChildren<ParticleSystem>();

        while (firstDoorPs[1].startColor.a > 0)
        {
            firstDoorPs[0].startColor += new Color(0, 0, 0, -0.1f) * Time.deltaTime * 2;
            firstDoorPs[1].startColor += new Color(0, 0, 0, -0.1f) * Time.deltaTime * 2;
        }
        
    }

    public float F_Get_StageTime()
    {
        if (StageLv < 3)
        {
            return StageLevelupTime[stageLv];
        }
        else
        {
            return -1;
        }
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
    public float IntervalDown;
}

