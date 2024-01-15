using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Inst;

    [SerializeField] GameObject UI;

    // InGame Info Bar
    [Header("# Info Window Close Time")]
    GameObject InfoObj;
    Animator infoAnim;
    Image infoBg;
    TMP_Text infoText;
    Color aZeroColor = new Color(1, 1, 1, 0);
    Color aColorUP = new Color(0, 0, 0, 0.3f);
    WaitForSeconds infoBarColse;
    [SerializeField] float closeTime;
    private void Awake()
    {
        if(Inst == null)
        {
            Inst = this;
        }
        else
        {
            Destroy(this);
        }
        
    }
    void Start()
    {
        InfoObj = UI.transform.Find("GameInfoMSG").gameObject;
        infoText = InfoObj.GetComponentInChildren<TMP_Text>();
        infoAnim = infoText.GetComponent<Animator>();
        infoBg = InfoObj.transform.Find("Bg").GetComponent<Image>();
        infoBarColse = new WaitForSeconds(closeTime);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    ///  ���Ӿȳ� ���� ��ȯ
    /// </summary>
    /// <param name="value"></param>
    public void F_GameInfoOpen(int value)
    {
        string alramText = string.Empty;
        
        switch (value)
        {
            case 1:
                alramText = "< ���� > ���� ���� ���� ���Ƚ��ϴ�.\n �� �������� �̵��ϼ���!";
                break;

            case 2:
                alramText = "< ���� > ������ �رݵǾ����ϴ�. \n �� �������� �̵��ϼ���!";
                break;

            case 3:
                alramText = "< ��ο� �� > ���� ���ϴ� ��Ż�� �����Ǿ����ϴ�.";
                break;

            case -1:
                alramText = "������ ���۵Ǿ����ϴ�.";
                break;
        }
        
        infoText.text = alramText;

       

        if (InfoObj.gameObject.activeSelf == false) 
        {
            InfoObj.gameObject.SetActive(true);
        }

        StopCoroutine(MsgInfoBar_Action());
        StartCoroutine(MsgInfoBar_Action());
    }

    
    IEnumerator MsgInfoBar_Action()
    {
        
        while(infoText.color.a < 0.95f)
        {
            infoBg.color += aColorUP * Time.deltaTime * 5;
            infoText.color += aColorUP * Time.deltaTime * 5;
            Debug.Log("1");
            yield return null;
        }

        infoAnim.enabled = true;
        yield return infoBarColse;
        infoAnim.enabled = false;

        while (infoText.color.a > 0.05f)
        {
            infoBg.color -= aColorUP * Time.deltaTime * 5;
            infoText.color -= aColorUP * Time.deltaTime * 5;
            Debug.Log("2");
            yield return null;
        }

        if (InfoObj.gameObject.activeSelf == true)
        {
            InfoObj.gameObject.SetActive(false);
        }

    }
  
}
