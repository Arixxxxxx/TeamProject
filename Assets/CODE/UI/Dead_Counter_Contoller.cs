using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dead_Counter_Contoller : MonoBehaviour
{
    [Header("#Insert UI Ref")]
    [Space]
    [SerializeField] GameObject Ui_ref;
    [Space]
    [Header("#Insert Number Sprite")]
    [Space]
    [SerializeField] Sprite[] count_IMG;
    Image countIMG;
    float waitOneSec = 1;
    WaitForSeconds oneSec;

    

    private void Awake()
    {
        countIMG = Ui_ref.transform.Find("Count_IMG").GetComponent<Image>();

        oneSec= new WaitForSeconds(waitOneSec);
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            F_ActiveDeadCounter();
        }
    }

    public void F_ActiveDeadCounter()
    {
        GameManager.Inst.MoveStop = true;
        countIMG.sprite = count_IMG[0];
        Ui_ref.gameObject.SetActive(true);

        StartCoroutine(Action());

    }

    
    
    IEnumerator Action()
    {
        yield return oneSec;
        countIMG.sprite = count_IMG[1];
        yield return oneSec;
        countIMG.sprite = count_IMG[2];
        yield return oneSec;

        // 플레이어 일어나세요 로직
        GameManager.Inst.PlayerHP_Recovery(); // 플레이어 풀피 만들어줌
        GameUIManager.Inst.Respawning = false; // 함수 한번만 들어오게 제어
        GameManager.Inst.MoveStop = false; // 이동불가 해제
        GameManager.Inst.IsPlayer_Dead = false;
        Ui_ref.gameObject.SetActive(false);
        GameManager.Inst.PlayerRespawn_Mujuk(); // 3초무적


    }
}
