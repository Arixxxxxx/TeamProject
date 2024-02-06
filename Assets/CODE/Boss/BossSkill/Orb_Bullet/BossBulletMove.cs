using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossBulletMove : MonoBehaviour
{
    public enum BulettPartType
    {
        Ball, Shadow
    }
    public BulettPartType type;

    Rigidbody2D rb;

    [SerializeField] float moveSpeed;

    [SerializeField] GameObject ShadowTarget;
    float shadowDownY = -2.2f;

    private void Awake()
    {
        if(this.gameObject.name == "Shadow")
        {
            type = BulettPartType.Shadow;
        }


        switch (type)
        {
            case BulettPartType.Ball:
                rb = GetComponent<Rigidbody2D>();
                break;

            case BulettPartType.Shadow:
                ShadowTarget = transform.parent.transform.Find("Bullet").gameObject;
                break;

        }
    }

    private void OnDisable()
    {
        ballStart = false;
        ReturnWait = false;
        returnCount = 0;
        count = 0;
    }
    
    void Start()
    {

    }

    private void FixedUpdate()
    {
        BallMove();
    }
    void Update()
    {
        Set_ShadowPos();
        StartTimeCounter();
        Obj_Return();
    }


    bool ballStart;
    bool ReturnWait;
    float count;
    float returnCount;
    [SerializeField] float startTime;
    [SerializeField] float ReturnTime;

    private void Obj_Return()
    {
        if(ReturnWait == false)
        {
            returnCount += Time.deltaTime;

            if (ReturnTime == 0)
            {
                ReturnTime = 5;
            }

            if (returnCount > ReturnTime)
            {
                ReturnWait = true;
                returnCount = 0;
                BossSkill_Pool.inst.F_ReturnSkillEfeect(transform.parent.gameObject, 1);
            }
        }
       

    }
    private void StartTimeCounter()
    {
        if (ballStart == false)
        {
            count += Time.deltaTime;

            if (count > startTime)
            {
                count = 0;
                ballStart = true;
            }
        }
    }
    private void BallMove()
    {
        if (type == BulettPartType.Ball && ballStart == true)
        {
            rb.MovePosition(rb.position + (Vector2)transform.up * Time.deltaTime * moveSpeed);
        }
    }


    

    private void Set_ShadowPos()
    {
        if (type == BulettPartType.Shadow)
        {
            transform.position = ShadowTarget.transform.position + new Vector3(0, shadowDownY);
        }
    }
}
