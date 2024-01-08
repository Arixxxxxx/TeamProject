using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvUp_Ui_Manager : MonoBehaviour
{
    public static LvUp_Ui_Manager Inst;

    [Header("# Insert Window Obj in Hierarchy")]
    [Space]
    [SerializeField] GameObject LvupWindow;
    [SerializeField] GameObject[] Btn;
    [Header("# 공격스킬만나올 레벨 제한선")]
    [Space]
    [SerializeField] int OnlyAttackSkillGet_Lv;
    [Header("# Cheak value")]
    [Space]
    [SerializeField] int[] Get_Skill_Value = new int[10];
    [SerializeField] List<int> Skill_ID_List = new List<int>();
    [SerializeField] List<int> Skill_Lv = new List<int>();
    [SerializeField] List<int> Cheak_List = new List<int>();
    Dictionary<int, int> skill_ID_Value = new Dictionary<int, int>();
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
        skill_ID_Value.Clear(); // 딕셔러니 초기화
        //Skill_ID_List.Clear();  //리스트에서 딕셔너리로 옴김
        //Skill_Lv.Clear();
        Cheak_List.Clear();
        SkillCount = 0;

        GameManager.Inst.F_Lvup_Btn_OnOff(1); // 버튼 잠금해제

        Get_Skill_Value = Skill_sc.F_Get_CurSkillLv(); // 현재 레벨 초기화 함수
        PlayerLv = status.F_Get_Player_LV(); // 플레이어 레벨 가져옴

        // 스킬 맥스레벨보다 낮은 스킬들 다 가져오기
        for (int i = 0; i < Get_Skill_Value.Length; i++)
        {
            if (Get_Skill_Value[i] < SkillMaxLv)
            {
                skill_ID_Value.Add(i, Get_Skill_Value[i]);
                //Skill_ID_List.Add(i);
                //Skill_Lv.Add(Get_Skill_Value[i]);
            }
        }

        if (skill_ID_Value.Count >= 3)
        {
            SkillCount = 3;
        }
        else if (skill_ID_Value.Count == 2)
        {
            SkillCount = 2;
        }
        else if (skill_ID_Value.Count == 1)
        {
            SkillCount = 1;
        }
        else if (skill_ID_Value.Count == 0)
        {
            Debug.Log("만렙");
            return;
        }

        int[] RandomValue = new int[SkillCount];

        if (PlayerLv < OnlyAttackSkillGet_Lv)
        {
            RandomValue = MakeRandomValue(5);
        }
        else if (PlayerLv >= OnlyAttackSkillGet_Lv)
        {
            RandomValue = DictionaryRandomKey(skill_ID_Value , SkillCount);
        }

        // 생성
        for (int i = 0; i < SkillCount; i++)
        {
         
            //int Skill_ID = Skill_ID_List[RandomValue[i]]; // 종류
            //int Lv_Value = Skill_Lv[RandomValue[i]]; //스킬레벨
            
            Ui_Skill_Select_Btn sc = Btn[i].GetComponent<Ui_Skill_Select_Btn>();
            sc.skilltype = (Ui_Skill_Select_Btn.SkillType)RandomValue[i];
            //sc.skilltype = (Ui_Skill_Select_Btn.SkillType)Skill_ID;

            sc.F_Set_SelectCard(skill_ID_Value[RandomValue[i]]);
            //sc.F_Set_SelectCard(Lv_Value);
            Btn[i].gameObject.SetActive(true);

            // 저장햇으니 지움
            //skill_ID_Value.Remove(RandomValue[i]);
            //Skill_ID_List.RemoveAt(Skill_ID_List[RandomValue[i]]);
            //Skill_Lv.RemoveAt(Skill_Lv[RandomValue[i]]);

        }

        if (LvupWindow.activeSelf == false)
        {
            LvupWindow.SetActive(true);
        }
    }

    /// <summary>
    /// 공격 인카운트만 선택시 
    /// </summary>
    /// <param name="Length">0~범위</param>
    /// <returns></returns>
    private int[] MakeRandomValue(int Length)
    {
        Cheak_List.Clear(); // 초기화
        int[] result = new int[3]; // 기본적으로 3개 스킬등록

            for(int i = 0; i < 3; i++)
            {
                if (i == 0)  // 첫번째는 무조건 패스
                {
                    result[i] = Random.Range(0, Length);
                    Cheak_List.Add(result[i]);
                }
                else if( i > 0) // 2,3번은 리스트에 담아서 같은 랜덤값이 나오면 다시 굴림
                {
                    int Ranvalue = Random.Range(0, Length);

                    while (Cheak_List.Contains(Ranvalue))
                    {
                        Ranvalue = Random.Range(0, Length);
                    }

                    result[i] = Ranvalue;

                    Cheak_List.Add(result[i]);
                }
            }
                   
        return result;
    }

    Dictionary<int, int> Temp = new Dictionary<int, int>();
    int SkillCount;
    /// <summary>
    /// 모든 선택지 고르기
    /// </summary>
    /// <param name="dic"></param>
    /// <param name="skillCount"></param>
    /// <returns></returns>
    private int[] DictionaryRandomKey(Dictionary<int,int> dic, int skillCount)
    {
        int[] result = new int[skillCount]; 
      
        Temp.Clear();

         foreach( var kvp in dic)
        {
            Temp.Add(kvp.Key, kvp.Value);
        }
        
        for (int i = 0;i < skillCount; i++)
        {
            int RandomValue = Random.Range(0, 10);

            while (Temp.ContainsKey(RandomValue) == false)
            {
                RandomValue = Random.Range(0, 10);
            }
            
            result[i] = RandomValue;
            Temp.Remove(RandomValue);
        }

        return result;
    }
}
