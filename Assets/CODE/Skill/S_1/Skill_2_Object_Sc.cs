using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_2_Object_Sc : MonoBehaviour
{
    [SerializeField] float count, timer;

    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer sr;
    

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    void Start()
    {

    }

    private void FixedUpdate()
    {
        if (rb.gravityScale == 0)
        {
           StopCoroutine(StoneUP());
           StartCoroutine(StoneUP());
        }
    }
    private void OnEnable()
    {
        InIt();
    }

    // Update is called once per frame
    void Update()
    {

        Enable_Obj();

        
    }

    private void InIt()
    {
        count = 0;
        rb.gravityScale = 1;
        sr.color = Color.white;
      
    }

    private void Enable_Obj()
    {
        count += Time.deltaTime;
        if (count > timer)
        {
            count = 0;
            
            anim.SetTrigger("hit");
            rb.gravityScale = 0;
            rb.velocity = Vector3.zero;

            GameObject obj = PoolManager.Inst.F_GetPlayerBullet(3);
            obj.transform.position = transform.position;
            obj.gameObject.SetActive(true);
        }
    }

    IEnumerator StoneUP()
    {
        rb.MovePosition(rb.position + Vector2.up * 2 * Time.deltaTime);
        yield return new WaitForSeconds(0.35f);
        anim.SetTrigger("off");
        PoolManager.Inst.F_Return_PlayerBullet(gameObject, 2);
    }
}
