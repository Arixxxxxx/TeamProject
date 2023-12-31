using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Object : MonoBehaviour
{
    public enum BulletType { Active1}
    public BulletType type;
    
    [Header("# Setting Value")]
    [Space]
    [Space]
    [SerializeField] float dis;
    [SerializeField] float bulletSpeed;
    [Header("# Cheak Value")]
    [Space]
    [SerializeField] float RanX;
    [SerializeField] float RanY;
    [SerializeField] Vector2 Target;
    Rigidbody2D rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        InIt();
        RandomPoint();
    }

    private void OnEnable()
    {
        Debug.Log("11");
            InIt();
            RandomPoint();
     
    }

    private void FixedUpdate()
    {
        if (Target != null)
        {
            dis = Vector2.Distance(transform.position, Target);
           
            rb.MovePosition(Vector3.Lerp(transform.position, Target, 0.5f * bulletSpeed));
           

            if (dis < 1f)
            {
                GameObject obj = PoolManager.Inst.F_GetPlayerBullet(1);
                obj.SetActive(true);
                obj.transform.position = transform.position;

                PoolManager.Inst.F_Return_PlayerBullet(gameObject, 0);
            }
        }

      
    }
    // Update is called once per frame
    void Update()
    {
        
    }
   
    private void InIt()
    {
        RanX = 0;
        RanY = 0;
        Target = Vector3.zero;
    }

    private void RandomPoint()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        
        Set_RandomValue();

        Target = new Vector2(transform.position.x + RanX, transform.position.y + RanY);
    } 

    private void Set_RandomValue()
    {
        int randomX = Random.Range(0, 2);
        
        if (randomX == 0)
        {
            RanX = Random.Range(-6.0f, -2.5f);
        }
        else if (randomX == 1)
        {
            RanX = Random.Range(2.5f, 6.0f);
        }

        RanY = -1.5f; /*Random.Range(-2.0f, 2.0f);*/
        
    }
}


