using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class New_S_4 : MonoBehaviour
{
    ParticleSystem L, R;
    ParticleSystem Lpapyun, Rpapyun;
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
        L = transform.GetChild(0).GetComponent<ParticleSystem>();
        Lpapyun = L.transform.Find("Particle System (1)").GetComponent<ParticleSystem>();
        R = transform.GetChild(1).GetComponent<ParticleSystem>();
        
        Lrb = L.GetComponent<Rigidbody2D>();
        Rrb = R.GetComponent<Rigidbody2D>();
        OriginL = Lrb.transform.localPosition;
        OriginR = Rrb.transform.localPosition;
    }
    void Start()
    {
        skill = Player_Skill_System.Inst;
    }

    private void OnEnable()
    {
        Lrb.transform.localPosition = OriginL;
        Rrb.transform.localPosition = OriginR;
      
        isRun = true;
        Lcurvelo = Vector3.zero;
        Rcurvelo = Vector3.zero;



    }
    // Update is called once per frame

    Vector3 Lcurvelo = Vector3.zero;
    Vector3 Rcurvelo = Vector3.zero;
    private void FixedUpdate()
    {
        if (isRun && L.isPlaying == true)
        {
                Lrb.MovePosition(Lrb.position + Vector2.left * MoveSpeed * Time.deltaTime);
                Rrb.MovePosition(Rrb.position + Vector2.right * MoveSpeed * Time.deltaTime);
        }
    }

    float count;
    float PingZ;
    void Update()
    {
        Value_Updater();
        Off();
    }


    private void Off()
    {
        if (isRun)
        {
            RunCount += Time.deltaTime;
            if (RunCount > Duration)
            {
                RunCount = 0;
                isRun = false;
                //curvelo = Vector3.zero;
                PoolManager.Inst.F_Return_PlayerBullet(gameObject, 4);
            }
        }
    }
    float[] GetValue = new float[3];
    private void Value_Updater()
    {
        GetValue = skill.F_Get_Skill_4_Value();
        Duration = GetValue[0];
        MoveSpeed = GetValue[1];
        SacleSpeed = GetValue[2];
    }
}
