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
        
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void F_Set_ActiveCheak(int value)
    {
        GameObject obj = Instantiate(UI_icon_Prefab, transform);
        Skill_UI_Icon sc = obj.GetComponent<Skill_UI_Icon>();

        switch (value)
        {
            case 0:

                for(int i = 0; i < activeSkill_Ui_Trs.transform.childCount; i++) // 예외처리
                {
                    if(activeSkill_Ui_Trs.transform.GetChild(i) != null && sc.type == Skill_UI_Icon.SkillType.Active_0)
                    {
                        Destroy(obj);
                        Destroy(sc);
                        return;
                    }
                    else
                    {
                        obj.transform.SetParent(activeSkill_Ui_Trs);
                        sc.type = Skill_UI_Icon.SkillType.Active_0;
                    }
                }

                if (activeSkill_Ui_Trs.transform.childCount == 0) // 만약 최초실행이라면
                {
                    obj.transform.SetParent(activeSkill_Ui_Trs);
                    obj.transform.GetChild(0).GetComponent<Image>().sprite = icon_Img[0];
                    obj.GetComponent<Animator>().SetTrigger("Jump");
                    sc.type = Skill_UI_Icon.SkillType.Active_0;
                    
                }
                break;

            case 1:
                
                break;

            case 2:
                
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
   
    }
}

