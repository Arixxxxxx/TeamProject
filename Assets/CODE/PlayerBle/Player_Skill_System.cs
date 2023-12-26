using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] float critical_Value;
    [Header("# Set Skill Value")]
    [SerializeField] int Skill_0_Level;
    [SerializeField] Skill[] Skill_0_Value;


    Skill_Ui_UpdaterSystem _updaterSystem;

    bool Alpha_1_Input;

    private void Awake()
    {
        _updaterSystem = GetComponent<Skill_Ui_UpdaterSystem>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Input_Cheaker();
        ActiveSKill_KeyDown();
        Play_Skill_01();
        Skill_0_LvelupSystem();
    }
    bool once;
    private void Skill_0_LvelupSystem()
    {
        if (Skill_0_Level == 0) { return; }

        else if (Skill_0_Level > 0)    { skill_Slot[0].gameObject.SetActive(true); }
        

        switch (Skill_0_Level)
        {
            case 1:

                if (once == false)
                {
                    _updaterSystem.F_Set_ActiveCheak(0);
                    once = true;
                    Skill_0_Instantiate(Skill_0_Level - 1);
                }
                    
            break;

            case 2:
                if (once == false)
                {
                    once = true;
                    Skill_0_Instantiate(Skill_0_Level - 1);
                }

                break;

            case 3:
                if (once == false)
                {
                    once = true;
                    Skill_0_Instantiate(Skill_0_Level - 1);
                }

                break;

            case 4:
                if (once == false)
                {
                    once = true;
                    Skill_0_Instantiate(Skill_0_Level - 1);
                }

                break;

            case 5:
                if (once == false)
                {
                    once = true;
                    Skill_0_Instantiate(Skill_0_Level - 1);
                }

                break;
        }

    }

    private void Skill_0_Instantiate(float Lv)
    {
        for (int i = 0; i < Skill_0_Value[(int)Lv].count; i++)
        {
            Transform bullet;

            if ( i < skill_Slot[0].transform.childCount) //  카운트 보다 작으면 잇는거 재활용
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

            bullet.GetComponent<Dmg_Object>().F_SetSkill_DMG(Skill_0_Value[(int)Lv].dmg); // 대미지 수정

            Vector3 rotvec = Vector3.forward * 360 * i / Skill_0_Value[(int)Lv].count;
            bullet.transform.Rotate(rotvec); // 회전
            bullet.transform.Translate(Vector3.up * Skill_0_Value[(int)Lv].range,  Space.Self); // 범위

            skill_Slot[0].GetComponent<Transform_Z_Rotaion>().F_Set_SpinSpeed(Skill_0_Value[(int)Lv].speed); // 회전속도 셋팅


        }
    }

    private void Input_Cheaker()
    {
        Alpha_1_Input = Input.GetKeyDown(KeyCode.Alpha1);
    }


    private void ActiveSKill_KeyDown()
    {
        if (Alpha_1_Input && Skill_0_Level < Skill_Max_Lvl)
        {
            Skill_0_Level++;
            once = false;
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

