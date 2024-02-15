using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    public enum expCoin_DropType { Small, Medium, Large };
    public enum EnemyName { Orc, Mushroom, SkeletonRanger, Slime,  Orc_Ranger, Tree };

    public EnemyName for_ReturnOBJ_enemyName;
    public expCoin_DropType ExpCoin_DropType;

    [Header("# Enemy Stats Info  ==     # ¿¹Áø ")]
    [Space]
    [SerializeField] float CurHP;
    [SerializeField] float MaxHP;
    [SerializeField] float Exp;
    [Space]
    [SerializeField] bool enemy_Dead;
    public bool Enemy_Dead { get { return enemy_Dead; } }
    [Header("# Enemy HP_Bar Middle Image Speed Info  ==     # ¿¹Áø ")]
    [SerializeField] float FillAmountSpeed;

    GameObject Hp_Bar;
    Image Hp_Bar_Middle;
    Image Hp_Bar_Front;

    SpriteRenderer sr;
    SpriteRenderer shadowSr;
    BoxCollider2D boxCollider;
    Enemy_Nav_Movement nav;
    Animator anim;
    Animator Hp_Bar_anim;

    NavMeshAgent navMesh;
    float tempNavColliderRadius;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        shadowSr = transform.Find("Shadow").GetComponent<SpriteRenderer>();
        Hp_Bar = transform.GetChild(0).gameObject;
        Hp_Bar_Middle = Hp_Bar.transform.GetChild(1).GetComponent<Image>();
        Hp_Bar_Front = Hp_Bar.transform.GetChild(2).GetComponent<Image>();
        boxCollider = GetComponent<BoxCollider2D>();
        nav = GetComponent<Enemy_Nav_Movement>();
        anim = GetComponent<Animator>();
        Hp_Bar_anim = transform.Find("HPBar").GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
       
    }
    private void OnEnable()
    {
        ReSponeEnemy_Init();
    }
    // Update is called once per frame
    void Update()
    {
        HpBar_Ui_Updater();
    }
    [SerializeField] float dmgFontY_Add;
    public void F_Enemy_On_Hit(float DMG, bool Cri)
    {
        if (CurHP > 0)
        {
            SoundManager.inst.F_Get_ControllSoundPreFabs_ETC_PlaySFX(10, 0.5f);
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

            CurHP -= Dmgs;

            Hp_Bar_anim.SetTrigger("hit");

            if (CurHP <= 0)
            {
                
                anim.SetTrigger("Dead");
                enemy_Dead = true;
                anim.SetBool("Attack", false);
                nav.F_Dead(true);

                GameManager.Inst.F_KillCountUp();
                GameObject obj = PoolManager.Inst.F_GetObj(1); // ÄÚÀÎ¾Ø¼º
                obj.GetComponent<Exp_Coin>().F_SettingCoin((int)ExpCoin_DropType); // °æÇèÄ¡ ¼ÂÆÃ
                obj.transform.position = transform.position - new Vector3(0, 0.7f, 0);
                obj.gameObject.SetActive(true);



                // 0.5% È®·ü·Î  ÆøÅº»ý¼º
                float spawnBombChans = 0.5f;
                float dice = Random.Range(0f, 100f);
                
                if (spawnBombChans > dice)
                {
                    GameObject obj2 = PoolManager.Inst.F_GetItem(2); // ÆøÅº
                    obj2.transform.position = transform.position - new Vector3(0, 0.7f, 0);
                    obj2.gameObject.SetActive(true);
                    //obj2.GetComponent<Magnet>().F_NoRigidBody();
                }
                



            }
        }
    }

    WaitForSeconds DMG_Font_Dealy = new WaitForSeconds(0.15f);
    IEnumerator DMG_Font_Animation(GameObject obj)
    {
        yield return DMG_Font_Dealy;
        obj.GetComponent<Animator>().SetTrigger("hit"); 
    }


    private void ReSponeEnemy_Init()
    {
        Hp_Bar.SetActive(true);
        boxCollider.enabled = true;
        CurHP = MaxHP;
        enemy_Dead = false;
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

    private void A_ReturnObj()
    {
        sr.color = Color.white;
        shadowSr.color = Color.white;
        PoolManager.Inst.F_Return_Enemy_Obj(gameObject, (int)for_ReturnOBJ_enemyName);

    }

}
