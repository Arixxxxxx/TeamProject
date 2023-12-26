using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Dragon : MonoBehaviour
{

    [SerializeField] float all_Speed;
    [SerializeField] float WaveSpeed;
    [SerializeField] float Waveheight;
    Rigidbody2D rb;
    Vector3 playerPos;

    [SerializeField] float PosX_Value;
    [SerializeField] float Add_Y;
    SpriteRenderer PlayerSr;
    SpriteRenderer DrangonSr;
    Transform PlayerTr;
    private void Awake()
    {
        PlayerSr = transform.parent.GetComponent<SpriteRenderer>();
        DrangonSr = transform.GetComponent<SpriteRenderer>();
        PlayerTr = transform.parent.GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        playerPos.x = PosX_Value;
        Debug.Log($"{PosX_Value} // {playerPos.x} // 플레이어 {PlayerTr.transform.position}");


        playerPos.y = Mathf.Sin(Time.time * WaveSpeed) * Waveheight;
        playerPos.y += Add_Y;
        rb.position = PlayerTr.transform.position + playerPos;

        //rb.position = playerPos;

    }
    void Update()
    {
        Dragon_Head_Direction_Updater();
    }

    private void Dragon_Head_Direction_Updater()
    {
        if(PlayerSr.flipX == true && PosX_Value < 0)
        {
            
            PosX_Value = PosX_Value  * - 1;
            DrangonSr.flipX = true;
        }
        else if (PlayerSr.flipX == false && PosX_Value > 0)
        {
            PosX_Value = PosX_Value * -1;
            DrangonSr.flipX = false;
        }
    }
}
