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

        //호출 버튼
        popupBtn[0] = mainUITrs.transform.Find("AchievBtn").GetComponent<Button>();
        popupBtn[1] = mainUITrs.transform.Find("BattleStatsBtn").GetComponent<Button>();

        popupBtn[1].onClick.AddListener(() => { Init_Window(); });

        //종료버튼
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
        LvUp_Ui_Manager.Inst.F_PlayerRunSounStop(); // 혹시 뛰고잇다면 소리종료
        SoundManager.inst.F_Get_ControllSoundPreFabs_ETC_PlaySFX(3, 1); // 버튼 눌리는 소리
        GameManager.Inst.F_TimeSclaeController(true);
        // 누적 전투시간 계산
        int outputTime = GameManager.Inst.TotalBattleTime + UnitFrame_Updater.inst.F_GetBattleTime();

        int days = outputTime / (24 * 3600);
        outputTime %= (24 * 3600);
        int hours = outputTime / 3600;
        outputTime %= 3600;
        int minutes = outputTime / 60;
        int seconds = outputTime % 60;

        if (days > 0)
        {
            logText[0].text = $"{days}일 {hours}시간 {minutes}분 {seconds}초 ";
        }
        else if (hours > 0)
        {
            logText[0].text = $"{hours}시간 {minutes}분 {seconds}초 ";
        }
        else if (minutes > 0)
        {
            logText[0].text = $"{minutes}분 {seconds}초 ";
        }
        else if (seconds >= 0)
        {
            logText[0].text = $"{seconds}초 ";
        }

        // 보스 처치

        logText[1].text = $"{GameManager.Inst.BossKillCount} 회";
        logText[2].text = $"{GameManager.Inst.TotalkillCount} 킬";
        logText[3].text = $"{GameManager.Inst.TotalLvUpCount} 레벨";
        logText[4].text = $"{GameManager.Inst.TotalPlayerDead} 회";

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
