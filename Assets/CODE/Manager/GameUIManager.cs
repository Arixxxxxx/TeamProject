using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Inst;

    [SerializeField] GameObject UI;

    // InGame Info Bar
    [Header("# Info Window Close Time")]
    Animator infoAnim;
    TMP_Text infoText;
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
        infoAnim = UI.transform.Find("GameInfoMSG").GetComponent<Animator>();
        infoText = infoAnim.GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void F_GameInfoOpen(int value)
    {
        string alramText = string.Empty;

        switch (value)
        {
            case 1:
                alramText = "< �� > �� ������ �رݵǾ����ϴ�.";
                break;

            case 2:
                alramText = "< ���� > ������ �رݵǾ����ϴ�.";
                break;

            case 3:
                alramText = "< ��ο� �� > ���� ���ϴ� ��Ż�� �����Ǿ����ϴ�.";
                break;

            case -1:
                alramText = "������ ���۵Ǿ����ϴ�.";
                break;
        }

        infoText.text = alramText;

        if (infoAnim.gameObject.activeSelf == false) 
        {
            infoAnim.gameObject.SetActive(true);
        }

        Invoke("closeWindow", closeTime);
    }

    private void closeWindow()
    {
        infoAnim.SetTrigger("close");
    }
}
