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

        // �÷��̾� �Ͼ���� ����
        GameManager.Inst.PlayerHP_Recovery(); // �÷��̾� Ǯ�� �������
        GameUIManager.Inst.Respawning = false; // �Լ� �ѹ��� ������ ����
        GameManager.Inst.MoveStop = false; // �̵��Ұ� ����
        GameManager.Inst.IsPlayer_Dead = false;
        Ui_ref.gameObject.SetActive(false);
        GameManager.Inst.PlayerRespawn_Mujuk(); // 3�ʹ���


    }
}
