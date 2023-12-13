using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TelePortZone : MonoBehaviour
{

    public enum TeleportNum { _0, _1, _2}
    public TeleportNum type;

    [Header("# Cheak Enter Object")]
    [Space]
    [SerializeField] GameObject Player;
    
    
    bool once;
    CircleCollider2D circleCollider;
   
    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }

        void Update()
    {
        if(Player != null && once == false)
        {
            once = true;
            TeleportManager.inst.F_Change_Ather_TeleportZone(type, Player);
        }
        
        
         
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Player == null)
        {
            Player = collision.gameObject;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Player != null)
        {
            once = false;
            Player = null;

        }
    }
}
