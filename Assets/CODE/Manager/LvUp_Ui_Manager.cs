using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvUp_Ui_Manager : MonoBehaviour
{
    public static LvUp_Ui_Manager Inst;

    [Header("# Insert Window Obj in Hierarchy")]
    [Space]
    [SerializeField] GameObject LvupWindow;
    [SerializeField] GameObject Prefabs;
    [SerializeField] GameObject[] Btn;
    [Header("# ���ݽ�ų������ ���� ���Ѽ�")]
    [Space]
    [SerializeField] int OnlyAttackSkillGet_Lv;
    [Header("# Cheak value")]
    [Space]
    [SerializeField] int[] Get_Skill_Value = new int[10];
    [SerializeField] List<int> Skill_ID_List = new List<int>();
    [SerializeField] List<int> Skill_Lv = new List<int>();
    Transform Slot;
    Player_Skill_System Skill_sc;
    Player_Stats status;

    int SkillMaxLv;
    int PlayerLv;

    private void Awake()
    {
        if(Inst  == null)
        {
            Inst = this;
        }
        else
        {
            Destroy(this);
        }

        Slot = LvupWindow.transform.Find("Box/Slot_Box").GetComponent<Transform>();
    }
    void Start()
    {
        status = Hub.Inst.Player_Status_sc;
        Skill_sc = Hub.Inst.player_skill_system_sc;
        SkillMaxLv = Skill_sc.skill_Max_Lvl;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            F_LvUP_SelectSkill();
        }
    }

    public void F_LvUP_SelectSkill()
    {
        GameManager.Inst.F_Lvup_Btn_OnOff(1);

        Get_Skill_Value = Skill_sc.F_Get_CurSkillLv(); // ���� ���� �ʱ�ȭ �Լ�
        PlayerLv = status.F_Get_Player_LV(); // �÷��̾� ���� ������

        // ��ų �ƽ��������� ���� ��ų�� �� ��������
        for (int i = 0; i < Get_Skill_Value.Length; i++)
        {
            if (Get_Skill_Value[i] < SkillMaxLv)
            {
                Skill_ID_List.Add(i);
                Skill_Lv.Add(Get_Skill_Value[i]);
            }
        }

        // ����
        for (int i = 0; i < 3; i++)
        {
            int RandomValue = 0 ;

            if(PlayerLv < OnlyAttackSkillGet_Lv)
            {
                Debug.Log("1");
                RandomValue = Random.Range(0, 3); // ����ī��Ʈ����
            }
            else if(PlayerLv >= OnlyAttackSkillGet_Lv) 
            {
                Debug.Log("2");
                RandomValue = Random.Range(0, Skill_ID_List.Count); // ����ī��Ʈ����    
            }


            
            int Skill_ID = Skill_ID_List[RandomValue]; // ����
            int Lv_Value = Skill_Lv[RandomValue]; //��ų����
                        
            
            Ui_Skill_Select_Btn sc = Btn[i].GetComponent<Ui_Skill_Select_Btn>();
            sc.skilltype = (Ui_Skill_Select_Btn.SkillType)Skill_ID;
            sc.F_Set_SelectCard(Lv_Value);
            Btn[i].gameObject.SetActive(true);


            // ���������� ����
            Skill_ID_List.RemoveAt(Skill_ID_List[RandomValue]);
            Skill_Lv.RemoveAt(Skill_Lv[RandomValue]);
        }

        Skill_ID_List.Clear();
        Skill_Lv.Clear();

        //�������� ����
        if (LvupWindow.activeSelf == false)
        {
            LvupWindow.SetActive(true);
        }
    }

}
