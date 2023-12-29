using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Ui_Skill_Select_Btn : MonoBehaviour
{
    public enum SkillType
    {
        Attack0, Attack1, Attack2, Attack3, Attack4,
        Passive0, Passive1, Passive2, Passive3, Passive4
    }
    public SkillType skilltype;

    [Header(" # Insert Prefab In Inspecter")]
    [Space]
    [Header(" # Star Spirte")]
    [SerializeField][Tooltip("별 모양 이미지")] Sprite[] star_Sprite;
    [Header(" #  Input_Field => Skill Text")]
    [Space]
    [SerializeField] CardStringValue[] SkillNameAndText;

    TMP_Text skill_Name;
    Image skill_Img;
    TMP_Text skill_Text;
    Transform diamondGorupTrs;
    Image[] start_IMG;


    void Start()
    {
        InIt();
    }
    int a;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            F_Set_SelectCard(a);
            a++;
        }
    }

    private void InIt()
    {
        skill_Name = transform.Find("Skill_Name").GetComponent<TMP_Text>();
        skill_Img = transform.Find("Skill_Img").GetComponent<Image>();
        skill_Text = transform.Find("Skill_Text").GetComponent<TMP_Text>();
        diamondGorupTrs = transform.Find("Dia_Group").GetComponent<Transform>();
        start_IMG = diamondGorupTrs.GetComponentsInChildren<Image>();
    }


    public void F_Set_SelectCard(int Star_Count)
    {
        if (skill_Name == null) { InIt(); }

        skill_Name.text = SkillNameAndText[(int)skilltype].Skill_Name;
        skill_Text.text = SkillNameAndText[(int)skilltype].Skill_Text;
        skill_Img.sprite = SkillNameAndText[(int)skilltype].Img;

        Star_Count++;

        for (int i = 0; i < 5; i++)
        {
            if (i < Star_Count)
            {
                start_IMG[i].sprite = star_Sprite[0];
            }
            else
            {
                start_IMG[i].sprite = star_Sprite[1];
            }
        }
    }
}

[System.Serializable]
public class CardStringValue
{
    public string Skill_Name;

    [Multiline]
    public string Skill_Text;
    public Sprite Img;
}
