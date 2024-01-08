using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dmg_Font : MonoBehaviour
{
    [SerializeField] float ColorA_FadeOup_Speed;
    [SerializeField] float CriticalFont;
    float OriginFontSize;
    TMP_Text Dmg_Text;
    [SerializeField] Color normalDMG_Color;
    [SerializeField] Color CriDMG_Color;

    private void Start()
    {
        Dmg_Text = GetComponentInChildren<TMP_Text>();
        OriginFontSize = Dmg_Text.fontSize;
    }

    private void Update()
    {
        if (FadeOut == true)
        {
            Dmg_Text.color -= new Color(0, 0, 0, 0.1f) * Time.deltaTime * ColorA_FadeOup_Speed;

            if(Dmg_Text.color.a <= 0.1f)
            {
                 PoolManager.Inst.F_ReturnObj(gameObject,2);
            }
        }
    }

    public void F_text_Init(float Dmg, bool Cri)
    {
        if(Dmg_Text == null)
        {
            Dmg_Text = GetComponentInChildren<TMP_Text>();
        }
        
        Dmg_Text.fontSize = 65;
        Dmg_Text.text = string.Empty;

        FadeOut = false;
        Dmg_Text.color = Color.white;
        
        if (Cri == true)
        {
            Dmg_Text.color = CriDMG_Color;
            Dmg_Text.fontSize += CriticalFont;
            Dmg_Text.text = $"Cri! {Dmg.ToString("F0")}"; // 소수점 첫번재자리까지만
        }
        else
        {
            Dmg_Text.color = normalDMG_Color;
            Dmg_Text.text = Dmg.ToString("F0");
        }
      
    }

    public void F_text_Init(string text)
    {
        if (Dmg_Text == null)
        {
            Dmg_Text = GetComponentInChildren<TMP_Text>();
        }

        Dmg_Text.fontSize = 65;
        Dmg_Text.text = string.Empty;

        FadeOut = false;
        Dmg_Text.color = Color.white;

        Dmg_Text.text = text;

    }


    [SerializeField] bool FadeOut;
    private void A_Font_Fadeout()
    {
        FadeOut = true;
    }

}
