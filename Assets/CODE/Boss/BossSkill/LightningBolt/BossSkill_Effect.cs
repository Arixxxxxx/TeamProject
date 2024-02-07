using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BossSkill_Effect : MonoBehaviour
{
    Light2D skillLight;
    Light2D lightEffect;
    SpriteRenderer sr;
    Animator anim;
    ParticleSystem ps;
    ParticleSystem stonePs;
    BoxCollider2D boxCollider;
    bool onceAttack;
    Player_Stats playerStats_sc;
    [SerializeField] bool autoMode;
    [SerializeField] float DMG;

    private void Awake()
    {
        skillLight = GetComponent<Light2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        ps = transform.Find("Boss_LightningSkill").GetComponent<ParticleSystem>();
        stonePs = transform.Find("Fog").GetComponent<ParticleSystem>();
        lightEffect = transform.Find("Light").GetComponent<Light2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void OnEnable()
    {
        if (autoMode)
        {
            F_StartSkill();
        }
    }
    void Start()
    {
        if (playerStats_sc == null)
        {
            playerStats_sc = GameManager.Inst.F_GetPlayerStats_Script();
        }

    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.N))
        {
            F_StartSkill();
        }
#endif
        SpriteRenderUpdater();
        LightEffectActive();
    }

    public void F_StartSkill()
    {
        StopCoroutine(SkillStart());
        StartCoroutine(SkillStart());
    }

    bool effectActive;
    IEnumerator SkillStart()
    {
        ps.Play();
        yield return new WaitForSeconds(1.9f);
        
        anim.SetTrigger("Hit");

        yield return new WaitForSeconds(0.25f);
        boxCollider.enabled = true;
        effectActive = true;
        stonePs.Play();

        yield return new WaitForSeconds(0.25f);
        boxCollider.enabled = false;
        onceAttack = false;

        yield return new WaitForSeconds(2.2f);

        BossSkill_Pool.inst.F_ReturnSkillEfeect(gameObject, 0);


    }
    //지면 충돌 조명
    bool valueDown;
    float inputEffectValue;
    [SerializeField] float effectSpeed;
    private void LightEffectActive()
    {
        if (effectActive)
        {

            lightEffect.intensity = inputEffectValue;

            if (inputEffectValue < 1.4f && valueDown == false)
            {
                inputEffectValue += Time.deltaTime * effectSpeed;
            }
            else if(inputEffectValue > 1.4f)
            {
                valueDown = true;
            }
        }

        if (valueDown && inputEffectValue > 0)
        {
            inputEffectValue -= Time.deltaTime * effectSpeed;
        }
        else if (valueDown && inputEffectValue <= 0)
        {
            inputEffectValue = 0;
            effectActive = false;
            valueDown = false;
        }
    }


    // 번개 애니메이션 조명
    private void SpriteRenderUpdater()
    {
        if(sr.sprite != null)
        {
            skillLight.lightCookieSprite = sr.sprite;
        }
        else
        {
            skillLight.lightCookieSprite = null;
        }
    }

    public void A_Shake()
    {
        CameraManager.inst.F_ShakeCamForBossSkillEffect(1.3f);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && onceAttack == false && collision.GetComponent<Player_Stats>() != null) 
        {
            onceAttack = true;
            playerStats_sc.F_Player_On_Hit(DMG);
        }
    }
}
