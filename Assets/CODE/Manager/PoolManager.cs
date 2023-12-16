using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Inst;


    [Header("# Insert Prefab Obj")]
    [SerializeField] GameObject[] Bullet;
    [SerializeField] int ArrowStartMakingEa;
    Queue<GameObject> Arrow = new Queue<GameObject>();
    Transform ArrowTrs;

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


        ArrowTrs = transform.Find("Arrow").GetComponent<Transform>();

        for(int i = 0; i < ArrowStartMakingEa; i++)
        {
          GameObject Obj =  Instantiate(Bullet[0],ArrowTrs);
          Obj.transform.position = Vector3.zero;
          Obj.gameObject.SetActive(false);
          Arrow.Enqueue(Obj);
        }
    }
    void Start()
    {
        
    }

       void Update()
    {
        
    }

    /// <summary>
    /// [ Polling System ] 0 = Arrow
    /// </summary>
    /// <param name="value"> 0 화살 / </param>
    /// <returns></returns>
    public GameObject F_GetObj(int value)
    {
        GameObject obj;

        switch (value)
        {
            case 0:
                if(Arrow.Count <= 1)
                {
                    GameObject Obj = Instantiate(Bullet[0], ArrowTrs);
                    Obj.transform.position = Vector3.zero;
                    Obj.gameObject.SetActive(false);
                    return Obj;
                }
                obj = Arrow.Dequeue();
                return obj;
                
        }

        return null;
    }
    /// <summary>
    /// [ Polling System ] Return Obj !! Gameobject + RoomNumber
    /// </summary>
    /// <param name="value"> 0 화살 / </param>
    /// <returns></returns>
    public void F_ReturnObj(GameObject obj, int value)
    {
        obj.SetActive(false);
        obj.transform.position = Vector3.zero;
       
        switch (value)
        {
            case 0:
                obj.transform.rotation = Quaternion.identity;
                Arrow.Enqueue(obj);
                break;


        }
    }
}
