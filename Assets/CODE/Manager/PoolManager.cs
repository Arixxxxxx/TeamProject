using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Inst;

    [Header("# Insert Prefab Enemy Obj")]
    [Space]
    [SerializeField] GameObject[] EnemyObj; // 
    [SerializeField] int Orc_melee_StartMakingEA;
    [SerializeField] int Mushroom_StartMakingEA;
    [SerializeField] int SkeletonRanager_StartMakingEA;
    [SerializeField] int Slime_StartMakingEA;
    [SerializeField] int OrcRanger_StartMakingEA;
    [SerializeField] int Tree_StartMakingEA;

    Queue<GameObject> OrcQue = new Queue<GameObject>();
    Queue<GameObject> MushRoomQue = new Queue<GameObject>();
    Queue<GameObject> SkeletonQue = new Queue<GameObject>();
    Queue<GameObject> SlimeQue = new Queue<GameObject>();
    Queue<GameObject> Orc_RangerQue = new Queue<GameObject>();
    Queue<GameObject> Tree_Que = new Queue<GameObject>();

    Transform OrcTrs, MushTrs, SkeletonTrs, SlimeTrs, OrcRangerTrs, TreeTrs;  // -> Transform




    [Header("# Insert Prefab Bullet Obj")]
    [Space]
    [SerializeField] GameObject[] Bullet; // 에너미 화살
    [SerializeField] int ArrowStartMakingEa;
    Queue<GameObject> ArrowQue = new Queue<GameObject>();

    [SerializeField] int Silme_PoisonStartMakingEa; //슬라임 독액
    Queue<GameObject> PoisonQue = new Queue<GameObject>();

    [SerializeField] int Silme_PoisonBadak_StartMakingEa; //바닥
    Queue<GameObject> PoisonBadakQue = new Queue<GameObject>();

    [SerializeField] int OrcStone_StartMakingEa;
    Queue<GameObject> OrcStoneQue = new Queue<GameObject>();

    [Space]
    [Space]
    [SerializeField] GameObject Exp_Coin; // 경험치 보석
    [SerializeField] int ExpCoin_StartMakingEa;
    Queue<GameObject> ExpCoinQue = new Queue<GameObject>();
    [Space]
    [Space]
    [SerializeField] GameObject Dmg_Font_Box; // 경험치 폰트 프리펩
    [SerializeField] int Dmg_Font_Box_StartMakingEa;
    Queue<GameObject> Dmg_Font_BoxQue = new Queue<GameObject>();




    [Header("# Insert Player Bullet Prefab Obj")]
    [Space]
    [SerializeField] GameObject[] PlayerBullet;

    Transform Skill_1_Trs;
    
    [SerializeField] int Skill_1_StartMakingEa;        
    Queue<GameObject> Skill_1_Que = new Queue<GameObject>();
    [SerializeField] int Skill_1_2_StartMakingEa;
    Queue<GameObject> Skill_1_2_Que = new Queue<GameObject>();

    Transform Skill_3_Trs;

    [SerializeField] int Skill_3_StartMakingEa;
    Queue<GameObject> Skill_3_Que = new Queue<GameObject>();
    [SerializeField] int Skill_3_2_StartMakingEa;
    Queue<GameObject> Skill_3_2_Que = new Queue<GameObject>();

    Transform Skill_4_Trs;

    [SerializeField] int Skill_4_StartMakingEa;
    Queue<GameObject> Skill_4_Que = new Queue<GameObject>();

    //[Header("# Insert ETC Prefab ")]
    //[Space]
    //[SerializeField] GameObject Skill_UI_Btn; // 레벨업 UI Btn
    //[SerializeField] int Skill_UI_Btn_StartMakingEa;
    //Queue<GameObject> Skill_UI_Btn_Que = new Queue<GameObject>();


    // Dyamic Tranform 변수
    Transform ArrowTrs, CoinTrs, FontTrs;

    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
        }
        else
        {
            Destroy(this);
        }



        // 1. Enemy Transform 

        OrcTrs = transform.Find("Enemy/Orc_Melee").GetComponent<Transform>();
        OrcRangerTrs = transform.Find("Enemy/Orc_Ranger").GetComponent<Transform>();
        MushTrs = transform.Find("Enemy/Mushroom").GetComponent<Transform>();
        SkeletonTrs = transform.Find("Enemy/Skeleton").GetComponent<Transform>();
        SlimeTrs = transform.Find("Enemy/Mushroom").GetComponent<Transform>();
        TreeTrs = transform.Find("Enemy/Tree").GetComponent<Transform>();

        CoinTrs = transform.Find("ExpCoin").GetComponent<Transform>();
        ArrowTrs = transform.Find("Arrow").GetComponent<Transform>();
        FontTrs = transform.Find("Font").GetComponent<Transform>();

        // 1. Player Transform 
        Skill_1_Trs = transform.Find("Player/Skill_1").GetComponent<Transform>();
        Skill_3_Trs = transform.Find("Player/Skill_3").GetComponent<Transform>();
        Skill_4_Trs = transform.Find("Player/Skill_4").GetComponent<Transform>();

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        // 풀링 초기생성 ////   Enemy
        /////////////////////////////////////////////////////////////////////////////////////////////////////

        for (int i = 0; i < Orc_melee_StartMakingEA; i++)
        {
            GameObject Obj = Instantiate(EnemyObj[0], OrcTrs);
            Obj.transform.position = Vector3.zero;
            Obj.gameObject.SetActive(false);
            OrcQue.Enqueue(Obj);
        }

        for (int i = 0; i < Mushroom_StartMakingEA; i++)
        {
            GameObject Obj = Instantiate(EnemyObj[1], MushTrs);
            Obj.transform.position = Vector3.zero;
            Obj.gameObject.SetActive(false);
            MushRoomQue.Enqueue(Obj);
        }

        for (int i = 0; i < SkeletonRanager_StartMakingEA; i++)
        {
            GameObject Obj = Instantiate(EnemyObj[2], SkeletonTrs);
            Obj.transform.position = Vector3.zero;
            Obj.gameObject.SetActive(false);
            SkeletonQue.Enqueue(Obj);
        }

        for (int i = 0; i < Slime_StartMakingEA; i++)
        {
            GameObject Obj = Instantiate(EnemyObj[3], SlimeTrs);
            Obj.transform.position = Vector3.zero;
            Obj.gameObject.SetActive(false);
            SlimeQue.Enqueue(Obj);
        }

        for (int i = 0; i < OrcRanger_StartMakingEA; i++)
        {
            GameObject Obj = Instantiate(EnemyObj[4], OrcRangerTrs);
            Obj.transform.position = Vector3.zero;
            Obj.gameObject.SetActive(false);
            Orc_RangerQue.Enqueue(Obj);
        }  
        
        for (int i = 0; i < Tree_StartMakingEA; i++)
        {
            GameObject Obj = Instantiate(EnemyObj[5], TreeTrs);
            Obj.transform.position = Vector3.zero;
            Obj.gameObject.SetActive(false);
            Tree_Que.Enqueue(Obj);
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////
        /// 오브젝트 초기생성

        for (int i = 0; i < ArrowStartMakingEa; i++) // 해골궁수 화살
        {
            GameObject Obj = Instantiate(Bullet[0], ArrowTrs);
            Obj.transform.position = Vector3.zero;
            Obj.gameObject.SetActive(false);
            ArrowQue.Enqueue(Obj);
        }

        for (int i = 0; i < Silme_PoisonStartMakingEa; i++) // 슬라임 독액
        {
            GameObject Obj = Instantiate(Bullet[1], ArrowTrs);
            Obj.transform.position = Vector3.zero;
            Obj.gameObject.SetActive(false);
            PoisonQue.Enqueue(Obj);
        }

        for (int i = 0; i < Silme_PoisonBadak_StartMakingEa; i++) // 슬라임 독액
        {
            GameObject Obj = Instantiate(Bullet[2], ArrowTrs);
            Obj.transform.position = Vector3.zero;
            Obj.gameObject.SetActive(false);
            PoisonBadakQue.Enqueue(Obj);
        }

        for (int i = 0; i < OrcStone_StartMakingEa; i++) // 돌맹이
        {
            GameObject Obj = Instantiate(Bullet[3], ArrowTrs);
            Obj.transform.position = Vector3.zero;
            Obj.gameObject.SetActive(false);
            OrcStoneQue.Enqueue(Obj);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        // 2. Exp_Coin 관련

        for (int i = 0; i < ExpCoin_StartMakingEa; i++)
        {
            GameObject Obj = Instantiate(Exp_Coin, CoinTrs);
            Obj.transform.position = Vector3.zero;
            Obj.gameObject.SetActive(false);
            ExpCoinQue.Enqueue(Obj);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        // 3. Dmg_Font  관련
        for (int i = 0; i < Dmg_Font_Box_StartMakingEa; i++)
        {
            GameObject Obj = Instantiate(Dmg_Font_Box, FontTrs);
            Obj.transform.position = Vector3.zero;
            Obj.gameObject.SetActive(false);
            Dmg_Font_BoxQue.Enqueue(Obj);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        // Player Bullet 

        for (int i = 0; i < Skill_1_StartMakingEa; i++) // Skill_01_Bullet
        {
            GameObject Obj = Instantiate(PlayerBullet[0], Skill_1_Trs);
            Obj.transform.position = Vector3.zero;
            Obj.gameObject.SetActive(false);
            Skill_1_Que.Enqueue(Obj);
        }

        for (int i = 0; i < Skill_1_2_StartMakingEa; i++) // Skill_01.2_Bome
        {
            GameObject Obj = Instantiate(PlayerBullet[1], Skill_1_Trs);
            Obj.transform.position = Vector3.zero;
            Obj.gameObject.SetActive(false);
            Skill_1_2_Que.Enqueue(Obj);
        }

        for (int i = 0; i < Skill_3_StartMakingEa; i++) // Skill_03_Bullet
        {
            GameObject Obj = Instantiate(PlayerBullet[2], Skill_3_Trs);
            Obj.transform.position = Vector3.zero;
            Obj.gameObject.SetActive(false);
            Skill_3_Que.Enqueue(Obj);
        }

        for (int i = 0; i < Skill_3_2_StartMakingEa; i++) // Skill_03_2_Bome
        {
            GameObject Obj = Instantiate(PlayerBullet[3], Skill_3_Trs);
            Obj.transform.position = Vector3.zero;
            Obj.gameObject.SetActive(false);
            Skill_3_2_Que.Enqueue(Obj);
        }

        for (int i = 0; i < Skill_4_StartMakingEa; i++) // Skill_04_Bullet
        {
            GameObject Obj = Instantiate(PlayerBullet[4], Skill_4_Trs);
            Obj.transform.position = Vector3.zero;
            Obj.gameObject.SetActive(false);
            Skill_4_Que.Enqueue(Obj);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        // UI  및 ETC 


    }
    void Start()
    {

    }

    void Update()
    {

    }

    /// <summary>
    /// 에너미 프리펩 Get 함수 -> 0오크/1버섯/2궁수 /3슬라임/4오크레인저
    /// </summary>
    /// <param name="value">0오크/1버섯/2궁수/3슬라임/4오크레인저/5나무</param>
    /// <returns></returns>
    public GameObject F_GetEnemyObj(int value)
    {
        GameObject obj;

        switch (value)
        {
            case 0: // 오크

                if (OrcQue.Count <= 1)
                {
                    obj = Instantiate(EnemyObj[0], OrcTrs);
                    return obj;
                }

                obj = OrcQue.Dequeue();

                return obj;


            case 1: // 버섯
                if (MushRoomQue.Count <= 1)
                {
                    obj = Instantiate(EnemyObj[1], MushTrs);
                    return obj;
                }
                obj = MushRoomQue.Dequeue();
                return obj;

            case 2: //해골궁수
                if (SkeletonQue.Count <= 1)
                {
                    obj = Instantiate(EnemyObj[2], SkeletonTrs);
                    return obj;
                }
                obj = SkeletonQue.Dequeue();
                return obj;

            case 3: // 슬라임
                if (SlimeQue.Count <= 1)
                {
                    obj = Instantiate(EnemyObj[3], SlimeTrs);
                    return obj;
                }
                obj = SlimeQue.Dequeue();
                return obj;

            case 4: // 오크레인저
                if (Orc_RangerQue.Count <= 1)
                {
                    obj = Instantiate(EnemyObj[4], OrcRangerTrs);
                    return obj;
                }
                obj = Orc_RangerQue.Dequeue();
                return obj;

            case 5: // 나무
                if (Tree_Que.Count <= 1)
                {
                    obj = Instantiate(EnemyObj[5], TreeTrs);
                    return obj;
                }
                obj = Tree_Que.Dequeue();
                return obj;
        }

        return null;
    }


    /// <summary>
    /// Enmey_return_Que
    /// </summary>
    /// <param name="obj"> GameObject </param>
    /// <param name="value"> 0 Orc / 1 Mushroom / 2 SkeletonRanger</param>
    public void F_Return_Enemy_Obj(GameObject obj, int value)
    {
        obj.SetActive(false);
        obj.transform.position = Vector3.zero;

        switch (value)
        {
            case 0:
                OrcQue.Enqueue(obj);
                break;

            case 1:
                MushRoomQue.Enqueue(obj);
                break;

            case 2:
                SkeletonQue.Enqueue(obj);
                break;

            case 3:
                SlimeQue.Enqueue(obj);
                break;

            case 4:
                Orc_RangerQue.Enqueue(obj);
                break;

            case 5:
               Tree_Que.Enqueue(obj);
                break;


        }

    }


    /// <summary>
    /// [ Polling System ] 0 화살 / 1 Exp 코인 / 2 데미지 폰트 // 3 슬라임 독액 // 4독액 바닥 //  5 돌맹이
    /// </summary>
    /// <param name="value"> 0 화살 / 1 Exp 코인 / 2 데미지 폰트 // 3 슬라임 독액 </param>
    /// <returns></returns>
    public GameObject F_GetObj(int value)
    {
        GameObject obj;

        switch (value)
        {
            case 0:
                if (ArrowQue.Count <= 1)
                {
                    GameObject Obj = Instantiate(Bullet[0], ArrowTrs);
                    Obj.transform.position = Vector3.zero;
                    Obj.gameObject.SetActive(false);
                    return Obj;
                }
                obj = ArrowQue.Dequeue();
                return obj;

            case 1:

                if (ExpCoinQue.Count <= 1)
                {
                    GameObject Obj = Instantiate(Exp_Coin, CoinTrs);
                    Obj.transform.position = Vector3.zero;
                    Obj.gameObject.SetActive(false);
                    return Obj;
                }

                obj = ExpCoinQue.Dequeue();
                return obj;

            case 2:  // 대미지폰트 박스
                if (Dmg_Font_BoxQue.Count <= 1)
                {

                    GameObject Obj = Instantiate(Dmg_Font_Box, FontTrs);
                    Obj.transform.position = Vector3.zero;
                    Obj.gameObject.SetActive(false);
                    return Obj;
                }

                obj = Dmg_Font_BoxQue.Dequeue();
                return obj;

            case 3:  // 슬라임 독액
                if (PoisonQue.Count <= 1)
                {
                    GameObject Obj = Instantiate(Bullet[1], ArrowTrs);
                    Obj.transform.position = Vector3.zero;
                    Obj.gameObject.SetActive(false);
                    return Obj;
                }

                obj = PoisonQue.Dequeue();
                return obj;

            case 4:  // 슬라임 독액 바닥
                if (PoisonBadakQue.Count <= 1)
                {
                    GameObject Obj = Instantiate(Bullet[2], ArrowTrs);
                    Obj.transform.position = Vector3.zero;
                    Obj.gameObject.SetActive(false);
                    return Obj;
                }

                obj = PoisonBadakQue.Dequeue();
                return obj;

            case 5:  // 돌맹이
                if (OrcStoneQue.Count <= 1)
                {
                    GameObject Obj = Instantiate(Bullet[3], ArrowTrs);
                    Obj.transform.position = Vector3.zero;
                    Obj.gameObject.SetActive(false);
                    return Obj;
                }

                obj = OrcStoneQue.Dequeue();
                return obj;
        }

        return null;
    }


    /// <summary>
    ///  0 = Skill01 Bullet // 1 = Skill01.2 Bome // 2 = 3번스킬  // 3 = 3-2 스킬(바닥) // 4 화염폭풍
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public GameObject F_GetPlayerBullet(int value)
    {
        GameObject obj = null;

        switch (value)
        {
            case 0:
                if (Skill_1_Que.Count <= 1)
                {
                    obj = Instantiate(PlayerBullet[0], Skill_1_Trs);
                    obj.transform.position = Vector3.zero;
                    obj.gameObject.SetActive(false);
                    return obj;
                }

                obj = Skill_1_Que.Dequeue();
                return obj;


            case 1:
                if (Skill_1_2_Que.Count <= 1)
                {
                    obj = Instantiate(PlayerBullet[1], Skill_1_Trs);
                    obj.transform.position = Vector3.zero;
                    obj.gameObject.SetActive(false);
                    return obj;
                }

                obj = Skill_1_2_Que.Dequeue();
                return obj;




            case 2:
                if (Skill_3_Que.Count <= 1)
                {
                    obj = Instantiate(PlayerBullet[2], Skill_3_Trs);
                    obj.transform.position = Vector3.zero;
                    obj.gameObject.SetActive(false);
                    return obj;
                }

                obj = Skill_3_Que.Dequeue();
                return obj;


            case 3:
                if (Skill_3_2_Que.Count <= 1)
                {
                    obj = Instantiate(PlayerBullet[3], Skill_3_Trs);
                    obj.transform.position = Vector3.zero;
                    obj.gameObject.SetActive(false);
                    return obj;
                }

                obj = Skill_3_2_Que.Dequeue();
                return obj;


            case 4:
                if (Skill_4_Que.Count <= 1)
                {
                    obj = Instantiate(PlayerBullet[4], Skill_4_Trs);
                    obj.transform.position = Vector3.zero;
                    obj.gameObject.SetActive(false);
                    return obj;
                }

                obj = Skill_4_Que.Dequeue();
                return obj;
        }
        return null;
    }

    /// <summary>
    /// 0 = Skill01 Bullet // 1 = Skill01.2 Bome // 2 = 3번스킬  // 3 = 3-2 스킬(바닥)
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    public void F_Return_PlayerBullet(GameObject obj, int value)
    {
        obj.SetActive(false);
        obj.transform.position = Vector3.zero;

        switch (value)
        {
            case 0:
                Skill_1_Que.Enqueue(obj);
                break;

            case 1:
                Skill_1_2_Que.Enqueue(obj);
                break;

            case 2:
                obj.transform.localScale = Vector3.one;
                Skill_3_Que.Enqueue(obj);
                break;

            case 3:
                Skill_3_2_Que.Enqueue(obj);
                break;  
            
            case 4:
                Transform L = obj.transform.Find("L").GetComponent<Transform>();
                Transform R = obj.transform.Find("R").GetComponent<Transform>();
                L.transform.localScale = Vector3.one;
                R.transform.localScale = Vector3.one;
                
                Skill_4_Que.Enqueue(obj);
                break;

        }

    }


    /// <summary>
    /// [ Polling System ]> 0 화살 / 1 경험치 보석 / 2 데미지폰트 / 3슬라임 독액 // 4바닥 // 5돌맹이
    /// </summary>
    /// <param name="value"> 0 화살 / 1 경험치 보석 / 2 데미지폰트 / 3슬라임 독액</param>
    /// <returns></returns>
    public void F_ReturnObj(GameObject obj, int value)
    {
        obj.SetActive(false);
        obj.transform.position = Vector3.zero;

        switch (value)
        {
            case 0:
                obj.transform.rotation = Quaternion.identity;
                ArrowQue.Enqueue(obj);
                break;

            case 1:
                ExpCoinQue.Enqueue(obj);
                break;

            case 2:
                Dmg_Font_BoxQue.Enqueue(obj);
                break;

            case 3:
                PoisonQue.Enqueue(obj);
                break;

            case 4:
                PoisonBadakQue.Enqueue(obj);
                break;

            case 5:
                OrcStoneQue.Enqueue(obj);
                break;


        }
    }
}
