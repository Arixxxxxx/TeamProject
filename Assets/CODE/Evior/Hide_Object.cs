using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide_Object : MonoBehaviour
{
    [SerializeField] List<GameObject> PlayerList = new List<GameObject>();
    [SerializeField] List<GameObject> EnemyList = new List<GameObject>();
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
        if(PlayerList.Count > 0 || EnemyList.Count >0 && sr.color.a == 1)
        {
            sr.color = inPlayer;
            sr.sortingOrder = 10;
        }
      
       
        if (EnemyList.Count == 0 && PlayerList.Count == 0)
        {
            sr.color = Color.white;
            sr.sortingOrder = 5;
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && PlayerList.Contains(collision.gameObject) == false  && collision.transform.position.y >= transform.position.y)
        {
                PlayerList.Add(collision.gameObject);
        }
        if (collision.CompareTag("Enemy") && EnemyList.Contains(collision.gameObject) == false && collision.transform.position.y >= transform.position.y)
        {
            EnemyList.Add(collision.gameObject);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && PlayerList.Contains(collision.gameObject))
        {
            PlayerList.Remove(collision.gameObject);
        }
        if (collision.CompareTag("Enemy") && EnemyList.Contains(collision.gameObject))
        {
            EnemyList.Remove(collision.gameObject);
        }
    }

}
