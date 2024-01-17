using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkCloud : MonoBehaviour
{

    [Header("# DarkCloud Set Value")]
    [Space]
    [SerializeField] float attackInterval;
    [SerializeField] float DMG;
    [SerializeField] GameObject player;
    Player_Stats sc;
    float count;    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInDarkClodeDMG();
    }

    private void PlayerInDarkClodeDMG()
    {
        if(player != null) 
        {
            count += Time.deltaTime;

            if(count > attackInterval)
            {
                count = 0;
                sc.F_Player_On_Hit(DMG);
                GameUIManager.Inst.F_SetDarkCloudWindow_OnOff(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && player == null && collision.gameObject.name == "Player_W" || collision.gameObject.name == "Player_M")
        {
            if (sc == null)
            {
                sc = collision.GetComponent<Player_Stats>();
            }

            player = collision.gameObject;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && player != null && collision.gameObject.name == "Player_W" || collision.gameObject.name == "Player_M")
        {
            player = null;
            GameUIManager.Inst.F_SetDarkCloudWindow_OnOff(false);
        }
    }
}
