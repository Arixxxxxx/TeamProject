using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCheakerSc : MonoBehaviour
{
    public enum AreaNumber
    {
        Field, Dengeon
    }
    [Header("# Only Read Cheak  in Area")]
    [SerializeField] GameObject Player;
    public AreaNumber areaNumber;
    [SerializeField] List<GameObject> NoSpawn = new List<GameObject>();
    void Start()
    {
        
    }

    private void Update()
    {
        if(Player != null)
        {
            SpawnManager.inst.F_PlayerAreaValue_Changer((int)areaNumber);
        }
        else
        {
            SpawnManager.inst.F_PlayerAreaValue_Changer(0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SpawnPoint") && NoSpawn.Contains(collision.gameObject) == false)
        {
            NoSpawn.Add(collision.gameObject);
            EnemySpawnPointSc sc = collision.GetComponent<EnemySpawnPointSc>();
            sc.F_Input_Area_Value((int)areaNumber);
        }

        if (collision.CompareTag("Player") && Player == null)
        {
            Player = collision.gameObject;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("SpawnPoint") && NoSpawn.Contains(collision.gameObject) == true)
        {
            NoSpawn.Remove(collision.gameObject);
            EnemySpawnPointSc sc = collision.GetComponent<EnemySpawnPointSc>();
            sc.F_Input_Area_Value(0);
        }

        if (collision.CompareTag("Player") && Player != null)
        {
            Player = collision.gameObject;
        }
    }
}
