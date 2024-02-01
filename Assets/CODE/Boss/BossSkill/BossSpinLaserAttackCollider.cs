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
    // �溸 ���� ��ǳ�� ��ƼŬ ��� �Լ�
    // �����Լ� ��ƼŬ  �̹̼ǰ� ���̰� ���� �����ƼŬ 0�Ǹ� ����
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



    public void F_SetDmg(float DMG) // ����� ���� �ʿ�
    {
        Dmg = DMG;
    }

    public void F_ActionStart() // �����Լ� [ ���� ��Ʈ�ѷ����� Ű�� ��������]
    {
        anim.SetTrigger("Play");
        boxCollider.enabled = true;
    }

    public void F_ActionEnd() // �����Լ�
    {
        StartCoroutine(DownEmisiion()); 
    }

    IEnumerator DownEmisiion() // �̹̼� ������ ���̸鼭 ����
    {
        float TempEmissionRate = ps.emissionRate; // ���� ����Ʈ�� ����

        while (ps.emissionRate > 0)  // ������� ����Ʈ�� ������
        {
            ps.emissionRate -= Time.deltaTime * 150;
            yield return null;
        }

        boxCollider.enabled = false; // �ݶ��̴� ����

        while (ps.particleCount > 0) // �� �����µ� ������� ��ƼŬ�� �մٸ� ��ٸ�
        {
            yield return null;
        }

        ps.Stop(); // ����

        ps.emissionRate = TempEmissionRate; // ������ ����
    }



    public void A_ParticlePlay() // �ִϸ��̼� �Լ�
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
