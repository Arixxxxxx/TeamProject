using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{

    [Header("# Enemy Stats Info  ==     # 예진 ")]
    [Space]
    [SerializeField] float CurHP;
    [SerializeField] float MaxHP;
    [SerializeField] float Exp;
    [Space]
    [SerializeField] bool Enemy_Dead;
    [Header("# Enemy HP_Bar Middle Image Speed Info  ==     # 예진 ")]
    [SerializeField] float FillAmountSpeed;

    GameObject Hp_Bar;
    Image Hp_Bar_Middle;
     Image Hp_Bar_Front;
    

    BoxCollider2D boxCollider;
    Enemy_Nav_Movement nav;
    Animator anim;
    Animator Hp_Bar_anim;

    private void Awake()
    {
        Hp_Bar = transform.GetChild(0).gameObject;
        Hp_Bar_Middle = Hp_Bar.transform.GetChild(1).GetComponent<Image>();
        Hp_Bar_Front = Hp_Bar.transform.GetChild(2).GetComponent<Image>();
        boxCollider = GetComponent<BoxCollider2D>();
        nav = GetComponent<Enemy_Nav_Movement>();
        anim = GetComponent<Animator>();
        Hp_Bar_anim = transform.Find("HPBar").GetComponent<Animator>();
    }
    void Start()
    {
        ReSponeEnemy_Init();
    }

    // Update is called once per frame
    void Update()
    {
        HpBar_Ui_Updater();
    }

    public void F_Enemy_On_Hit(float DMG)
    {
        if (CurHP > 0)
        {
            CurHP -= DMG;
            anim.SetTrigger("hit");
            Hp_Bar_anim.SetTrigger("hit");

            if (CurHP <= 0)
            {
                anim.SetTrigger("Dead");
                Enemy_Dead = true;
                nav.F_Dead(true);
                GameManager.Inst.F_Get_PlayerSc().F_GetExp_LevelupSystem(Exp);
            }
        }
    }


    private void ReSponeEnemy_Init()
    {
        Hp_Bar.SetActive(true);
        boxCollider.enabled = true;
        CurHP = MaxHP;
        Enemy_Dead = false;
    }

    private void HpBar_Ui_Updater()
    {
        if (CurHP <= 0 && Hp_Bar.gameObject.activeSelf == true)
        {
            Hp_Bar.SetActive(false);
            boxCollider.enabled = false;
        }

        Hp_Bar_Front.fillAmount = CurHP / MaxHP;
        if (Hp_Bar_Front.fillAmount < Hp_Bar_Middle.fillAmount)
        {
            Hp_Bar_Middle.fillAmount -= Time.deltaTime * FillAmountSpeed;
        }
        else if (Hp_Bar_Front.fillAmount > Hp_Bar_Middle.fillAmount)
        {
            Hp_Bar_Middle.fillAmount = Hp_Bar_Front.fillAmount;
        }
    }

   
}
