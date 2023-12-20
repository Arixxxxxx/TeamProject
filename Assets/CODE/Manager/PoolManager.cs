using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Inst;

    [Header("# Insert Prefab Enemy Obj")]
    [Space]
    [SerializeField] GameObject[] EnemyObj; // 
    [SerializeField] int Enemy_Obj_01_StartMakingEA;
    [SerializeField] int Enemy_Obj_02_StartMakingEA;
    [SerializeField] int Enemy_Obj_03_StartMakingEA;
    Queue<GameObject> OrcQue = new Queue<GameObject>();
    Queue<GameObject> MushRoomQue = new Queue<GameObject>();
    Queue<GameObject> SkeletonQue = new Queue<GameObject>();
    Transform OrcTrs, MushTrs, SkeletonTrs;

    [Header("# Insert Prefab Bullet Obj")]
    [Space]
    [SerializeField] GameObject[] Bullet; // 에너미 화살
    [SerializeField] int ArrowStartMakingEa;
    Queue<GameObject> ArrowQue = new Queue<GameObject>();

    [SerializeField] int Silme_PoisonStartMakingEa;
    Queue<GameObject> PoisonQue = new Queue<GameObject>();

    [SerializeField] int Silme_PoisonBadak_StartMakingEa;
    Queue<GameObject> PoisonBadakQue = new Queue<GameObject>();

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


    // Dyamic Tranform 변수
    Transform ArrowTrs, CoinTrs, FontTrs;
    
    private void Awake()
    {
        if(Inst == null)
        {
            Inst = this;
        }
        else
        {
            Destroy(this);
        }
        // 1. Enemy Obj 초기생성


        // 1. Enemy 화살 관련
        OrcTrs = transform.Find("Enemy/Orc").GetComponent<Transform>();
        MushTrs = transform.Find("Enemy/Mushroom").GetComponent<Transform>();
        SkeletonTrs = transform.Find("Enemy/Skeleton").GetComponent<Transform>();
       
        CoinTrs = transform.Find("ExpCoin").GetComponent<Transform>();
        ArrowTrs = transform.Find("Arrow").GetComponent<Transform>();
        FontTrs = transform.Find("Font").GetComponent<Transform>();


        for (int i = 0; i < Enemy_Obj_01_StartMakingEA; i++)
        {
            GameObject Obj = Instantiate(EnemyObj[0], OrcTrs);
            Obj.transform.position = Vector3.zero;
            Obj.gameObject.SetActive(false);
            OrcQue.Enqueue(Obj);
        }


        for (int i = 0; i < Enemy_Obj_02_StartMakingEA; i++)
        {
            GameObject Obj = Instantiate(EnemyObj[1], MushTrs);
            Obj.transform.position = Vector3.zero;
            Obj.gameObject.SetActive(false);
            MushRoomQue.Enqueue(Obj);
        }

        for (int i = 0; i < Enemy_Obj_03_StartMakingEA; i++)
        {
            GameObject Obj = Instantiate(EnemyObj[2], SkeletonTrs);
            Obj.transform.position = Vector3.zero;
            Obj.gameObject.SetActive(false);
            SkeletonQue.Enqueue(Obj);
        }

        for (int i = 0; i < ArrowStartMakingEa; i++) // 해골궁수 화살
        {
          GameObject Obj =  Instantiate(Bullet[0],ArrowTrs);
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

        // 2. Exp_Coin 관련

        for (int i = 0; i < ExpCoin_StartMakingEa; i++)
        {
            GameObject Obj = Instantiate(Exp_Coin, CoinTrs);
            Obj.transform.position = Vector3.zero;
            Obj.gameObject.SetActive(false);
            ExpCoinQue.Enqueue(Obj);
        }

        // 3. Dmg_Font  관련
        for (int i = 0; i < Dmg_Font_Box_StartMakingEa; i++)
        {
            GameObject Obj = Instantiate(Dmg_Font_Box, FontTrs);
            Obj.transform.position = Vector3.zero;
            Obj.gameObject.SetActive(false);
            Dmg_Font_BoxQue.Enqueue(Obj);
        }

    }
    void Start()
    {
        
    }

       void Update()
    {
        
    }

    /// <summary>
    /// 에너미 프리펩 Get 함수 -> 0오크/1버섯/2궁수 
    /// </summary>
    /// <param name="value">0오크/1버섯/2궁수</param>
    /// <returns></returns>
    public GameObject F_GetEnemyObj(int value)
    {
        GameObject obj;

        switch(value)
        {
            case 0: // 오크

                if(OrcQue.Count <= 1)
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
                if (obj.transform.parent != OrcTrs)
                {
                    obj.transform.SetParent(OrcTrs);
                }
                OrcQue.Enqueue(obj);
                break;

                case 1:
                if (obj.transform.parent != MushTrs)
                {
                    obj.transform.SetParent(OrcTrs);
                }
                MushRoomQue.Enqueue(obj);
                break;

                case 2:
                if (obj.transform.parent != SkeletonTrs)
                {
                    obj.transform.SetParent(OrcTrs);
                }
                SkeletonQue.Enqueue(obj);   
                break;

        }

    }


    /// <summary>
    /// [ Polling System ] 0 화살 / 1 Exp 코인 / 2 데미지 폰트 // 3 슬라임 독액
    /// </summary>
    /// <param name="value"> 0 화살 / 1 Exp 코인 / 2 데미지 폰트 // 3 슬라임 독액 </param>
    /// <returns></returns>
    public GameObject F_GetObj(int value)
    {
        GameObject obj;

        switch (value)
        {
            case 0:
                if(ArrowQue.Count <= 1)
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

        }

        return null;
    }

    /// <summary>
    /// [ Polling System ]> 0 화살 / 1 경험치 보석 / 2 데미지폰트 / 3슬라임 독액
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

        }
    }
}
