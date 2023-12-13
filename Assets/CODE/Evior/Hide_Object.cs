using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hide_Object : MonoBehaviour
{
    [SerializeField] GameObject Player;
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
        if(Player != null && sr.color.a == 1)
        {
            sr.color = inPlayer;
            sr.sortingOrder = 10;
        }
        else if(Player == null) 
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
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Player != null)
        {
            Player = null;
        }
    }

}
