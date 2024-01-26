using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Text_Tyiping : MonoBehaviour
{
    TMP_Text mainText;
    [SerializeField] float Speed;
    [SerializeField] int index;
    [SerializeField] Color textColor;
    private void Awake()
    {
        mainText = GetComponent<TMP_Text>();
    }

    string GetText;
    
   
    public void F_Set_TalkBox_Main_Text(string value)
    {

        GetText = value;
        mainText.color = textColor;
        mainText.text = string.Empty;
        Insert_Word();
    }


    private void Insert_Word()
    {
        if (mainText.text == GetText)
        {
            End_Text();
            return;
        }

        mainText.text += GetText[index];
        index++;

        Invoke("Insert_Word", 1 / Speed);
    }

    private void End_Text()
    {
        index = 0;
        Opening_Manager.inst.nextOk = false;
    }

    public void F_TextEmpty()
    {
        mainText.text = string.Empty;
    }

    public void F_HideText(float FadeValue)
    {
        StartCoroutine(HIdeText(FadeValue));
    }

    IEnumerator HIdeText(float FadeValue)
    {
        while(mainText.color.a > 0)
        {
            mainText.color -= new Color(0, 0, 0, 0.2f) * Time.deltaTime * FadeValue;
            yield return null;

            if(mainText.color.a == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
