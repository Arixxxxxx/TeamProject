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


    [Header("# Input Skill Spec  ==>  # ���� ")]
    [Space]
    [SerializeField] int Skill_Max_Lvl;
    public int skill_Max_Lvl { get { return Skill_Max_Lvl; } }

    [SerializeField] float critical_Value;
    [Header("# Set Attack Skill Value")]
    [Space]
    [SerializeField] int Skill_0_Level;
    [SerializeField] Skill[] Skill_0_Value;
    [Space]
    [SerializeField] int Skill_1_Level;
    [SerializeField] Skill[] Skill_1_Value;
    [Space]
    [SerializeField] int Skill_2_Level;
    [SerializeField] Skill[] Skill_2_Value;
    [Space]
    [Header("# Set Passive Skill Value")]
    [Space]
    [SerializeField] int Passive_0_Lv;
    [SerializeField] float[] Passive_0_UpValue;

    Transform Skill_Start_Point;
    Skill_Ui_UpdaterSystem _updaterSystem;
    Movement charMove_Sc;

    int[] Skill_Lv = new int[10];


    bool Alpha_1_Input;
    bool Alpha_2_Input;
    bool Alpha_3_Input;
    bool Alpha_6_Input;

    private void Awake()
    {
        _updaterSystem = GetComponent<Skill_Ui_UpdaterSystem>();
        Skill_Start_Point = transform.Find("Skill_Start_Point").GetComponent<Transform>();
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
        

    }

    private void Skill_0_Lvelup()
    {
        if (Skill_0_Level < Skill_Max_Lvl) // ������
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
                _updaterSystem.F_Set_ActiveCheak(0); // UI ������ �־��ֱ�
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

            if (i < skill_Slot[0].transform.childCount) //  ī��Ʈ ���� ������ �մ°� ��Ȱ��
            {
                bullet = skill_Slot[0].transform.GetChild(i);
            }
            else
            {
                GameObject obj = Instantiate(skill_Obj[0], skill_Slot[0]); // ����
                bullet = obj.transform;
            }
            bullet.transform.localPosition = Vector3.zero;
            bullet.transform.eulerAngles = Vector3.zero;

            bullet.GetComponent<Dmg_Object>().F_SetSkill_DMG(Skill_0_Value[(int)Lv].dmg); // ����� ����

            Vector3 rotvec = Vector3.forward * 360 * i / Skill_0_Value[(int)Lv].count;
            bullet.transform.Rotate(rotvec); // ȸ��
            bullet.transform.Translate(Vector3.up * Skill_0_Value[(int)Lv].range, Space.Self); // ����

            skill_Slot[0].GetComponent<Transform_Z_Rotaion>().F_Set_SpinSpeed(Skill_0_Value[(int)Lv].speed); // ȸ���ӵ� ����


        }
    }

    private void Skill_1_Lvelup()
    {
        if (Skill_1_Level < Skill_Max_Lvl) // ������
        {
            Skill_1_Level++;

            if (Skill_1_Level == 1)
            {
                _updaterSystem.F_Set_ActiveCheak(1); // UI ������ �־��ֱ�
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
        if (Skill_1_Level == 0)  // ���� 0 ����
        {
            if (skill_1_ShotCount == 0)
            {
                skill_1_ShotCount = 1; //���� 1���ڸ��� ��ù߻��Ҽ��ְ� ä����
            }
            return;
        }

        skill_1_ShotTimer = Skill_1_Value[Skill_1_Level - 1].cooltime; // ��Ÿ�� �ʱ�ȭ

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
        else if (Skill_2_Level < Skill_Max_Lvl) // ������
        {
            Skill_2_Level++;

            if (Skill_2_Level == 1)
            {
                _updaterSystem.F_Set_ActiveCheak(2); // UI ������ �־��ֱ�

                if (skill_Obj[2].gameObject.activeSelf == false)
                {
                    skill_Obj[2].SetActive(true);
                }
            }

            if (Skill_2_Level > 0)
            {
                skill_Obj[2].GetComponent<Dmg_Object>().F_SetSkill_DMG(Skill_2_Value[Skill_2_Level - 1].dmg);
            }
        }
    }


    /////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////// Passive /////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////


    private void Passive_0_LvUp() // ��ú� 1�� �̵��ӵ�����
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




    /////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////// ETC /////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////

    private void Input_Cheaker()
    {
        Alpha_1_Input = Input.GetKeyDown(KeyCode.Alpha1);
        Alpha_2_Input = Input.GetKeyDown(KeyCode.Alpha2);
        Alpha_3_Input = Input.GetKeyDown(KeyCode.Alpha3);
        Alpha_6_Input = Input.GetKeyDown(KeyCode.Alpha6);
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
        if (Alpha_6_Input)
        {
            Passive_0_LvUp();
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
                return Passive_0_Lv;

            case 2:
                return Passive_0_Lv;

        }

        return -1;
    }

    public float F_GetSkillDMG() // ���߿� �ٸ� ��ų�� �ʿ��ҽ� �Ű����� �ְ� ����ġ �����
    {
        if (Skill_1_Level == 0) { return 0; }
        return Skill_1_Value[Skill_1_Level - 1].dmg;
    }

    private void SkillLv_Updater()
    {
        Skill_Lv[0] = Skill_0_Level;
        Skill_Lv[1] = Skill_1_Level;
        Skill_Lv[2] = Skill_2_Level;
        Skill_Lv[5] = Passive_0_Lv;
    }

    public int[] F_Get_CurSkillLv()
    {
        SkillLv_Updater();

        return Skill_Lv;
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




