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
        int RandomValue = Random.Range(0, zone.Length);

        if (RandomValue == (int)Value)
        {
            F_Change_Ather_TeleportZone(Value, Player);
            return;
        }


        StartCoroutine(ColliderOnOff(RandomValue));

        Player.transform.position = zone[RandomValue].transform.position;
    }

   
    IEnumerator ColliderOnOff(int value)
    {
        zone[value].GetComponent<CircleCollider2D>().enabled = false;
        yield return TeleportDelay;
        zone[value].GetComponent<CircleCollider2D>().enabled = true;
    }
}
