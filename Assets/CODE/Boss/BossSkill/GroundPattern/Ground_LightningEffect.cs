using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Ground_LightningEffect : MonoBehaviour
{
    Light2D lightEffect;
    Light2D skillLight;
    Animator anim;
    ParticleSystem[] Ps = new ParticleSystem[2];
    SpriteRenderer sr;

    [SerializeField] float effectSpeed;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        skillLight = GetComponent<Light2D>();
        lightEffect = transform.Find("Light").GetComponent<Light2D>();
        anim = GetComponent<Animator>();
        Ps = transform.GetComponentsInChildren<ParticleSystem>();

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        skillLight.lightCookieSprite = sr.sprite;

        LightEffectActive();
    }

    public void F_PlayEffect()
    {
        StartCoroutine(Action());
    }

    IEnumerator Action()
    {
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < Ps.Length; i++)
        {
            Ps[i].Play();
        }

        effectActive = true;

    }

    bool effectActive;
    bool valueDown;
    float inputEffectValue;

    private void LightEffectActive()
    {
        if (effectActive)
        {

            lightEffect.intensity = inputEffectValue;

            if (inputEffectValue < 1.4f && valueDown == false)
            {
                inputEffectValue += Time.deltaTime * effectSpeed;
            }
            else if (inputEffectValue > 1.4f)
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
}
