using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{

    public static TeleportManager inst;

    [SerializeField] TelePortZone[] zone;
    WaitForSeconds TeleportDelay;
   [SerializeField] float ColliderDealyTime;


    private void Awake()
    {
        if(inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }

    }
    private void Start()
    {
        TeleportDelay = new WaitForSeconds(ColliderDealyTime);
    }


    /// <summary>
    /// 플레이어 랜덤 위치이동
    /// </summary>
    /// <param name="Value"></param>
    /// <param name="Player"></param>
    public void F_Change_Ather_TeleportZone(TelePortZone.TeleportNum Value, GameObject Player)
    {
        int RandomValue = (int)Value;

        if (RandomValue == 0)
        {
            RandomValue = 1;
            StartCoroutine(ColliderOnOff(1));
        }
        else
        {
            RandomValue = 0;
            StartCoroutine(ColliderOnOff(0));
        }


      

        Player.transform.position = zone[RandomValue].transform.position;
    }

   
    IEnumerator ColliderOnOff(int value)
    {
        zone[value].GetComponent<CircleCollider2D>().enabled = false;
        yield return TeleportDelay;
        zone[value].GetComponent<CircleCollider2D>().enabled = true;
    }
}
