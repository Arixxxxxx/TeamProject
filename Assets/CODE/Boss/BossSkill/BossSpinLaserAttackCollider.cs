using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossSpinLaserAttackCollider : MonoBehaviour
{
    [SerializeField] float Dmg;
    Player_Stats playerSc;
    BoxCollider2D boxCollider;
    ParticleSystem ps;
    Animator anim;
    BossSpinLaser parent_Sc;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        ps =   transform.Find("Ps").GetComponent<ParticleSystem>();
        boxCollider = GetComponent<BoxCollider2D>();

        if(boxCollider.enabled == true)
        {
            boxCollider.enabled = false;
        }

        parent_Sc = transform.GetComponentInParent<BossSpinLaser>();



    }
    // 경보 이후 선풍기 파티클 재생 함수
    // 종료함수 파티클  이미션값 줄이고 현재 출력파티클 0되면 종료
    void Start()
    {
        playerSc = GameManager.Inst.F_Get_PlayerSc();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B)) 
        {
            F_ActionEnd();
        }
    }



    public void F_SetDmg(float DMG) // 대미지 관리 필요
    {
        Dmg = DMG;
    }

    public void F_ActionStart() // 시작함수 [ 상위 컨트롤러에서 키고 끌수잇음]
    {
        anim.SetTrigger("Play");
        boxCollider.enabled = true;
    }

    public void F_ActionEnd() // 엔드함수
    {
        StartCoroutine(DownEmisiion()); 
    }

    IEnumerator DownEmisiion() // 이미션 서서히 줄이면서 꺼짐
    {
        float TempEmissionRate = ps.emissionRate; // 현재 레이트값 복사

        while (ps.emissionRate > 0)  // 재생중인 레이트값 낮춰줌
        {
            ps.emissionRate -= Time.deltaTime * 150;
            yield return null;
        }

        boxCollider.enabled = false; // 콜라이더 꺼줌

        while (ps.particleCount > 0) // 다 낮췃는데 재생중인 파티클이 잇다면 기다림
        {
            yield return null;
        }

        ps.Stop(); // 정지

        ps.emissionRate = TempEmissionRate; // 기존값 복원
    }



    public void A_ParticlePlay() // 애니메이션 함수
    {
        ps.Play();
        parent_Sc.SpinStart = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.name != "Dragon")
        {
            if(Dmg == 0)
            {
                Dmg = 20;
            }

            playerSc.F_Player_On_Hit(Dmg);
        }
    }
}
