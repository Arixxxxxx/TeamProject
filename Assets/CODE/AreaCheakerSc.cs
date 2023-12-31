using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCheakerSc : MonoBehaviour
{
    public enum AreaNumber
    {
        Stage0, Stage1, Stage2, Boss
    }
    [Header("# Only Read Cheak  in Area")]
    [SerializeField] GameObject Player;
    public AreaNumber areaNumber;
    [SerializeField] List<GameObject> spawnPointInArea = new List<GameObject>();
    void Start()
    {
        
    }

    private void Update()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SpawnPoint") && spawnPointInArea.Contains(collision.gameObject) == false)
        {
            spawnPointInArea.Add(collision.gameObject);
            EnemySpawnPointSc sc = collision.GetComponent<EnemySpawnPointSc>();
            sc.F_Input_Area_Value((int)areaNumber);
        }

        if (collision.CompareTag("Player") && Player == null)
        {
            SpawnManager.inst.F_PlayerAreaValue_Changer((int)areaNumber);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("SpawnPoint") && spawnPointInArea.Contains(collision.gameObject) == true)
        {
            spawnPointInArea.Remove(collision.gameObject);
        }

        if (collision.CompareTag("Player") && Player != null)
        {
            Player = null;
        }
    }
}
