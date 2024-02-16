using NavMeshPlus.Components;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class GameManager : MonoBehaviour
{
    public static GameManager Inst;

   

    NavMeshSurface nav_Surface;
    [Header("# Insert Object in Hiearachy")]
    [Space]
    [SerializeField] GameObject playerParent;
    GameObject Player; // ������ ��ġ ���� ����(Ÿ������Ʈ��)
    GameObject PlayerObj; //
    Player_Stats player_stats_sc;
    [SerializeField] Light2D globalLight;
    [SerializeField] float light_Change_Speed;
    [SerializeField] GameObject[] GameStop_Ui_Window;
    [SerializeField] GameObject[] moveStop_Ui_Windows;
    [SerializeField] Button[] Btn;
    [SerializeField] GameObject enemyList;
    bool isPlayer_Dead;
    bool isActionReady;
    bool testMode;
    GameObject opengController;
    public bool IsActionReady {  get { return isActionReady; } set { isActionReady = value; } }

    [Header("# Ingame Cheak & Test Value")]
    [Space]
    [Space]
    [SerializeField] bool mainGameStart;
    [SerializeField] float Start_GetSkill_WaitSec;
    WaitForSeconds startWaitTime;
    [Space]
    [SerializeField] bool uiOpen_EveryObecjtStop;
    
    [Header("# Battle Count")]
    [Space]
    [SerializeField] int battleTime;
    [SerializeField] int bossKillCount;
    [SerializeField] int totalkillCount;
    [SerializeField] int totalLvUpCount;
    [SerializeField] int playerDeadCount;
    
    // ���� ������
    public int TotalBattleTime;
    public int TotalPlayerDead;

    [SerializeField] int killCount;
    [SerializeField] int lvUpCount;
    [Space]
    [Header("# Dark Cloud Value Setting")]
    [Space]
    [SerializeField] ParticleSystem darkCloudeObj;
    public bool MainGameStart { get { return mainGameStart; } set { mainGameStart = value; } }
    public bool UiOpen_EveryObecjtStop { get { return uiOpen_EveryObecjtStop; } set { uiOpen_EveryObecjtStop = value; } }
    public bool IsPlayer_Dead { get { return isPlayer_Dead; } set { isPlayer_Dead = value; } }
    public int KillCount { get { return killCount; } set { killCount = value; } }
    public int BossKillCount { get { return bossKillCount; } set { bossKillCount = value; } }
    public int TotalLvUpCount { get { return totalLvUpCount; } set {  totalLvUpCount = value; } }
    public int TotalkillCount { get { return totalkillCount; } set { totalkillCount = value; } }

        
    [SerializeField] int FieldEnemy;

    int sceneNumber;
    [Header("# Boss Room Set Cheking")]
    [Space]
        // �����̵��� �ʿ��� ������
    [SerializeField] SpriteRenderer[] playerAndDragon;
    [SerializeField] ParticleSystem telePortPs;
    [SerializeField] float Action0_Dleay;
    [SerializeField] Transform[] tellPoint;
    

    [SerializeField]  bool enterBossRoom;
    [SerializeField]  bool boosBattleStart;

    private UserGameInfo userDataForSeverUpload = new UserGameInfo(0,0,0,0,0);
   
    public bool BossBattleStart { get { return boosBattleStart; } set { boosBattleStart = value; } }
    public bool EnterBossRoom { get { return enterBossRoom; } set { enterBossRoom = value; } }
    bool bossMode;
    public bool BossMode { get { return bossMode; } set { bossMode = value; } } 

    bool bossDead;
    public bool BossDead { get { return bossDead; } set { bossDead = value; } }


    [Header("# For MoveStop")]
    [Space]
    [SerializeField] bool moveStop;
    public bool MoveStop { get { return moveStop; } set { moveStop = value; } }

    public bool isRun;



    
    private void Awake()
    {

        Application.targetFrameRate = 60;

        if(Inst == null)
        {
            Inst = this;
        }
        else
        {
            Destroy(this);
        }

        opengController = transform.parent.Find("OpeningController").gameObject;
        if(opengController.activeSelf == false)
        {
            testMode = true;
        }

    }


    
    
    public void F_Set_PlayerStatsSc(Player_Stats value)
    {
        player_stats_sc = value;
        PlayerObj = playerParent.transform.Find("Player_W").gameObject;
        Player = playerParent.transform.Find("Player_W/TargetPoint").gameObject;
    }
    void Start()
    {
        
        startWaitTime = new WaitForSeconds(Start_GetSkill_WaitSec);

        NavMapBake_init();
        sceneNumber = SceneManager.GetActiveScene().buildIndex;

        //�����̵� ������
        playerAndDragon = new SpriteRenderer[2];
        playerAndDragon[0] = playerParent.transform.Find("Player_W").GetComponent<SpriteRenderer>();
        playerAndDragon[1] = playerAndDragon[0].transform.Find("Dragon").GetComponent<SpriteRenderer>();
        telePortPs = playerAndDragon[0].transform.Find("PS/Teleport").GetComponent<ParticleSystem>();

        teleportDelay = new WaitForSeconds(telDelay);
        bossMapAction0_Delay = new WaitForSeconds(Action0_Dleay);

       
        if(testMode == false)
        {
            // ���������� ���� ���ӿ� �ʱ�ȭ
            UserGameInfo userdata = DataManager.inst.F_GetUserData();

            TotalBattleTime = userdata.battleTime;
            BossKillCount = userdata.bossKillCount;
            TotalkillCount = userdata.totalKillEnemy;
            TotalLvUpCount = userdata.LevelUpCount;
            TotalPlayerDead = userdata.playerDeadCount;
        }
        

    }

    
    bool once;
    void Update()
    {
        if(MainGameStart && !once)
        {
            once = true;
            GameUIManager.Inst.F_GameInfoOpen(-1);
            Invoke("OpeningMSG", 6);
        }

        battleTime = UnitFrame_Updater.inst.F_GetBattleTime();


        StartGameGetLVUP();

        //60�ʰ��� �ڵ����� ������
        if (testMode == false && EnterBossRoom == false)
        {
            AutoUploadData();
        }

    }

    // �ڵ� ����

    float counter;
    [SerializeField] float AutoSeverSaveTime;

    // ������ ���۵ǰų� ������ �ױ��� �ڵ� ���̺� (����)
    private void AutoUploadData()
    {
        
        if (MainGameStart == false || BossDead == true)   { return; }

        counter += Time.deltaTime;

        if(counter > AutoSeverSaveTime)
        {
            counter = 0;
            F_Manual_SaveGame();
        }
    }

    public void F_Manual_SaveGame()
    {
        GameUIManager.Inst.F_SaveIconPopup();

        TotalBattleTime += battleTime;
        TotalPlayerDead += playerDeadCount;

        userDataForSeverUpload.battleTime = TotalBattleTime;
        userDataForSeverUpload.bossKillCount = BossKillCount;
        userDataForSeverUpload.totalKillEnemy = TotalkillCount;
        userDataForSeverUpload.LevelUpCount = TotalLvUpCount;
        userDataForSeverUpload.playerDeadCount = TotalPlayerDead;

        Debug.Log($"{userDataForSeverUpload.battleTime} / {userDataForSeverUpload.playerDeadCount}");
        Debug.Log($"{userDataForSeverUpload.bossKillCount} / {userDataForSeverUpload.totalKillEnemy} / {userDataForSeverUpload.LevelUpCount}");
        DataManager.inst.F_SaveGameAndServerUpload(userDataForSeverUpload);
    }
    /// <summary>
    /// Timescale Changer / include = Character MoveStop
    /// </summary>
    /// <param name="value"></param>
    public void F_TimeSclaeController(bool value)
    {

        if (value && Time.timeScale == 1) 
        {
            Time.timeScale = 0;
            MoveStop = true;
        }
        else if(!value && Time.timeScale == 0)
        {
            Time.timeScale = 1;
            MoveStop = false;
        }
    }
    private void OpeningMSG()
    {
        GameUIManager.Inst.F_SetMSGUI(0,true);
    }
    bool start_once;
    private void StartGameGetLVUP()
    {
        if (mainGameStart == true && start_once == false)
        {
            start_once = true;
            StartCoroutine(StartLvUP());
        }
    }
    IEnumerator StartLvUP()
    {
        yield return startWaitTime;

        LvUp_Ui_Manager.Inst.F_LvUP_SelectSkill();
    }


    private void NavMapBake_init()
    {
        nav_Surface = GetComponent<NavMeshSurface>();
        nav_Surface.BuildNavMeshAsync();
    }

    public Vector2 F_Get_PlayerObj()
    {
        return Player.transform.position;
    }

    

    public Player_Stats F_Get_PlayerSc()
    {
        return player_stats_sc;
    }

    public float F_Get_PlyerPos(Vector3 InputPos)
    {
        float dis;
        dis = Vector3.Distance(Player.transform.position, InputPos);
        return dis;
    }

    /// <summary>
    /// �÷��̾�� ���ʹ� ��ġ X�� ���
    /// </summary>
    /// <param name="InputPos"></param>
    /// <returns></returns>

    public float F_Get_Filpx_Value(Vector3 InputPos)
    {
        float dis;
        dis = InputPos.x - Player.transform.position.x;
        return dis;
    }

    /// <summary>
    /// ����ü �÷��̾���� ���
    /// </summary>
    /// <param name="StartPos"></param>
    /// <returns></returns>
    public Vector2 F_Enemy_BulletTargetPos(Vector3 StartPos)
    {
        Vector2 Target = Player.transform.position - StartPos;
        return Target;
    }


    public float F_PlayerAndMeDistance(Vector2 StartPos)
    {
        float Dis = Vector2.Distance(Player.transform.position, StartPos);
        return Dis;
    }
    /// <summary>
    /// ����ü �������
    /// </summary>
    /// <param name="StartPos"></param>
    /// <returns></returns>
    public float F_EnemyBulletRotation(Vector3 StartPos)
    {
        Vector3 Target = Player.transform.position - StartPos;
        float angle = Mathf.Atan2(Target.y, Target.x) * Mathf.Rad2Deg;
        return angle;
    }

    public void F_KillCountUp()
    {
        killCount++;
        TotalkillCount++;
    }

    public void LvUpCountUP()
    {
        lvUpCount++;
        TotalLvUpCount++;
    }


  
    /// <summary>
    /// ��ų���� ���� ������ ����
    /// </summary>
    public void F_Lvup_Slot_Reset()
    {
        
        for (int i = 0; i < Btn.Length; i++)
        {
            if(i > 0)
            {
                Btn[i].transform.parent.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// ��ư Ŭ���� 0 : ��Ŵ� / 1 : Ǯ����
    /// </summary>
    /// <param name="value"></param>
    public void F_Lvup_Btn_OnOff(int value)
    {
        if(value == 0)
        {
            for(int i = 0;i < Btn.Length;i++)
            {
                Btn[i].interactable = false;
            }
        }
        else if (value == 1)
        {
            for (int i = 0; i < Btn.Length; i++)
            {
                Btn[i].interactable = true;
            }
        }
    }

    /// <summary>
    /// ���� ��ü ����
    /// </summary>
    public void F_ActiveBomb()
    {
        EnemyStats[] group = enemyList.GetComponentsInChildren<EnemyStats>();
        // ������ ȿ�� �ֱ�
        for (int i = 0; i < group.Length; i++)
        {
            if (group[i].gameObject.activeSelf == true)
            {
                group[i].F_Enemy_On_Hit(1000, true, true);
            }
        }
    }
    public void F_MainUI_SetAcvite_True()
    {
        GameStop_Ui_Window[2].SetActive(true);
    }

  

    bool waitCorutine;
    public void TelePort(int value)
    {
        if(waitCorutine == true) { return; }

        waitCorutine = true;

        StartCoroutine(tell(value));
    }

    WaitForSeconds teleportDelay;
    WaitForSeconds bossMapAction0_Delay;
    [SerializeField] float telDelay;
    IEnumerator tell(int value)
    {
        telePortPs.gameObject.SetActive(true);
        yield return null;

        playerAndDragon[0].enabled = false;
        playerAndDragon[1].gameObject.SetActive(false);

        if (TotorialCountStart)
        {
            TutoCount++;
        }

        switch (value) 
        {
            case 0: // ��ų �ڷ���Ʈ��
        
                yield return teleportDelay;
                playerAndDragon[0].enabled = true;
                playerAndDragon[1].gameObject.SetActive(true);
                break;

            case 1:
                
                GameUIManager.Inst.SkillEffectStop = true;
                LvUp_Ui_Manager.Inst.F_PlayerRunSounStop(); // Ȥ�� �ٰ��մٸ� �Ҹ�����
                SoundManager.inst.F_Get_ControllSoundPreFabs_ETC_PlaySFX(2,1);
                MoveStop = true;
                GameUIManager.Inst.F_GameUIActive(false);
                playerAndDragon[0].transform.Find("Shadow").gameObject.SetActive(false);
                yield return new WaitForSeconds(0.3f);
                SoundManager.inst.F_Bgm_Player(5, 0.25f, 1);
                Cutton_Controller.inst.F_FadeCuttonActive(1.8f);
                yield return new WaitForSeconds(1f);

                // �÷��̾� ��ġ�̵�
                EnterBossRoom = true;
                playerParent.transform.Find("Player_W").gameObject.transform.position = tellPoint[0].transform.position;

                for (int i = 1; i <playerAndDragon.Length; i++) // �÷��̾� �� �巹�� �������� ����
                {
                    playerAndDragon[i].flipX = false;
                }
                 // ���������

                F_ActiveBomb(); // Ÿ�鼭 �����մ� ���� ���� ����������

                GlobalLightController.Inst.F_LightControl(3); // ���� ��Ʈ��

                // �̵�

                yield return new WaitForSeconds(2.1f);
                telePortPs.gameObject.SetActive(true);
                SoundManager.inst.F_Get_ControllSoundPreFabs_ETC_PlaySFX(2, 1);
                yield return new WaitForSeconds(0.2f);
                
                playerAndDragon[0].enabled = true;
                playerAndDragon[1].gameObject.SetActive(true);
                playerAndDragon[0].transform.Find("Shadow").gameObject.SetActive(true);
                yield return new WaitForSeconds(1.5f);
                GameUIManager.Inst.F_SetMSGUI(1, false);
                yield return new WaitForSeconds(4f);
                
                MoveStop = false; // ��ŸƮ
                GameUIManager.Inst.SkillEffectStop = false;
                // �����˾��� �ʿ��� �Լ��� ȣ��
                GameUIManager.Inst.F_GameUIActive(true);
                CameraManager.inst.F_CameraDirectZoomOut(12); // ī�޶� �ܾƿ�
                GameUIManager.Inst.F_BossHpBarActive(true); // ���� ����ġ��
                BossBattleStart = true;
                //������������ ȣ��
                //��ȣ�� ������
                //��ü UI �����ٰ� �����°� ��
                break;
        }



       
        waitCorutine = false;
    }

    public Transform F_GetPalyerTargetPoint()
    {
        return Player.transform;
    }
    public void PlayerHP_Recovery()
    {
        player_stats_sc.F_CurrentHPFull();
    }

  public void PlayerRespawn_Mujuk()
    {
        player_stats_sc.F_RespawnMujuk();
    }


    public Player_Stats F_GetPlayerStats_Script()
    {
        return player_stats_sc;
    }

    public void F_PlayerTransformMove(Vector3 Pos)
    {
        player_stats_sc.gameObject.transform.position = Pos;
    }

    public void F_PlayerObjPositionMove(Transform trs)
    {
        PlayerObj.transform.position = trs.position;
    }

    public void F_NextScene(int value)
    {
        SceneManager.LoadScene(value);
    }

    /// <summary>
    /// Ʃ�丮��� ���� ����
    /// </summary>
    bool tutorialCountStart;
    public bool TotorialCountStart { get {  return tutorialCountStart; } set {  tutorialCountStart = value; } }

    int TutoCount;
   
    public int GetTutorialTeleportCounter()
    {
        return TutoCount;
    }

    /// <summary>
    /// ���� ������ �޾ư��� �Լ�
    /// </summary>
    /// <returns></returns>
    public int[] F_GetUserGameCountData()
    {
        int[] Result_value = new int[3];

        Result_value[0] = BossKillCount;
        Result_value[1] = TotalkillCount;
        Result_value[2] = TotalLvUpCount;

        return Result_value;
    }
    public void F_PlayerDeadUp()
    {
        playerDeadCount++;
    }
}
