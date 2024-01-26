using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCheakerSc : MonoBehaviour
{
    public enum AreaNumber
    {
        Stage1, Stage2, Stage3, PortalRoom, Boss, Side=-1
    }
    [Header("# Only Read Cheak  in Area")]
    [SerializeField] GameObject Player;
    public AreaNumber areaNumber;
    [SerializeField] List<GameObject> spawnPointInArea = new List<GameObject>();
    EnemySpawnPointSc sc;
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
            sc = collision.GetComponent<EnemySpawnPointSc>();
            sc.F_Input_Area_Value((int)areaNumber);
        }

        if (collision.CompareTag("Player") && Player == null && areaNumber != AreaNumber.Side)
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

        if (collision.CompareTag("Player") && Player != null && areaNumber != AreaNumber.Side)
        {
            Player = null;
        }
    }

    
}
