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

    bool[] activePass = new bool[5];
    bool[] passivePass = new bool[5];
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void F_Set_ActiveCheak(int value)
    {
        if (activePass[value] == false)
        {
            activePass[value] = true;
        }
        
    }
    public void F_Set_PassiveCheak(int value)
    {
        if (passivePass[value] == false) 
        {
            passivePass[value] = true;
        }
    }
}

