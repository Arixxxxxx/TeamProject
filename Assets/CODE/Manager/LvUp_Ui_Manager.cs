using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class LvUp_Ui_Manager : MonoBehaviour
{

    [Header("# Insert Window Obj in Hierarchy")]
    [Space]
    [SerializeField] GameObject LvupWindow;
    [SerializeField] GameObject Prefabs;
    [Header("# Cheak value")]
    [Space]
    [SerializeField] int[] SkillLv = new int[10];
    [SerializeField] List<int> SkillNumber = new List<int>();
    [SerializeField] List<int> SkillValue = new List<int>();
    Transform Slot;
    Player_Skill_System Skill_sc;


    private void Awake()
    {
        Slot = LvupWindow.transform.Find("Box/Slot_Box").GetComponent<Transform>();
    }
    void Start()
    {
        Skill_sc = Hub.Inst.player_skill_system_sc;

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
        //if(Skill_sc = null) { Skill_sc = Hub.Inst.player_skill_system_sc; }

        SkillLv = Skill_sc.F_Get_CurSkillLv(); // ���� ���� �ʱ�ȭ �Լ�

        // �̰����� ����
        for(int i = 0; i < SkillLv.Length; i++)
        {
            if (SkillLv[i] < Skill_sc.skill_Max_Lvl)
            {

                SkillNumber.Add(i);
                SkillValue.Add(SkillLv[i]);
            }
        }

        for (int i = 0; i < 3; i++)
        {
            int RandomValue = Random.Range(0, SkillNumber.Count); // ����ī��Ʈ����
            int Skill_ID = SkillNumber[RandomValue]; // ����
            int ID_Value = SkillValue[RandomValue]; //ī��Ʈ�� ����մ°�
                        

            //���⼭ �߿��Ѱ� ��ų���̵� ����

            GameObject obj = Instantiate(Prefabs, Slot);
            Ui_Skill_Select_Btn sc = obj.GetComponent<Ui_Skill_Select_Btn>();
            sc.skilltype = (Ui_Skill_Select_Btn.SkillType)Skill_ID;
            sc.F_Set_SelectCard(ID_Value);

            // ���������� ����
            SkillNumber.Remove(SkillNumber[RandomValue]);
            SkillValue.Remove(SkillValue[RandomValue]);
        }

        SkillNumber.Clear();

        //�������� ����
        if (LvupWindow.activeSelf == false)
        {
            LvupWindow.SetActive(true);
        }
    }

    private void ButtonInIt()
    {

        int[] ����; 


    }

    

    

}
