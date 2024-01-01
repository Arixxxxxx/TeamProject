using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvUp_Ui_Manager : MonoBehaviour
{

    [Header("# Insert Window Obj in Hierarchy")]
    [Space]
    [SerializeField] GameObject LvupWindow;
    [SerializeField] GameObject Prefabs;
    [Header("# Cheak value")]
    [Space]
    [SerializeField] int[] Get_Skill_Value = new int[10];
    [SerializeField] List<int> Skill_ID_List = new List<int>();
    [SerializeField] List<int> Skill_Lv = new List<int>();
    Transform Slot;
    Player_Skill_System Skill_sc;
    int SkillMaxLv;

    private void Awake()
    {
        Slot = LvupWindow.transform.Find("Box/Slot_Box").GetComponent<Transform>();
    }
    void Start()
    {
        Skill_sc = Hub.Inst.player_skill_system_sc;
        SkillMaxLv = Skill_sc.skill_Max_Lvl;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            F_OpenWindow();
        }
    }

    public void F_OpenWindow()
    {
        
        Get_Skill_Value = Skill_sc.F_Get_CurSkillLv(); // ���� ���� �ʱ�ȭ �Լ�

        // �̰����� ����
        for(int i = 0; i < Get_Skill_Value.Length; i++)
        {
            if (Get_Skill_Value[i] < SkillMaxLv)
            {
                Skill_ID_List.Add(i);
                Skill_Lv.Add(Get_Skill_Value[i]);
            }
        }

        for (int i = 0; i < 3; i++)
        {
            int RandomValue = Random.Range(0, Skill_ID_List.Count); // ����ī��Ʈ����
            int Skill_ID = Skill_ID_List[RandomValue]; // ����
            int Lv_Value = Skill_Lv[RandomValue]; //��ų����
                        
                       

            GameObject obj = Instantiate(Prefabs, Slot);
            Ui_Skill_Select_Btn sc = obj.GetComponent<Ui_Skill_Select_Btn>();
            sc.skilltype = (Ui_Skill_Select_Btn.SkillType)Skill_ID;
            sc.F_Set_SelectCard(Lv_Value);

            // ���������� ����
            Skill_ID_List.Remove(Skill_ID_List[RandomValue]);
            Skill_Lv.Remove(Skill_Lv[RandomValue]);
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
