using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public enum ItemType
    {
        Magnet, HP_Potion
    }
    public ItemType type;

    BoxCollider2D boxColl; // 획득용
    CircleCollider2D circleColl; //연출용

    //연출용 변수
    GameObject target;
    bool action0, action1;
    Vector3 pos;


    Player_Stats player_stats_sc;

    // 획득용 변수
    Exp_Coin[] exp;
    Rigidbody2D rb;
    [SerializeField] float stopRigidbodyDelay;
    float count;

    bool stopRigidbody;
    private void Awake()
    {
        boxColl = GetComponent<BoxCollider2D>();
        circleColl = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        
    }

    private void OnEnable()
    {
        switch (type)
        {
            case ItemType.Magnet:
                rb.AddForce(new Vector2(2f, 4), ForceMode2D.Impulse);
                break;
                case ItemType.HP_Potion:
                rb.AddForce(new Vector2(-2f, 4), ForceMode2D.Impulse);
                break;

        }
        
        Init();
    }
    void Start()
    {
        
    }
    private void FixedUpdate()
    {
        ActionMoving(); 
    }
    // Update is called once per frame
    void Update()
    {
        ActionOpen();
    }
    private void Init()
    {
        boxColl.enabled = false;
        circleColl.enabled = true;
        target = null;
        pos = Vector3.zero;
        action0 = false;
        action1 = false;
    }


    private void ActionOpen()
    {
        if (!stopRigidbody)
        {
            count += Time.deltaTime;
            if (count > stopRigidbodyDelay)
            {
                stopRigidbody = true;
                count = 0;
            }
        }

        if (stopRigidbody)
        {
            //rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            circleColl.enabled = true;
        }
    }

    private void ManetTrue()
    {
        exp = FindObjectsOfType<Exp_Coin>();

        if(exp.Length == 0) 
        {
             gameObject.SetActive(false);
        }
        else if(exp.Length >= 1) 
        {
            for(int i  = 0; i < exp.Length; i++)
            {
                if (exp[i].type == Exp_Coin.ItemType.ExpCoin)
                {
                    exp[i].F_magnetActive();
                }
            }

            PoolManager.Inst.F_ReturnItem(gameObject, 0);
        }
    }


    [SerializeField] float action0_Dur;
    // 당겨지는 속도
    [SerializeField] float inputSpeed;
    [SerializeField] float magnetSpeed;
    float count1;
    private void ActionMoving()
    {
        if (action0)
        {
            pos = (transform.position - target.transform.position).normalized;
            rb.AddForce(pos * 8f, ForceMode2D.Impulse);


            count1 += Time.deltaTime;
            if (count1 > action0_Dur)
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
            magnetSpeed += Time.deltaTime;

            Vector2 ppos = GameManager.Inst.F_Enemy_BulletTargetPos(transform.position);
            rb.MovePosition(rb.position + ppos * Time.deltaTime * magnetSpeed);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (circleColl.enabled == true && collision.CompareTag("Player"))
        {
            target = collision.gameObject;
            action0 = true;
            action1 = false;
            circleColl.enabled = false;
        }

        if (boxColl.enabled == true && collision.CompareTag("Player"))
        {
            boxColl.enabled = false;
            switch (type)
            {
                case ItemType.Magnet:
                    ManetTrue();
                    break;

                case ItemType.HP_Potion:
                    boxColl.enabled = false;
                    if (player_stats_sc == null)
                    {
                        player_stats_sc = collision.GetComponent<Player_Stats>();
                    }
                    player_stats_sc.F_Use_HP_Potion(25);
                    PoolManager.Inst.F_ReturnItem(gameObject, 1);
                    break;
            }
        }


     

    }
}
