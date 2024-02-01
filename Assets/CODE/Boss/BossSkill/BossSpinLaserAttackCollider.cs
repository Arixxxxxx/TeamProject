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
    // °æº¸ ÀÌÈÄ ¼±Ç³±â ÆÄÆ¼Å¬ Àç»ý ÇÔ¼ö
    // Á¾·áÇÔ¼ö ÆÄÆ¼Å¬  ÀÌ¹Ì¼Ç°ª ÁÙÀÌ°í ÇöÀç Ãâ·ÂÆÄÆ¼Å¬ 0µÇ¸é Á¾·á
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



    public void F_SetDmg(float DMG) // ´ë¹ÌÁö °ü¸® ÇÊ¿ä
    {
        Dmg = DMG;
    }

    public void F_ActionStart() // ½ÃÀÛÇÔ¼ö [ »óÀ§ ÄÁÆ®·Ñ·¯¿¡¼­ Å°°í ²ø¼öÀÕÀ½]
    {
        anim.SetTrigger("Play");
        boxCollider.enabled = true;
    }

    public void F_ActionEnd() // ¿£µåÇÔ¼ö
    {
        StartCoroutine(DownEmisiion()); 
    }

    IEnumerator DownEmisiion() // ÀÌ¹Ì¼Ç ¼­¼­È÷ ÁÙÀÌ¸é¼­ ²¨Áü
    {
        float TempEmissionRate = ps.emissionRate; // ÇöÀç ·¹ÀÌÆ®°ª º¹»ç

        while (ps.emissionRate > 0)  // Àç»ýÁßÀÎ ·¹ÀÌÆ®°ª ³·ÃçÁÜ
        {
            ps.emissionRate -= Time.deltaTime * 150;
            yield return null;
        }

        boxCollider.enabled = false; // ÄÝ¶óÀÌ´õ ²¨ÁÜ

        while (ps.particleCount > 0) // ´Ù ³·­Ÿ´Âµ¥ Àç»ýÁßÀÎ ÆÄÆ¼Å¬ÀÌ ÀÕ´Ù¸é ±â´Ù¸²
        {
            yield return null;
        }

        ps.Stop(); // Á¤Áö

        ps.emissionRate = TempEmissionRate; // ±âÁ¸°ª º¹¿ø
    }



    public void A_ParticlePlay() // ¾Ö´Ï¸ÞÀÌ¼Ç ÇÔ¼ö
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
