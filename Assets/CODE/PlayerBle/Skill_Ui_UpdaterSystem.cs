using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Ui_UpdaterSystem : MonoBehaviour
{
    [Header("# Insert Object in Inspecter")]
    [Space]
    [SerializeField] Transform activeSkill_Ui_Trs;
    [SerializeField] Transform passiveSkill_Ui_Trs;
    [SerializeField] GameObject UI_icon_Prefab;
    [SerializeField] Sprite[] icon_Img;
    [SerializeField] Sprite[] Passive_icon_Img;
        
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void F_Set_ActiveCheak(int value)
    {
        GameObject obj = Instantiate(UI_icon_Prefab, transform); // 프리펩생성
        Skill_UI_Icon sc = obj.GetComponent<Skill_UI_Icon>(); // 아이콘 스크립트
        obj.transform.SetParent(activeSkill_Ui_Trs); // 트랜스폼 지정
        obj.transform.GetChild(0).GetComponent<Image>().sprite =  icon_Img[value]; // 내부 이미지 설정
        obj.GetComponent<Animator>().SetTrigger("Jump"); // 스르륵 연출

        switch (value)
        {
            case 0:
                sc.type = Skill_UI_Icon.SkillType.Active_0;
                break;

            case 1:
                sc.type = Skill_UI_Icon.SkillType.Active_1;
                break;

            case 2:
                sc.type = Skill_UI_Icon.SkillType.Active_2;
                break;

            case 3:
                
                break;

            case 4:
                
                break;

            case 5:
                
                break;
        }
        



    }
    public void F_Set_PassiveCheak(int value)
    {
        GameObject obj = Instantiate(UI_icon_Prefab, transform); // 프리펩생성
        Skill_UI_Icon sc = obj.GetComponent<Skill_UI_Icon>(); // 아이콘 스크립트
        obj.transform.SetParent(passiveSkill_Ui_Trs); // 트랜스폼 지정
        obj.transform.GetChild(0).GetComponent<Image>().sprite = Passive_icon_Img[value]; // 내부 이미지 설정
        obj.GetComponent<Animator>().SetTrigger("Jump"); // 스르륵 연출

        switch (value)
        {
            case 0:
                sc.type = Skill_UI_Icon.SkillType.Passive_0;
                break;

            case 1:
                sc.type = Skill_UI_Icon.SkillType.Passive_1;
                break;

            case 2:
                sc.type = Skill_UI_Icon.SkillType.Passive_2;
                break;

            case 3:
                sc.type = Skill_UI_Icon.SkillType.Passive_3;
                break;

            case 4:
                sc.type = Skill_UI_Icon.SkillType.Passive_4;
                break;

            case 5:

                break;
        }

    }
}

