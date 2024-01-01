using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.UI.Image;

public class S_4_Object : MonoBehaviour
{

    SpriteRenderer L, R;
    Rigidbody2D Lrb, Rrb;

    float RunCount;

    [SerializeField] float Duration;
    [SerializeField] float MoveSpeed;
    [SerializeField] float SacleSpeed;
    [SerializeField] bool isRun;

    Player_Skill_System skill;

    Vector3 OriginL, OriginR;
    private void Awake()
    {
        L = transform.GetChild(0).GetComponent<SpriteRenderer>();
        R = transform.GetChild(1).GetComponent<SpriteRenderer>();
        Lrb = L.GetComponent<Rigidbody2D>();
        Rrb = R.GetComponent<Rigidbody2D>();
        OriginL = Lrb.transform.localPosition ;
        OriginR = Rrb.transform.localPosition;
    }
    void Start()
    {
        skill = Hub.Inst.player_skill_system_sc;
    }

    private void OnEnable()
    {
        Lrb.transform.localPosition = OriginL;
        Rrb.transform.localPosition = OriginR;
        L.color = new Color(1, 1, 1, 0);
        R.color = new Color(1, 1, 1, 0);
        isRun = true;
        
    }
    // Update is called once per frame

    private void FixedUpdate()
    {
        if (isRun)
        {
            Lrb.MovePosition(Lrb.position + Vector2.left * MoveSpeed * Time.deltaTime);
            Rrb.MovePosition(Rrb.position + Vector2.right * MoveSpeed * Time.deltaTime);
        }
        else
        {

        }
    }

    float count;
    float PingZ;
    void Update()
    {
        Value_Updater();
        Off();


        if (isRun)
        {
            count += Time.deltaTime * SacleSpeed;
            Lrb.gameObject.transform.localScale = new Vector3(1 + count * 0.35f, 1 + count, 1);
            Rrb.gameObject.transform.localScale = new Vector3(1 + count * 0.35f, 1 + count, 1);
            L.color = new Color(1, 1, 1, count * 0.9f);
            R.color = new Color(1, 1, 1, count * 0.9f);
            PingZ = Mathf.PingPong(Time.time * 25, 6);
            PingZ -= 3f; 
            Lrb.transform.eulerAngles = new Vector3(0, 0, PingZ);
            Rrb.transform.eulerAngles = new Vector3(0, 0, PingZ);
        }
        else
        {
            count = 0;
        }
    }

    
    private void Off()
    {
        if(isRun)
        {
            RunCount += Time.deltaTime;
            if(RunCount > Duration)
            {
                RunCount = 0;
                isRun = false;
                
                PoolManager.Inst.F_Return_PlayerBullet(gameObject, 4);
            }
        }
    }
    float[] GetValue = new float[3];
    private void Value_Updater()
    {
        GetValue = skill.F_Get_Skill_4_Value();
        Duration = GetValue[0];
        MoveSpeed = GetValue[1] ;
        SacleSpeed = GetValue[2];
    }
}
