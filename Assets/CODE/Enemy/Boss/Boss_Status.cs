using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Boss_Status : MonoBehaviour
{
    [Header("# Input boss Status Value == ¿¹Áø")]
    [Space]
    [SerializeField] float boss_CurHP;
    [SerializeField] float boss_MaxHP;
    [SerializeField] bool isDead;
    [SerializeField] bool isShield;
    GameObject Mujuk_Effect;

    [Header("# Input boss Status Value == ¿¹Áø")]
    [Space]
    [SerializeField] float FillAmountSpeed;
    [SerializeField] float dmgFontY_Add;


    Animator anim;
    TMP_Text Hp_Bar_Text;
    Transform Boss_HP_Bar_Object;
    Image Boss_Front_IMG, Boss_Middle_IMG;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        Mujuk_Effect = transform.Find("Skill/Shield").gameObject;
    }
    void Start()
    {
        Boss_HP_Bar_Object = UnitFrame_Updater.inst.F_Get_UI_Tranfrom().transform.Find("Main_Canvas/Boos_Hp_Bar");
        Boss_Front_IMG = Boss_HP_Bar_Object.transform.Find("HP_Front").GetComponent<Image>();
        Boss_Middle_IMG = Boss_HP_Bar_Object.transform.Find("HP_Middle").GetComponent<Image>();
        Hp_Bar_Text = Boss_Front_IMG.transform.Find("LvText").GetComponent<TMP_Text>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Boss_Hp_UiBar_Updater();
        Shiled_Cheaker();

    }

    private void Shiled_Cheaker()
    {
        if(Mujuk_Effect.activeSelf == true)
        {
            isShield = true;
        }
        else
        {
            isShield = false;
        }
    }
    public void F_Enemy_On_Hit(float DMG, bool Cri)
    {
        if(isShield == true) 
        {
            GameObject obj_font = PoolManager.Inst.F_GetObj(2);
            obj_font.GetComponent<Dmg_Font>().F_text_Init("Èí¼ö");
            obj_font.transform.position = transform.position + new Vector3(0, dmgFontY_Add, 0);
            obj_font.gameObject.SetActive(true);

            StartCoroutine(DMG_Font_Animation(obj_font));
            return; 
        
        }

        if (boss_CurHP > 0)
        {
            float Dmgs = DMG;

            if (Cri == true)
            {
                Dmgs = DMG * 2;
                anim.SetTrigger("hit");
            }

            GameObject obj_font = PoolManager.Inst.F_GetObj(2);
            obj_font.GetComponent<Dmg_Font>().F_text_Init(Dmgs, Cri);
            obj_font.transform.position = transform.position + new Vector3(0, dmgFontY_Add, 0);
            obj_font.gameObject.SetActive(true);

            StartCoroutine(DMG_Font_Animation(obj_font));

            boss_CurHP -= Dmgs;

            //Hp_Bar_anim.SetTrigger("hit");

            if (boss_CurHP <= 0)
            {
                //anim.SetTrigger("Dead"); 

                isDead = true;
                
            }
        }
    }

    WaitForSeconds DMG_Font_Dealy = new WaitForSeconds(0.15f);
    IEnumerator DMG_Font_Animation(GameObject obj)
    {
        yield return DMG_Font_Dealy;
        obj.GetComponent<Animator>().SetTrigger("hit");
    }


    private void Boss_Hp_UiBar_Updater()
    {
        if(Boss_HP_Bar_Object.gameObject.activeSelf == false) { return; }

        Hp_Bar_Text.text = $" [Á¤¿¹] ¸¶³à    {boss_CurHP} / {boss_MaxHP}";

        Boss_Front_IMG.fillAmount = boss_CurHP / boss_MaxHP;
        if (Boss_Front_IMG.fillAmount < Boss_Middle_IMG.fillAmount)
        {
            Boss_Middle_IMG.fillAmount -= Time.deltaTime * FillAmountSpeed;
        }
        else if (Boss_Front_IMG.fillAmount > Boss_Middle_IMG.fillAmount)
        {
            Boss_Middle_IMG.fillAmount = Boss_Front_IMG.fillAmount;
        }
    }


}
