using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class Player_Skill_System : MonoBehaviour
{
    [Header("# Insert Skill Object in Hierarchy")]
    [Space]
    [SerializeField] Transform[] skill_Slot;
    [SerializeField] GameObject[] skill_Obj;


    [Header("# Input Skill Spec  ==>  # 예진 ")]
    [Space]
    [SerializeField] int Skill_Max_Lvl;
    public int skill_Max_Lvl { get { return Skill_Max_Lvl; } }

    [SerializeField] float critical_Value;
    [Header("# Set Attack Skill Value")]
    [Space]
    [SerializeField] int Skill_0_Level; // 파이어볼
    [SerializeField] Skill[] Skill_0_Value;
    [Space]
    [SerializeField] int Skill_1_Level; // 화염분수
    [SerializeField] Skill[] Skill_1_Value;
    [Space]
    [SerializeField] int Skill_2_Level; // 화염보호막
    [SerializeField] Skill[] Skill_2_Value;
    [Space]
    [SerializeField] int Skill_3_Level; // 메테오
    [SerializeField] Skill[] Skill_3_Value;
    [Space]
    [SerializeField] int Skill_4_Level; // 화염풍
    [SerializeField] Skill[] Skill_4_Value;
    [Space]
    [Header("# Set Passive Skill Value")]
    [Space]
    [SerializeField] int Passive_0_Lv; // 이동속도
    [SerializeField] float[] Passive_0_UpValue;
    [Space]
    [SerializeField] int Passive_1_Lv; // 체력증가
    [SerializeField] float[] Passive_1_UpValue;
    [Space]
    [SerializeField] int Passive_2_Lv; // 체력자동회복
    [SerializeField] float[] Passive_2_Time;
    [SerializeField] float[] Passive_2_Hp;
    [Space]
    [SerializeField] int Passive_3_Lv; // 순간이동 
    [SerializeField] float[] Passive_3_Tel_Distance_AddValue;
    [Space]
    [SerializeField] int Passive_4_Lv; // 공격력증가 5%씩
    [SerializeField] float[] Passive_4_AttackPowerAdd;



    Transform S_3_StartPos;
    Transform Skill_Start_Point;
    Skill_Ui_UpdaterSystem _updaterSystem;
    Movement charMove_Sc;

    int[] Skill_Lv = new int[10];


    bool Alpha_1_Input;
    bool Alpha_2_Input;
    bool Alpha_3_Input;
    bool Alpha_4_Input;
    bool Alpha_5_Input;
    bool Alpha_6_Input;
    bool Alpha_7_Input;
    bool Alpha_8_Input;
    bool Alpha_9_Input;
    bool Alpha_10_Input;

    private void Awake()
    {
        _updaterSystem = GetComponent<Skill_Ui_UpdaterSystem>();
        Skill_Start_Point = transform.Find("Skill_Start_Point").GetComponent<Transform>();
        S_3_StartPos = transform.Find("Skill_LIst/S_3").GetComponent<Transform>();
    }
    void Start()
    {
        charMove_Sc = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        Input_Cheaker();
        

        ActiveSKill_Test_KeyDown();
        Play_Skill_01();
        Skill_1_AutoFire();
        Skill_3_AutoFire();
        Skill_4_AutoFire();
    }

    private void Skill_0_Lvelup()
    {
        if (Skill_0_Level < Skill_Max_Lvl) // 레벨업
        {
            Skill_0_Level++;
        }
        else if (Skill_0_Level == Skill_Max_Lvl)
        {
            return;
        }

        if (skill_Slot[0].gameObject.activeSelf == false)
        {
            skill_Slot[0].gameObject.SetActive(true);
        }


        switch (Skill_0_Level)
        {
            case 1:
                _updaterSystem.F_Set_ActiveCheak(0); // UI 프리펩 넣어주기
                Skill_0_Instantiate(Skill_0_Level - 1);
                break;

            case 2:
                Skill_0_Instantiate(Skill_0_Level - 1);
                break;

            case 3:
                Skill_0_Instantiate(Skill_0_Level - 1);
                break;

            case 4:
                Skill_0_Instantiate(Skill_0_Level - 1);
                break;

            case 5:
                Skill_0_Instantiate(Skill_0_Level - 1);
                break;
        }
    }

    private void Skill_0_Instantiate(float Lv)
    {
        for (int i = 0; i < Skill_0_Value[(int)Lv].count; i++)
        {
            Transform bullet;

            if (i < skill_Slot[0].transform.childCount) //  카운트 보다 작으면 잇는거 재활용
            {
                bullet = skill_Slot[0].transform.GetChild(i);
            }
            else
            {
                GameObject obj = Instantiate(skill_Obj[0], skill_Slot[0]); // 생성
                bullet = obj.transform;
            }

            bullet.transform.localPosition = Vector3.zero;
            bullet.transform.eulerAngles = Vector3.zero;

            //bullet.GetComponent<Dmg_Object>().F_SetSkill_DMG
            //(Skill_0_Value[(int)Lv].dmg); // 스킬파워 + 패시브파워

            Vector3 rotvec = Vector3.forward * 360 * i / Skill_0_Value[(int)Lv].count;
            bullet.transform.Rotate(rotvec); // 회전
            bullet.transform.Translate(Vector3.up * Skill_0_Value[(int)Lv].range, Space.Self); // 범위

            skill_Slot[0].GetComponent<Transform_Z_Rotaion>().F_Set_SpinSpeed(Skill_0_Value[(int)Lv].speed); // 회전속도 셋팅


        }
    }

    private void Skill_1_Lvelup()
    {
        if (Skill_1_Level < Skill_Max_Lvl) // 레벨업
        {
            Skill_1_Level++;

            if (Skill_1_Level == 1)
            {
                _updaterSystem.F_Set_ActiveCheak(1); // UI 프리펩 넣어주기
            }

        }
        else if (Skill_1_Level == Skill_Max_Lvl)
        {
            return;
        }
    }

    float skill_1_ShotCount;
    float skill_1_ShotTimer;
    private void Skill_1_AutoFire()
    {
        if (Skill_1_Level == 0)  // 레벨 0 리턴
        {
            if (skill_1_ShotCount == 0)
            {
                skill_1_ShotCount = 1; //레벨 1되자마자 즉시발사할수있게 채워줌
            }
            return;
        }

        skill_1_ShotTimer = Skill_1_Value[Skill_1_Level - 1].cooltime; // 쿨타임 초기화

        skill_1_ShotCount += Time.deltaTime;

        if (skill_1_ShotCount > skill_1_ShotTimer)
        {
            skill_1_ShotCount = 0;
            GameObject obj = PoolManager.Inst.F_GetPlayerBullet(0);
            obj.transform.position = Skill_Start_Point.position;
            obj.SetActive(true);
        }
    }


    private void Skill_2_Lvelup()
    {
        if (Skill_2_Level == Skill_Max_Lvl)
        {
            return;
        }
        else if (Skill_2_Level < Skill_Max_Lvl) // 레벨업
        {
            Skill_2_Level++;

            if (Skill_2_Level == 1)
            {
                _updaterSystem.F_Set_ActiveCheak(2); // UI 프리펩 넣어주기

                if (skill_Obj[2].gameObject.activeSelf == false)
                {
                    skill_Obj[2].SetActive(true);
                }
            }

            //if (Skill_2_Level > 0)
            //{
            //    skill_Obj[2].GetComponent<Dmg_Object>().F_SetSkill_DMG(Skill_2_Value[Skill_2_Level - 1].dmg * Passive_4_AttackPowerAdd[Passive_4_Lv]);
            //}
        }
    }


    private void Skill_3_Lvelup()
    {
        if (Skill_3_Level < Skill_Max_Lvl) // 레벨업
        {
            Skill_3_Level++;

            if (Skill_3_Level == 1)
            {
                _updaterSystem.F_Set_ActiveCheak(3); // UI 프리펩 넣어주기
            }

        }
        else if (Skill_3_Level == Skill_Max_Lvl)
        {
            return;
        }
    }

    float skill_3_ShotCount;
    float skill_3_ShotTimer;
    [SerializeField] float s3_StartAddX, s3_StartAddY;
    private void Skill_3_AutoFire()
    {
        if (Skill_3_Level == 0)  // 레벨 0 리턴
        {
            if (skill_3_ShotCount == 0)
            {
                skill_3_ShotCount = 1; //레벨 1되자마자 즉시발사할수있게 채워줌
            }
            return;
        }

        skill_3_ShotTimer = Skill_3_Value[Skill_3_Level - 1].cooltime; // 쿨타임 초기화

        skill_3_ShotCount += Time.deltaTime;

        if (skill_3_ShotCount > skill_3_ShotTimer)
        {
            skill_3_ShotCount = 0;
            GameObject obj = PoolManager.Inst.F_GetPlayerBullet(2);
            Vector3 ransPos = Set_RandomValue();
            obj.transform.position = ransPos + new Vector3(s3_StartAddX, s3_StartAddY);
            obj.SetActive(true);
        }
    }
    
    float RanX, RanY;
    private Vector3 Set_RandomValue()
    {
        int randomX = Random.Range(0, 2);
        int randomY = Random.Range(0, 2);

        if (randomX == 0)
        {
            RanX = Random.Range(-1f, -8.0f);
        }
        else if (randomX == 1)
        {
            RanX = Random.Range(1f, 10.0f);
        }

        if (randomY == 0)
        {
            RanY = Random.Range(2f, -4f);
        }
        else if (randomY == 1)
        {
            RanY = Random.Range(5f, 9.0f);
        }
        

        Vector3 Pos = new Vector3(transform.position.x + RanX, transform.position.y + RanY);

        return Pos;
    }

    

    private void Skill_4_Lvelup()
    {
        if (Skill_4_Level < Skill_Max_Lvl) // 레벨업
        {
            Skill_4_Level++;

            if (Skill_4_Level == 1)
            {
                _updaterSystem.F_Set_ActiveCheak(4); // UI 프리펩 넣어주기
            }

        }
        else if (Skill_4_Level == Skill_Max_Lvl)
        {
            return;
        }
    }

    float skill_4_ShotCount;
    float skill_4_ShotTimer;
    private void Skill_4_AutoFire()
    {
        if (Skill_4_Level == 0)  // 레벨 0 리턴
        {
            if (skill_4_ShotCount == 0)
            {
                skill_4_ShotCount = 1; //레벨 1되자마자 즉시발사할수있게 채워줌
            }
            return;
        }

        skill_4_ShotTimer = Skill_4_Value[Skill_4_Level - 1].cooltime; // 쿨타임 초기화

        skill_4_ShotCount += Time.deltaTime;

        if (skill_4_ShotCount > skill_4_ShotTimer)
        {
            skill_4_ShotCount = 0;
            GameObject obj = PoolManager.Inst.F_GetPlayerBullet(4);

            obj.transform.position = transform.position;
            obj.SetActive(true);
        }
    }

    /////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////// Passive /////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////


    private void Passive_0_LvUp() // 페시브 0번 이동속도증가
    {
        if (Passive_0_Lv == Skill_Max_Lvl)
        {
            return;
        }
        else if (Passive_0_Lv < Skill_Max_Lvl)
        {
            Passive_0_Lv++;

            if (Passive_0_Lv == 1)
            {
                _updaterSystem.F_Set_PassiveCheak(0);
            }

            if (Passive_0_Lv > 0)
            {
                charMove_Sc.F_SetMoveSpeedAdd(Passive_0_UpValue[Passive_0_Lv - 1]);
            }
        }
    }

    private void Passive_1_LvUp() // 페시브 1번 체력증가
    {
        if (Passive_1_Lv == Skill_Max_Lvl)
        {
            return;
        }
        else if (Passive_1_Lv < Skill_Max_Lvl)
        {
            Passive_1_Lv++;

            if (Passive_1_Lv == 1)
            {
                _updaterSystem.F_Set_PassiveCheak(1);
            }

            if (Passive_1_Lv > 0)
            {
                Player_Stats sc =  Hub.Inst.Player_Status_sc;
                sc.F_Set_Add_Hp_Passiveskill(Passive_1_UpValue[Passive_1_Lv - 1]);
                    
            }
        }
    }

    private void Passive_2_LvUp() // 페시브 1번 체력자동회복
    {
        if (Passive_2_Lv == Skill_Max_Lvl)
        {
            return;
        }
        else if (Passive_2_Lv < Skill_Max_Lvl)
        {
            Passive_2_Lv++;

            if (Passive_2_Lv == 1)
            {
                _updaterSystem.F_Set_PassiveCheak(2);
            }

            if (Passive_2_Lv > 0)
            {
                Player_Stats sc = Hub.Inst.Player_Status_sc;
                sc.F_HpRecoveryPassive_LvUp(Passive_2_Time[Passive_2_Lv-1], Passive_2_Hp[Passive_2_Lv-1]);
            }
        }
    }

    private void Passive_3_LvUp() // 페시브 1번 체력자동회복
    {
        if (Passive_3_Lv == Skill_Max_Lvl)
        {
            return;
        }
        else if (Passive_3_Lv < Skill_Max_Lvl)
        {
            Passive_3_Lv++;

            if (Passive_3_Lv == 1)
            {
                _updaterSystem.F_Set_PassiveCheak(3);
            }

            if (Passive_3_Lv > 0)
            {
                Movement sc = Hub.Inst.Movement_sc;
                sc.F_Set_Add_TelePortDistance(Passive_3_Tel_Distance_AddValue[Passive_3_Lv - 1]);
            }
        }
    }


    private void Passive_4_LvUp() // 페시브 5번 스태미너량 증가 -> 모든공격력증가 변경
    {
        if (Passive_4_Lv == Skill_Max_Lvl)
        {
            return;
        }
        else if (Passive_4_Lv < Skill_Max_Lvl)
        {
            Passive_4_Lv++;

            if (Passive_4_Lv == 1)
            {
                _updaterSystem.F_Set_PassiveCheak(4);
            }

            if (Passive_4_Lv > 0)
            {
                //Movement sc = Hub.Inst.Movement_sc;
                //sc.F_SetSprintTimeAdd(Passive_4_MaxSprintUpValue[Passive_4_Lv - 1]);


            }
        }
    }
    /////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////// ETC /////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////

    private void Input_Cheaker()
    {
        Alpha_1_Input = Input.GetKeyDown(KeyCode.Alpha1);
        Alpha_2_Input = Input.GetKeyDown(KeyCode.Alpha2);
        Alpha_3_Input = Input.GetKeyDown(KeyCode.Alpha3);
        Alpha_4_Input = Input.GetKeyDown(KeyCode.Alpha4);
        Alpha_5_Input = Input.GetKeyDown(KeyCode.Alpha5);
        Alpha_6_Input = Input.GetKeyDown(KeyCode.Alpha6);
        Alpha_7_Input = Input.GetKeyDown(KeyCode.Alpha7);
        Alpha_8_Input = Input.GetKeyDown(KeyCode.Alpha8);
        Alpha_9_Input = Input.GetKeyDown(KeyCode.Alpha9);
        Alpha_10_Input = Input.GetKeyDown(KeyCode.Alpha0);
    }


    private void ActiveSKill_Test_KeyDown()
    {
        if (Alpha_1_Input)
        {
            Skill_0_Lvelup();
        }

        if (Alpha_2_Input)
        {
            Skill_1_Lvelup();
        }

        if (Alpha_3_Input)
        {
            Skill_2_Lvelup();
        }

        if (Alpha_4_Input)
        {
            Skill_3_Lvelup();
        }
        if (Alpha_5_Input)
        {
            Skill_4_Lvelup();
        }

        if (Alpha_6_Input)
        {
            Passive_0_LvUp();
        }

        if (Alpha_7_Input)
        {
            Passive_1_LvUp();
        }

        if (Alpha_8_Input)
        {
            Passive_2_LvUp();
        }


        if (Alpha_9_Input)
        {
            Passive_3_LvUp();
        }

        if (Alpha_10_Input)
        {
            Passive_4_LvUp();
        }
    }
    /// <summary>
    /// 레벨업Btn에서 받아다씀
    /// </summary>
    /// <param name="value">스킬 ID</param>
    public void F_Skill_LvUp(int value)
    {
        switch (value)
        {
            case 0:
                Skill_0_Lvelup();
                break;
                case 1:
                Skill_1_Lvelup();
                break;
                case 2:
                Skill_2_Lvelup();
                break;
                case 3:
                Skill_3_Lvelup();
                break;
                case 4:
                Skill_4_Lvelup();
                break;
                case 5:
                Passive_0_LvUp();
                break;
                case 6:
                Passive_1_LvUp();
                break;
                case 7:
                Passive_2_LvUp();
                break;
                case 8:
                Passive_3_LvUp();
                break;
                case 9:
                Passive_4_LvUp();
                break;

        }
    }


    private void Play_Skill_01()
    {
        if (skill_Obj[0].gameObject.activeSelf == true)
        {
            skill_Obj[0].transform.eulerAngles += Vector3.forward * Time.deltaTime;
        }
    }

    public float F_Get_Player_Critical()
    {
        return critical_Value;
    }

    public int F_Get_Attack_Skill_Lv(int type)
    {
        switch (type)
        {
            case 0:
                return Skill_0_Level;

            case 1:
                return Skill_1_Level;

            case 2:
                return Skill_2_Level;

            case 3:
                return Skill_3_Level;

            case 4:
                return Skill_4_Level;

        }

        return -1;
    }

    public int F_Get_Passive_Skill_Lv(int type)
    {
        switch (type)
        {
            case 0:
                return Passive_0_Lv;

            case 1:
                return Passive_1_Lv;

            case 2:
                return Passive_2_Lv;

            case 3:
                return Passive_3_Lv;

            case 4:
                return Passive_4_Lv;

        }

        return -1;
    }

    public float F_GetSkillDMG(int Skill_ID) // 나중에 다른 스킬도 필요할시 매개변수 넣고 스위치 떙기셈
    {
        switch (Skill_ID) 
        {
            case 0:
                if (Skill_0_Level == 0) { return 0; }
                return Skill_0_Value[Skill_0_Level - 1].dmg * Passive_4_AttackPowerAdd[Passive_4_Lv];

            case 1:
                if (Skill_1_Level == 0) { return 0; }
                return Skill_1_Value[Skill_1_Level - 1].dmg * Passive_4_AttackPowerAdd[Passive_4_Lv];

            case 2:
                if (Skill_2_Level == 0) { return 0; }
                return Skill_2_Value[Skill_2_Level - 1].dmg * Passive_4_AttackPowerAdd[Passive_4_Lv];

            case 3:
                if (Skill_3_Level == 0) { return 0; }
                return Skill_3_Value[Skill_3_Level - 1].dmg * Passive_4_AttackPowerAdd[Passive_4_Lv];

            case 4:
                if (Skill_4_Level == 0) { return 0; }
                return Skill_4_Value[Skill_4_Level - 1].dmg * Passive_4_AttackPowerAdd[Passive_4_Lv];

        }
        return -1;
        
    }

    public float F_Get_Skill2_CoolTime()
    {
        return Skill_2_Value[Skill_2_Level - 1].cooltime;
    }

    private void SkillLv_Updater()
    {
        Skill_Lv[0] = Skill_0_Level;
        Skill_Lv[1] = Skill_1_Level;
        Skill_Lv[2] = Skill_2_Level;
        Skill_Lv[3] = Skill_3_Level;
        Skill_Lv[4] = Skill_4_Level;
        Skill_Lv[5] = Passive_0_Lv;
        Skill_Lv[6] = Passive_1_Lv;
        Skill_Lv[7] = Passive_2_Lv;
        Skill_Lv[8] = Passive_3_Lv;
        Skill_Lv[9] = Passive_4_Lv;
        
    }

    public int[] F_Get_CurSkillLv()
    {
        SkillLv_Updater();

        return Skill_Lv;
    }

    float[] skill4value = new float[3];

    /// <summary>
    /// Return 0 Du / 1Speed / 2Range
    /// </summary>
    /// <returns></returns>
    public float[] F_Get_Skill_4_Value()
    {
        if(Skill_4_Level == 0) { return null; }

        skill4value[0] = Skill_4_Value[Skill_4_Level-1].duration;
        skill4value[1] = Skill_4_Value[Skill_4_Level-1].speed;
        skill4value[2] = Skill_4_Value[Skill_4_Level-1].range;

        return skill4value;
    }
}

[System.Serializable]
public class Skill
{
    public float dmg;
    public float count;
    public float speed;
    public float range;
    public float cooltime;
    public float duration;
}




