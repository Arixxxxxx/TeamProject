using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Dragon : MonoBehaviour
{
    [SerializeField] float Speed;
    [SerializeField] float WaveSpeed;
    [SerializeField] float Waveheight;
    Rigidbody2D rb;
    Vector2 playerPos;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        
        playerPos.x = transform.parent.position.x - 1;
        playerPos.y = Mathf.Sin(Time.time * WaveSpeed) * Waveheight;
        rb.position = playerPos;
        
    }
    void Update()
    {
      
    }
}
