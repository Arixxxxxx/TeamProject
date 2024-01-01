using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Ui_Skill_Select_Btn : MonoBehaviour
{
    public enum SkillType
    {
        Attack0, Attack1, Attack2, Attack3, Attack4,
        Passive0, Passive1, Passive2, Passive3, Passive4
    }
    public SkillType skilltype;

       
    [Header(" #  TextColor")]
    [SerializeField] Color AttackSkill_Name_Color;
    [SerializeField] Color AttackSkill_Text_Color;
    [SerializeField] Color Passive_Name_Color;
    [SerializeField] Color Passive_Text_Color;
    [Header(" # Star Spirte")]
    [SerializeField][Tooltip("별 모양 이미지")] Sprite[] star_Sprite;
    [Header(" #  Input_Field => Skill Text")]
    [Space]
    [SerializeField] CardStringValue[] SkillNameAndText;

    GameObject SkillWindow;
    TMP_Text skill_Name;
    Image skill_Img;
    TMP_Text skill_Text;
    Transform diamondGorupTrs;
    [SerializeField] Image[] start_IMG;

    Image SelectLight;
    Animator staranim;
    Button Btn;
    Player_Skill_System skill;
    Animator anim;
    private void Awake()
    {
        InIt();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
     

    }
    float count;
    private void Update()
    {
        if (SelectLight.gameObject.activeSelf == true)
        {
            SelectLight.transform.Rotate(Vector3.back * Time.deltaTime * SpinSpeed);
        }

        SpinLight();
    }

    private void InIt()
    {
        skill_Name = transform.Find("Skill_Name").GetComponent<TMP_Text>();
        skill_Img = transform.Find("Skill_Img").GetComponent<Image>();
        skill_Text = transform.Find("Skill_Text").GetComponent<TMP_Text>();
        diamondGorupTrs = transform.Find("Dia_Group").GetComponent<Transform>();
        start_IMG = diamondGorupTrs.GetComponentsInChildren<Image>();
        SelectLight = transform.parent.parent.transform.Find("Light ").GetComponent<Image>();
        //SelectLight = transform.Find("Light").GetComponent<Image>();
        staranim = start_IMG[5].GetComponent<Animator>();
        Btn = transform.Find("Btn").GetComponent<Button>();
    }

    private void OnEnable()
    {
        
        Btn_Init();
    }

    private void Btn_Init()
    {
    
        Btn.onClick.RemoveAllListeners();
        Btn.onClick.AddListener(() =>
        {
            SelectAction();
        });
    }

    



    [SerializeField] float SpinSpeed;
    [SerializeField] float animSpeed;
    [SerializeField] float animSpeed1;
    
    public void F_Set_SelectCard(int Star_Count)
    {
        if (skill_Name == null) { InIt(); }

        skill_Name.text = SkillNameAndText[(int)skilltype].Skill_Name;
        skill_Text.text = SkillNameAndText[(int)skilltype].Skill_Text;
        skill_Img.sprite = SkillNameAndText[(int)skilltype].Img;

        switch (skilltype)
        {
            case SkillType.Attack0:
            case SkillType.Attack1:
            case SkillType.Attack2:
            case SkillType.Attack3:
            case SkillType.Attack4:
                skill_Name.color = AttackSkill_Name_Color;
                skill_Text.color = AttackSkill_Text_Color;
                break;

            case SkillType.Passive0:
            case SkillType.Passive1:
            case SkillType.Passive2:
            case SkillType.Passive3:
            case SkillType.Passive4:
                skill_Name.color = Passive_Name_Color;
                skill_Text.color = Passive_Text_Color;
                break;

        }
        //Star_Count++ 첨부터1서응로 보여지는것 삭제 -> 애니메이션으로 변경
        curStar = Star_Count;

        for (int i = 0; i < 5; i++)
        {
            if (i < Star_Count)
            {
                start_IMG[i].sprite = star_Sprite[0];
            }
            else
            {
                start_IMG[i].sprite = star_Sprite[1];
            }
        }
    }

    int curStar;
    private void SelectAction()
    {
      
        if (SelectLight.gameObject.activeSelf == false)
        {
            SelectLight.transform.position = transform.position + new Vector3(-8,30);
            SelectLight.color = new Color(1, 1, 1, 0);
            SelectLight.gameObject.SetActive(true);       // 켜기
        }

        start_IMG[5].fillAmount = 0;
        start_IMG[5].gameObject.transform.position = start_IMG[curStar].transform.position; // 애니메이션 위치이동
        
        StartCoroutine(Start_FillAount());
    }
    [SerializeField] float fillAmountSpeed;
    IEnumerator Start_FillAount()
    {

        anim.SetBool("on", true);
        yield return null;
        while (anim.GetCurrentAnimatorStateInfo(0).IsName("Up") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            if (anim == null)
            {
                anim.GetComponent<Animator>();
            }
            animSpeed1 += Time.unscaledDeltaTime * 1.5f;
            anim.Play("Up", 0, animSpeed1);
            yield return null;
        }

        while (start_IMG[5].fillAmount < 1)
        {
            start_IMG[5].fillAmount += Time.unscaledDeltaTime * fillAmountSpeed;
            yield return null;
        }

        staranim.SetTrigger("hit");
        yield return null;

        while(staranim.GetCurrentAnimatorStateInfo(0).IsName("Action") &&  staranim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            animSpeed += Time.unscaledDeltaTime * 1f;
            staranim.Play("Action", 0, animSpeed);
            yield return null;
        }

       

        yield return new WaitForSecondsRealtime(1.2f);
        skill = Hub.Inst.player_skill_system_sc;
        skill.F_Skill_LvUp((int)skilltype);

        SkillWindow = transform.parent.parent.parent.gameObject;
        GameManager.Inst.F_Lvup_Slot_Reset();
        anim.SetBool("on", false);
        SelectLight.gameObject.SetActive(false);
        staranim.gameObject.SetActive(false);
        GameManager.Inst.F_MainUI_SetAcvite_True();
        SkillWindow.gameObject.SetActive(false);
    }

    private void SpinLight()
    {
        if(SelectLight.gameObject.activeSelf == true)
        {
            
            if(SelectLight.color.a < 1)
            {
            
                SelectLight.color += new Color(0, 0, 0, 0.2f) * Time.unscaledDeltaTime * 0.5f ;
            }
            
            //SelectLight.transform.Rotate(Vector3.back , Time.unscaledDeltaTime * SpinSpeed);
        }
    }
}

[System.Serializable]
public class CardStringValue
{
    public string Skill_Name;

    [Multiline]
    public string Skill_Text;
    public Sprite Img;
}
