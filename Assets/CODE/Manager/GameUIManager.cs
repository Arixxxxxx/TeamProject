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
                alramText = "< 숲 > 의 지형이 해금되었습니다.";
                break;

            case 2:
                alramText = "< 성당 > 지형이 해금되었습니다.";
                break;

            case 3:
                alramText = "< 어두운 숲 > 으로 향하는 포탈이 생성되었습니다.";
                break;

            case -1:
                alramText = "게임이 시작되었습니다.";
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
