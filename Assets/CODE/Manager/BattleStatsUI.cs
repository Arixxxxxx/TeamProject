using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

public class BattleStatsUI : MonoBehaviour
{
    Transform mainUITrs;
    GameObject battleWindowObj;
    GameObject textGroup;

    Button[] popupBtn = new Button[2];
    Button exitBtn;
    TMP_Text[] logText = new TMP_Text[5];

    
    void Start()
    {
        mainUITrs = GameUIManager.Inst.F_GetMainCanvasTRS();
        battleWindowObj = mainUITrs.transform.Find("BattaleStatsWindow").gameObject;
        textGroup = battleWindowObj.transform.Find("Case/LogTextGroup").gameObject;
        logText = textGroup.GetComponentsInChildren<TMP_Text>();

        //ȣ�� ��ư
        popupBtn[0] = mainUITrs.transform.Find("AchievBtn").GetComponent<Button>();
        popupBtn[1] = mainUITrs.transform.Find("BattleStatsBtn").GetComponent<Button>();

        popupBtn[1].onClick.AddListener(() => { Init_Window(); });

        //�����ư
        exitBtn = battleWindowObj.transform.Find("Case/ExitBtn").GetComponent<Button>();
        exitBtn.onClick.AddListener(() => 
        {
            if (battleWindowObj.activeSelf)
            {
                SoundManager.inst.F_Get_ControllSoundPreFabs_ETC_PlaySFX(3, 1);
                battleWindowObj.SetActive(false); 
                BtnLock(true); 
                GameManager.Inst.F_TimeSclaeController(false); 
            } 
        });
    }

    private void Init_Window()
    {
        LvUp_Ui_Manager.Inst.F_PlayerRunSounStop(); // Ȥ�� �ٰ��մٸ� �Ҹ�����
        SoundManager.inst.F_Get_ControllSoundPreFabs_ETC_PlaySFX(3, 1); // ��ư ������ �Ҹ�
        GameManager.Inst.F_TimeSclaeController(true);
        // ���� �����ð� ���
        int outputTime = GameManager.Inst.TotalBattleTime + UnitFrame_Updater.inst.F_GetBattleTime();

        int days = outputTime / (24 * 3600);
        outputTime %= (24 * 3600);
        int hours = outputTime / 3600;
        outputTime %= 3600;
        int minutes = outputTime / 60;
        int seconds = outputTime % 60;

        if (days > 0)
        {
            logText[0].text = $"{days}�� {hours}�ð� {minutes}�� {seconds}�� ";
        }
        else if (hours > 0)
        {
            logText[0].text = $"{hours}�ð� {minutes}�� {seconds}�� ";
        }
        else if (minutes > 0)
        {
            logText[0].text = $"{minutes}�� {seconds}�� ";
        }
        else if (seconds >= 0)
        {
            logText[0].text = $"{seconds}�� ";
        }

        // ���� óġ

        logText[1].text = $"{GameManager.Inst.BossKillCount} ȸ";
        logText[2].text = $"{GameManager.Inst.TotalkillCount} ų";
        logText[3].text = $"{GameManager.Inst.TotalLvUpCount} ����";
        logText[4].text = $"{GameManager.Inst.TotalPlayerDead} ȸ";

        if(battleWindowObj.activeSelf == false)
        {
            BtnLock(false);
            battleWindowObj.SetActive(true);
        }

    }

    private void BtnLock(bool value)
    {
        for(int i = 0; i < popupBtn.Length; i++)
        {
            popupBtn[i].interactable = value;
        }
    }
}
