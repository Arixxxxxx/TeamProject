using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    
    [Header("# Enemy Stats Info")]
    [Space]
    [SerializeField] float CurHP;
    [SerializeField] float MaxHP;
    [SerializeField] float Exp;
    [SerializeField] bool Enemy_Dead;
    [Header("# Insert HpBar Obj")]
    [Space]
    [SerializeField] GameObject Hp_Bar;
    [SerializeField] Image Hp_Bar_Middle;
    [SerializeField] Image Hp_Bar_Front;
    [SerializeField] float FillAmountSpeed;

    BoxCollider2D boxCollider;
    Enemy_Nav_Movement nav;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        nav = GetComponent<Enemy_Nav_Movement>();
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
        if(CurHP > 0)
        {
            CurHP -= DMG;

            if(CurHP <= 0)
            {
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
        if(CurHP <= 0 && Hp_Bar.gameObject.activeSelf == true)
        {
            Hp_Bar.SetActive(false);
            boxCollider.enabled = false;
        }

        Hp_Bar_Front.fillAmount = CurHP / MaxHP;
        if(Hp_Bar_Front.fillAmount < Hp_Bar_Middle.fillAmount)
        {
            Hp_Bar_Middle.fillAmount -= Time.deltaTime * FillAmountSpeed;
        }
        else if(Hp_Bar_Front.fillAmount > Hp_Bar_Middle.fillAmount)
        {
            Hp_Bar_Middle.fillAmount = Hp_Bar_Front.fillAmount;
        }

        
    }
}
