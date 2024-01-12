using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S3_1 : MonoBehaviour
{
    [SerializeField] float Speed;
    [SerializeField] float LifeTime;
    float count;
    Animator anim;
    ParticleSystem ps;
    Vector3 startPos;
    TrailRenderer trail;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        ps = transform.Find("S_3_Particle").GetComponent<ParticleSystem>();
        trail = transform.Find("Trail").GetComponent<TrailRenderer>();
    }
    void Start()
    {
       
    }

    private void OnEnable()
    {
        Init();
    }
    bool stoneup;
    void Update()
    {
        count += Time.deltaTime;

        if (count < LifeTime)
        {
            transform.Translate(Vector2.down * Speed * Time.deltaTime);
        }
        else if (count > LifeTime && stoneup == false)
        {
            stoneup = true;
            trail.Clear();
            StartCoroutine(off());
            ps.gameObject.SetActive(false);
            anim.SetTrigger("hit");
            GameObject obj = PoolManager.Inst.F_GetPlayerBullet(1);
            obj.transform.position = transform.position + new Vector3(-0.5f,-0.2f);
            obj.gameObject.SetActive(true);
        }

        StoneUP();
    }

    private void StoneUP()
    {
        if (stoneup)
        {
            transform.position += Vector3.up * 0.5f * Time.deltaTime;
        }
    }


    IEnumerator off()
    {
        yield return new WaitForSeconds(0.35f);
        stoneup = false;
        anim.SetTrigger("off");
        transform.position = startPos;
        count = 0;
        PoolManager.Inst.F_Return_PlayerBullet(gameObject, 2);
    }

    private void Init()
    {
        ps.gameObject.SetActive(true);
        startPos = transform.position;
    }
}
