using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Inst;


    [Header("# Insert Prefab Obj")]
    [Space]
    [SerializeField] GameObject[] Bullet; // 에너미 화살
    [SerializeField] int ArrowStartMakingEa;
    Queue<GameObject> ArrowQue = new Queue<GameObject>();
    [Space]
    [Space]
    [SerializeField] GameObject Exp_Coin; // 경험치 보석
    [SerializeField] int ExpCoin_StartMakingEa;
    Queue<GameObject> ExpCoinQue = new Queue<GameObject>();



    // Dyamic Tranform 변수
    Transform ArrowTrs, CoinTrs;
    
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

        // 1. Enemy 화살 관련
        ArrowTrs = transform.Find("Arrow").GetComponent<Transform>();

        for(int i = 0; i < ArrowStartMakingEa; i++)
        {
          GameObject Obj =  Instantiate(Bullet[0],ArrowTrs);
          Obj.transform.position = Vector3.zero;
          Obj.gameObject.SetActive(false);
          ArrowQue.Enqueue(Obj);
        }

        CoinTrs = transform.Find("ExpCoin").GetComponent<Transform>();

        // 2. Exp_Coin 관련

        for (int i = 0; i < ExpCoin_StartMakingEa; i++)
        {
            GameObject Obj = Instantiate(Exp_Coin, CoinTrs);
            Obj.transform.position = Vector3.zero;
            Obj.gameObject.SetActive(false);
            ExpCoinQue.Enqueue(Obj);
        }

    }
    void Start()
    {
        
    }

       void Update()
    {
        
    }

    /// <summary>
    /// [ Polling System ] 0 = Arrow / 1 = ExpCoin
    /// </summary>
    /// <param name="value"> 0 화살 / 1 Exp 코인 </param>
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
        }

        return null;
    }
    /// <summary>
    /// [ Polling System ] Return Obj !! Gameobject + RoomNumber (0화살/1경험치보석)
    /// </summary>
    /// <param name="value"> 0 화살 / 1 경험치 보석 / </param>
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

        }
    }
}
