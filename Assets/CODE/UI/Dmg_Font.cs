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
        }
    }

    public void F_text_Init(float Dmg, bool Cri)
    {
        if(Dmg_Text == null)
        {
            Dmg_Text = GetComponentInChildren<TMP_Text>();
        }

        Dmg_Text.text = string.Empty;

        FadeOut = false;
        Dmg_Text.color = Color.white;
        
        if (Cri == true)
        {
            Dmg_Text.color = CriDMG_Color;
            Dmg_Text.fontSize += CriticalFont;
            Dmg_Text.text = $"Cri! {Dmg.ToString()}";
        }
        else
        {
            Dmg_Text.color = normalDMG_Color;
            Dmg_Text.text = Dmg.ToString();
        }
      
    }


    [SerializeField] bool FadeOut;
    private void A_Font_Fadeout()
    {
        FadeOut = true;
    }

}
