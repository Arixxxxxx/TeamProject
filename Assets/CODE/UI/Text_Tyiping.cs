using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Text_Tyiping : MonoBehaviour
{
    
    TMP_Text mainText;
    [SerializeField] float Speed;
    [SerializeField] float addSpeed;
    [SerializeField] int index;
    [SerializeField] Color textColor;

    int sceneNum;
    private void Awake()
    {
        mainText = GetComponent<TMP_Text>();
        sceneNum = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(sceneNum);
    }

    string GetText;
    
   
    public void F_Set_TalkBox_Main_Text(string value)
    {

        GetText = value;
        mainText.color = textColor;
        mainText.text = string.Empty;
        Insert_Word();
    }

    float sumSpeed;
    private void Insert_Word()
    {
        if (mainText.text == GetText)
        {
            End_Text();
            return;
        }

        mainText.text += GetText[index];
        index++;
        
        sumSpeed = Speed + addSpeed;

        Invoke("Insert_Word", 1 / sumSpeed);
    }

    private void End_Text()
    {
        index = 0;


        if(sceneNum == 1)
        {
            Opening_Manager.inst.nextOk = false;
        }
        else if(sceneNum == 3)
        {
            EndingDirector.inst.NextOk = false;
           
        }
        
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

    public void F_SetAddSpeed(bool value)
    {
        if (value)
        {
            addSpeed = 40;
        }
        else
        {
            addSpeed = 0;
        }
    }
}
