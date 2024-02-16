using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public enum ItemType
    {
        Magnet, HP_Potion, Bomb
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
    [SerializeField] bool stopRigidbody;

    float deafultGravityScale;
    float count;

    private void Awake()
    {
        boxColl = GetComponent<BoxCollider2D>();
        circleColl = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        deafultGravityScale = rb.gravityScale;
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

            case ItemType.Bomb:
                stopRigidbody = true;
                break;

        }
        
        Init();
    }
    void Start()
    {
        player_stats_sc = GameManager.Inst.F_GetPlayerStats_Script();
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
        stopRigidbody = false;
        rb.gravityScale = deafultGravityScale;
    }

    bool firstTouch;
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
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            circleColl.enabled = true;

            switch (type)
            {
                case ItemType.Magnet:
                case ItemType.HP_Potion:
                    Invoke("firstTouchOn", 0.3f);
                    break;

                case ItemType.Bomb:
                    Invoke("firstTouchOn", 1.5f);
                    break;

            }
          
        }
    }
    private void firstTouchOn()
    {
        firstTouch = true;
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
    [SerializeField]  float dis;
    float count1;
    private void ActionMoving()
    {
        if (action0) // 반대로 갔다가
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

        if (action1) // 땡겨지기
        {

            if (boxColl.enabled == false)
            {
                boxColl.enabled = true;
            }
            
            magnetSpeed += Time.deltaTime;

            Vector2 ppos = GameManager.Inst.F_Enemy_BulletTargetPos(transform.position);

            ppos.Normalize();
            rb.MovePosition(rb.position + ppos * Time.deltaTime * magnetSpeed);
            
             dis = GameManager.Inst.F_PlayerAndMeDistance(transform.position);
            if(dis < 0.4f)
            {
                canEat = true;
            }
            
        }
    }


    [SerializeField]  bool canEat;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (circleColl.enabled == true && collision.CompareTag("Player") && firstTouch == true)
        {
            target = collision.gameObject;
            action0 = true;
            action1 = false;
            circleColl.enabled = false;
        }

        if (boxColl.enabled == true && collision.CompareTag("Player") && canEat == true)
        {
            if (player_stats_sc == null)
            {
                player_stats_sc = collision.GetComponent<Player_Stats>();
            }

            boxColl.enabled = false;
            SoundManager.inst.F_Get_ControllSoundPreFabs_ETC_PlaySFX(15, 0.7f);
            switch (type)
            {
                case ItemType.Magnet:
                    ManetTrue();
                    break;

                case ItemType.HP_Potion:
                    boxColl.enabled = false;
                    
                    player_stats_sc.F_Use_HP_Potion(25);
                    PoolManager.Inst.F_ReturnItem(gameObject, 1);
                    break;

                case ItemType.Bomb:
                    SoundManager.inst.F_Get_ControllSoundPreFabs_ETC_PlaySFX(11, 0.8f);
                    GameManager.Inst.F_ActiveBomb();
                    PoolManager.Inst.F_ReturnItem(gameObject, 2);
                    GameUIManager.Inst.F_BombEffectOn();
                    
                    break;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (circleColl.enabled == true && collision.CompareTag("Player") && firstTouch == true)
        {
            target = collision.gameObject;
            action0 = true;
            action1 = false;
            circleColl.enabled = false;
        }
    }
}
