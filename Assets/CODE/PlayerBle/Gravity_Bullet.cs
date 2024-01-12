using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity_Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float Force;
    [SerializeField] float veloY;
    [SerializeField] float RanX, RanY;
    [SerializeField] bool CheakBullet;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    void Start()
    {
        //CheakBullet = false;
        //Set_RandomValue();
        //StartCoroutine(CheakVeloY());
    }

    private void OnEnable()
    {
        if(CheakBullet == true)
        {
            CheakBullet = false;
        }
        Set_RandomValue();
        StartCoroutine(CheakVeloY());

    }

    IEnumerator CheakVeloY()
    {
        InIt();

        rb.AddForce(new Vector2(RanX, RanY) * Force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.1f);

        veloY = rb.velocity.y;
                
        yield return new WaitForSeconds(0.1f);
        veloY *= -1;
        float RanYValue = Random.Range(-6f, 2f);
        veloY = veloY + RanYValue;

        yield return null;
        CheakBullet = true;
    }
    
    private void InIt()
    {
        rb.velocity = Vector2.zero;
        CheakBullet = false;
    }
    void Update()
    {
        
        transform.up = rb.velocity;
        //rb.MoveRotation(Quaternion.LookRotation(rb.velocity) * Quaternion.Euler(0, 0, -90));
        // 이동하는 방향으로회전 // 이동하는 방향 넣었던 // 화살의 이미지 자체가 잘못되어서 90도 꺽음

        if (rb.velocity.y < veloY && CheakBullet == true)
        {
            GameObject obj = PoolManager.Inst.F_GetPlayerBullet(3);
            obj.transform.position = transform.position + new Vector3(0,-0.3f);
            obj.SetActive(true);

            PoolManager.Inst.F_Return_PlayerBullet(gameObject, 0);
        }
        
    }
    
    private void Set_RandomValue()
    {
        int randomX = Random.Range(0, 2);
        int randomY = Random.Range(0, 2);

        if (randomX == 0)
        {
            RanX = Random.Range(-4.0f, -7.0f);
        }
        else if (randomX == 1)
        {
            RanX = Random.Range(4.0f, 7.0f);
        }

        RanY = Random.Range(6.0f, 8.5f);
    }
}
