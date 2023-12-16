using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hide_Object : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] GameObject Enemy;
    [SerializeField] float hide_color_A;
    [SerializeField] Color inPlayer;
    
    SpriteRenderer sr;
    
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        
    }

        void Update()
    {
        if(Player != null || Enemy != null && sr.color.a == 1)
        {
            sr.color = inPlayer;
            sr.sortingOrder = 10;
        }
      
       
        if (Enemy == null && Player == null)
        {
            sr.color = Color.white;
            sr.sortingOrder = 0;
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && Player == null)
        {
            Player = collision.gameObject;
        }
        if (collision.CompareTag("Enemy") && Enemy == null)
        {
            Enemy = collision.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && Enemy == null)
        {
            Enemy = collision.gameObject;
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Player != null)
        {
            Player = null;
        }
        if (collision.CompareTag("Enemy") && Enemy != null)
        {
            Enemy = null;
        }
    }

}
