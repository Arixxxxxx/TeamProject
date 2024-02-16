using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stage_Trigger : MonoBehaviour
{
    public enum DoorNumber { First, Sec, Boss }
    public DoorNumber number;

    BoxCollider2D boxColl;


    private void Awake()
    {
        boxColl = GetComponent<BoxCollider2D>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool once;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && boxColl.isTrigger == true && !once)
        {
            once = true;
            SpawnManager.inst.F_StageLvUp();
            GameUIManager.Inst.F_NextMapArrowActiveSec(false);
            SpawnManager.inst.F_DeleteCloud((int)number); // 구름제거
            NextStageAcive();


            if (number == DoorNumber.Boss) // 보스방전용
            {
                CameraManager.inst.F_CameraZoomIn(10);
                
            }
            else if(number == DoorNumber.Sec) // 스테이지 1~2구간
            {
                
                CameraManager.inst.F_CameraDirectZoomOut(12.5f);


            }

            else if(number == DoorNumber.First)// 스테이지 1~2구간
            {
                
                CameraManager.inst.F_CameraDirectZoomOut(11.8f);
            }
        }
    }

    private void NextStageAcive()
    {
        DarkCloud_Controller.inst.F_darkCloudeSpeedUp(1, (int)number, 2.4f);
        GlobalLightController.Inst.F_LightControl((int)number);

        switch(number)
        {
            case DoorNumber.First:

                SoundManager.inst.F_Bgm_Player(2, 2f, 0.9f);

                break;

            case DoorNumber.Sec:

                SoundManager.inst.F_Bgm_Player(3, 2f, 0.9f);
                break;

            case DoorNumber.Boss:

                SoundManager.inst.F_Bgm_Player(4, 2f, 1);
                break;

        }
    }

 
}
