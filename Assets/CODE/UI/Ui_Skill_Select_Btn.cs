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
    [SerializeField][Tooltip("�� ��� �̹���")] Sprite[] star_Sprite;
    [Header(" #  Input_Field => Skill Text")]
    [Space]
    [SerializeField] CardStringValue[] SkillNameAndText;
    [Header(" #  Input_Field => Skill Text")]
    [Space]
    [SerializeField] float ExitTime;

    GameObject SkillWindow;
    TMP_Text skill_Name;
    Image skill_Img;
    TMP_Text skill_Text;
    Transform diamondGorupTrs;
    [SerializeField] Image[] start_IMG;
    [SerializeField] int Get_Star;
    Image SelectLight;
    Animator lightAnim;
    Animator staranim;
    Button Btn;
    Player_Skill_System skill;
    Animator anim;
    Image NewLight;
    Image Light2;
    
    private void Awake()
    {
        InIt();
        anim = GetComponent<Animator>();
        
    }
    void Start()
    {
     

    }
    float count;
    float animSpeed2;
    private void Update()
    {
        //SpinLight();
    }

    private void InIt()
    {
        skill_Name = transform.Find("Skill_Name").GetComponent<TMP_Text>();
        skill_Img = transform.Find("Skill_Img").GetComponent<Image>();
        skill_Text = transform.Find("Skill_Text").GetComponent<TMP_Text>();
        diamondGorupTrs = transform.Find("Dia_Group").GetComponent<Transform>();
        start_IMG = diamondGorupTrs.GetComponentsInChildren<Image>();
        staranim = start_IMG[5].GetComponent<Animator>();
        Btn = transform.Find("Btn").GetComponent<Button>();
        NewLight = transform.Find("Light").GetComponent<Image>();
        Light2 = transform.Find("Light2").GetComponent<Image>();
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
    
    /// <summary>
    ///  ī�� ���� �Է� / �Ű����� : ��ų����
    /// </summary>
    /// <param name="Star_Count"></param>
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
        //Star_Count++ ÷����1������ �������°� ���� -> �ִϸ��̼����� ����
        Get_Star = Star_Count;

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
    bool cardUpDown;
    
    //ī�� ���� �����Լ�
    private void SelectAction()
    {
        GameManager.Inst.F_Lvup_Btn_OnOff(0); // ��� ��ư Interactable true
        animSpeed1 = 0; // �ִϸ��̼� float ���� �ʱ�ȭ
        animSpeed = 0; // �ִϸ��̼� float ���� �ʱ�ȭ
        Light2.gameObject.SetActive(false);

        if (NewLight.gameObject.activeSelf == false)  // ���� �� ��� ���� ����
        {
            NewLight.gameObject.SetActive(true);       // �ѱ�
        }
        start_IMG[5].fillAmount = 0; // ���̹��� �ʱ�ȭ
        start_IMG[5].gameObject.transform.position = start_IMG[Get_Star].transform.position; // �� �̹��� ��ġ�̵�
        staranim.gameObject.SetActive(true);

        StartCoroutine(Start_FillAount()); // �ڷ�ƾ ����
    }
    [SerializeField] float fillAmountSpeed;
    IEnumerator Start_FillAount()
    {

        anim.SetBool("on", true);

        yield return null;
                 

        while (start_IMG[5].fillAmount < 1)  // ���� ���� ���� 
        {
            start_IMG[5].fillAmount += Time.unscaledDeltaTime * fillAmountSpeed;
            yield return null;
        }
        staranim.SetTrigger("hit");

        yield return new WaitForSecondsRealtime(1.2f); // ��� ���� �� �ش� �ð� ���� ����

        skill = Hub.Inst.player_skill_system_sc;
        skill.F_Skill_LvUp((int)skilltype);

        SkillWindow = transform.parent.parent.parent.gameObject;
        
        anim.SetBool("on", false);

        yield return null;

        NewLight.gameObject.SetActive(false);
        Light2.gameObject.SetActive(false);
        staranim.gameObject.SetActive(false);
        GameManager.Inst.F_MainUI_SetAcvite_True();
        GameManager.Inst.F_Lvup_Slot_Reset();
        
        cardUpDown = false;
        GameManager.Inst.F_TimeSclaeController(false);
        SkillWindow.gameObject.SetActive(false);
        
    }
    

    private void SpinLight()
    {
        if(NewLight.gameObject.activeSelf == true)
        {
            if(NewLight.color.a < 0.7f)
            {
                NewLight.color += new Color(0, 0, 0, 0.1f) * Time.unscaledDeltaTime * 2f ;
            }
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
