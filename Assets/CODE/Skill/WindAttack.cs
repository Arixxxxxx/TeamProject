using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAttack : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] Rigidbody2D PlayerRb;
    [SerializeField] float Force;
    [SerializeField] bool isAttack;
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (Player != null)
        {
            PlayerRb.AddForce(Vector2.right * Force, ForceMode2D.Force);
        }
    }
    // Update is called once per frame
    void Update()
    {
       
        
    }

   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && Player == null)
        {
            Player = collision.gameObject;
            PlayerRb = collision.GetComponent<Rigidbody2D>();
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
