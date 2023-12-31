using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Exp_Coin : MonoBehaviour
{
    
    [SerializeField] Sprite[] CoinImage;
    [Header("# 스몰, 미디움, 라지 순으로 경험치 입력")]
    [SerializeField] float[] Exp_Value;
    [Header("# Drop Exp (Cheak!!)")]
    [SerializeField] float Exp;
    [SerializeField] float action0_Dis;
    [SerializeField] GameObject target;
    SpriteRenderer sr;
    BoxCollider2D boxColl;
    CircleCollider2D circleColl;
    Rigidbody2D rb;
    Vector3 startPos;
    Vector2 currentVelocity = Vector2.zero;
    private void Awake()
    {
        boxColl = GetComponent<BoxCollider2D>();
        circleColl = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }
   


    private void OnEnable()
    {
        Init();
    }
      
    void Start()
    {
        
    }
    private void Init()
    {
        boxColl.enabled = false;
        circleColl.enabled = true;
        target = null;
        startPos = transform.position;
        pos = Vector3.zero;
        action0 = false;
        action1 = false;
    }

    /// <summary>
    /// 코인생성시 여러가지 타입 셋팅
    /// </summary>
    /// <param name="value"> 0 스몰 / 1미디움 / 2라지</param>
    public void F_SettingCoin(int value)
    {
        if(sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }

        sr.sprite = CoinImage[value];
        Exp = Exp_Value[value];
    }

   

    private void FixedUpdate()
    {
        coinMoving();
    }

    [SerializeField] float action0_Dur;
    float count;
    float speed = 2;
    private void coinMoving()
    {
        if (action0) 
        {
            pos = (transform.position - target.transform.position).normalized ;
            rb.AddForce(pos * 3f, ForceMode2D.Impulse);
            

            count += Time.deltaTime;
            if(count > action0_Dur)
            {
                action0 = false;
                count = 0;
                target = null;
                rb.velocity = Vector3.zero;
                action1 = true;
            }
        }

        if (action1)
        {
            
            if (boxColl.enabled == false)
            {
                boxColl.enabled = true;
            }
            speed += Time.deltaTime;

            Vector2 ppos = GameManager.Inst.F_Enemy_BulletTargetPos(transform.position);
            //pos = (target.transform.position - transform.position).normalized;
            rb.MovePosition(rb.position +  ppos * Time.deltaTime * speed);

        }

    }


    [Header("#체크")]
    [SerializeField] bool action0, action1;
    bool once;
    [SerializeField] Vector3 pos;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //연출
        if (circleColl.enabled == true && collision.CompareTag("Player") && target == null)
        {
            target = collision.gameObject;
            action0 = true;
            action1 = false;
            circleColl.enabled = false;
        }


         //획득
        if (boxColl.enabled == true && collision.CompareTag("Player") && once == false && collision.GetComponent<Player_Stats>() != null)
        {
                boxColl.enabled = false;
                collision.GetComponent<Player_Stats>().F_GetExp_LevelupSystem(Exp);
                PoolManager.Inst.F_ReturnObj(gameObject, 1);
        }
        
    }
}
