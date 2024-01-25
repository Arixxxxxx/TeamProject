using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager inst;


    [Header("# Spawn Setting  == >   # ����")]
    [Space]
    [SerializeField] int Spawn_Level; // ����
    [SerializeField] int stageLv; // ����
    
    [SerializeField] int[] AddCount; // �������� Ȥ�� �ð��� �����Ǵ� ���ͷ�
    [SerializeField] float startDealy;
    public int StageLv { get { return stageLv; } }
    [Header("# �������� ������ �ð�(��) �������  == >   # ����")]
    [Space]
    [SerializeField] List<int> StageLevelupTime = new List<int>(); // ������ �ð�
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


    // ���� ��ȯ�ؾ��� ����Ʈ
    int EnemyA_SpawnTrs;
    int EnemyB_SpawnTrs;


    [SerializeField] EnemySpawnPointSc[] spawn_PointTrs; // ��������Ʈ ����Ʈ

    // ���������� ���� ��������, ����, ī��Ʈ
    [SerializeField][Tooltip("��ũ,����,�ذ�ü�,������,��ũ������,����")] SpawnData[] spawnData;
    [SerializeField][Tooltip("�ٲ�ð� ,ī��Ʈ������, �������� ����")] LvCountUp[] lvCounUpData;

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
        GameTime = TimeSc.F_Get_GameTime(); // ���ӽð� üũ

        SpawnStart_Dealy();

        //SpawnStart(); ������ �ð�����
        New_SpawnStart();
        SpawnLvUpdater();
        Level_Updater();
        //MapInfoArrowActiveUpdater();

        if (Input.GetKeyDown(KeyCode.L)) // 2�� ������ �׽�Ʈ
        {
            F_BlockTriggerOn(2);
        }
     
    }
    /// <summary>
    /// �ð��� ���� ī��Ʈ ������
    /// </summary>
    [SerializeField] float nextLvTime;
    private void SpawnLvUpdater()
    {
        if (gm.MainGameStart == false || Spawn_Level == 10) { return; }

        if (Spawn_Level == lvCounUpData.Length) { return; } //�ִ� ���������� �������� ����

        nextLvTime = lvCounUpData[Spawn_Level].nextSpawnLvtime;

        spawnTimeCounter += Time.deltaTime;

        if (spawnTimeCounter > nextLvTime)
        {
            Spawn_Level++;
        }

    }

    bool arrowPopup0, arrowPopup1;

    /// <summary>
    /// ���ӽð� üũ�ؼ� ȭ��ǥ ���ִ� �Լ�
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
        // �������� 3������ ����
        if (timeListValue == 3) { return; }

        if (GameTime > StageLevelupTime[timeListValue])
        {
        
            if (timeListValue == 0) // �� Ʈ���� true (�հ� ���������հ�)
            {
                
                GameUIManager.Inst.F_GameInfoOpen(timeListValue); // �رݾȳ�����
                F_BlockTriggerOn(0); // Ʈ���� �۵�
                timeListValue++;
            }
            else if (timeListValue == 1)
            {
                // ���⼭ UI �رݵǾ��ٴ� Text���� �����������
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

    // ������ (�ð���� ������ => �ߵ� Ʈ���� ���� 24.01.24)
    //private void Level_Updater() 
    //{
    //    // �������� 3������ ����
    //    if (StageLv == 3) { return; }

    //    GameTime = TimeSc.F_Get_GameTime();

    //    if (GameTime > StageLevelupTime[stageLv])
    //    {
    //        stageLv++;
    //        CameraManager.inst.F_CameraZoomOut(stageLv); // ī�޶� �ܾƿ�
    //        GameUIManager.Inst.F_GameInfoOpen(stageLv);

    //        if (stageLv == 1) // ������
    //        {
    //            // ���⼭ UI �رݵǾ��ٴ� Text���� �����������

    //            F_BlockTriggerOn(0);
    //        }
    //        else if (stageLv == 2)
    //        {
    //            // ���⼭ UI �رݵǾ��ٴ� Text���� �����������
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

        //������ ���� ����

        //���� Ʈ������ ���������� ���� �۾� ( ������ ���� / 24.01.24 )
        //EnemyA_SpawnTrs = (int)Mathf.Repeat(EnemyA_SpawnTrs, spawn_PointTrs.Length);
        //EnemyB_SpawnTrs = (int)Mathf.Repeat(EnemyB_SpawnTrs, spawn_PointTrs.Length);

        // ���� ���������� �ѹ�Ȯ�� 
        //if (cheakEnemyAreaNumA > stageLv || cheakEnemyAreaNumA == -1)
        //{
        //     //EnemyA_SpawnTrs++;
        //}

        EnemyA_SpawnTrs = Random.Range(0, spawn_PointTrs.Length);
        EnemyB_SpawnTrs = Random.Range(0, spawn_PointTrs.Length);

        int cheakEnemyAreaNumA = spawn_PointTrs[EnemyA_SpawnTrs].AreaNumber;
        int cheakEnemyAreaNumB = spawn_PointTrs[EnemyB_SpawnTrs].AreaNumber;
        // AŸ�� �����غ�
        // ���� �ر� �ȵ������̶�� ���� ���� �Ѿ

        while (cheakEnemyAreaNumA > stageLv || cheakEnemyAreaNumA < stageLv || cheakEnemyAreaNumA == -1) // ��� �ٽñ���
        {
            EnemyA_SpawnTrs = Random.Range(0, spawn_PointTrs.Length); // ��ġ����
            cheakEnemyAreaNumA = spawn_PointTrs[EnemyA_SpawnTrs].AreaNumber; //��ġ ����Ȯ��
        }
        
        if (cheakEnemyAreaNumA == stageLv) // �ر������̶�� ����
        {
            count[0] += Time.deltaTime;

            spawnTImeA = spawnData[playerAreaNum].interval[0] - lvCounUpData[Spawn_Level].IntervalDown;

            //���� �����ð� - ���������� ����ð�
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

                //�⺻���������� ī��Ʈ��  + ���������� ���� �߰�������
                for (int i = 0; i < spawnEnemyA; i++)
                {
                    //������������ �ѹ��� �ִ� ���� Ǯ��
                    int PoolNum = spawnData[cheakEnemyAreaNumA].enemy_ID[0];
                    GameObject obj = pm.F_GetEnemyObj(PoolNum);
                    obj.transform.position = Vector3.zero;
                    obj.transform.position = spawn_PointTrs[EnemyA_SpawnTrs].transform.position
                                                               + new Vector3(Random.Range(0f, 0.1f), Random.Range(0f, 0.1f));
                    // A,B Ÿ���� ���ĳ����°��� ����=> ����ī��Ʈ

                    obj.gameObject.SetActive(true);
                    gm.SpawnCount++;
                }

                EnemyA_SpawnTrs++;
                addCountA = 0;
            }

        }

        // BŸ�� �����غ�
        //���� ����
              
        while (cheakEnemyAreaNumB > stageLv || cheakEnemyAreaNumB < stageLv || cheakEnemyAreaNumB == -1) // ��� �ٽñ���
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

                if (Spawn_Level == lvCounUpData.Length) // �߰��� Ǯ���ϴ� ������ �ִ���  �����Ϳ��� Ȯ��
                {
                    addCountB = 0;
                }
                else if (Spawn_Level < lvCounUpData.Length)
                {
                    addCountB = lvCounUpData[Spawn_Level].addCount;
                }

                spawnEnemyB = spawnData[playerAreaNum].count[1] + addCountB;  // ���� Ƚ���� ������

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


    //�÷��̾� ��ұ��
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

