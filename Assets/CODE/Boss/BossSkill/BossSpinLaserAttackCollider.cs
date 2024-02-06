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
    WaitForSeconds m_25Sec = new WaitForSeconds(2.5f);
    WaitForSeconds m_1Sec = new WaitForSeconds(1f);
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
    
    }



    public void F_SetDmg(float DMG) // ����� ���� �ʿ�
    {
        Dmg = DMG;
    }

    public void F_ActionStart() // �����Լ� [ ���� ��Ʈ�ѷ����� Ű�� ��������]
    {
        StartCoroutine(ActionPlay());
    }

   
   

    IEnumerator ActionPlay()
    {
        anim.SetTrigger("Play");
        yield return m_25Sec;
        anim.SetTrigger("Action");
        yield return m_1Sec;
        boxCollider.enabled = true;
        ps.gameObject.SetActive(true);
    }

    public void F_ActionEnd() // �����Լ�
    {
        StartCoroutine(DownEmisiion()); 
    }

    IEnumerator DownEmisiion() // �̹̼� ������ ���̸鼭 ����
    {
        float TempEmissionRate = ps.emissionRate; // ���� ����Ʈ�� ����

        anim.SetTrigger("Close");


        while (ps.emissionRate > 0)  // ������� ����Ʈ�� ������
        {
            ps.emissionRate -= Time.deltaTime * 180;
            yield return null;
        }

        boxCollider.enabled = false; // �ݶ��̴� ����

        parent_Sc.SpinStart = false;

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
        Invoke("SpinStartInvoke", 2f);
    }

    private void SpinStartInvoke()
    {
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
