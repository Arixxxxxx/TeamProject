using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Enemy_Arrow : MonoBehaviour
{
    [SerializeField] Vector2 Target;
    [SerializeField] float Arrow_Speed;
    Rigidbody2D Rb;
    GameManager gm;
    float RotAngle;
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        gm = GameManager.Inst;

        Arrow_Init();
    }

    private void OnEnable()
    {
        if(gm == null)
        {
            gm = GameManager.Inst;
        }

        Arrow_Init();
    }
    private void OnBecameInvisible()
    {
        Target = Vector2.zero;
        RotAngle = 0;
        transform.rotation = Quaternion.identity;
        PoolManager.Inst.F_ReturnObj(gameObject, 0);
    }
    private void FixedUpdate()
    {
        Target = Target.normalized;
        Rb.MovePosition(Rb.position +  Target * Arrow_Speed * Time.deltaTime);
    }

    private void Arrow_Init()
    {
        Target = gm.F_Enemy_BulletTargetPos(transform.position);
        RotAngle = gm.F_EnemyBulletRotation(transform.position);
        transform.rotation = Quaternion.AngleAxis(RotAngle * -1, Vector3.back);
    }
    
    void Update()
    {
        
    }
}
