using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkCloud_Controller : MonoBehaviour
{
    public static DarkCloud_Controller inst;

    [Header("# Insert Inspecter")]
    [Space]
    [SerializeField] Rigidbody2D darkCloud;
    [SerializeField] float cloudSpeed;
    [SerializeField] Transform startPoint, stopPoint0, stopPoint1, stopPoint2;
    [SerializeField] bool cloudMoveStart;
    [SerializeField] float dealyTime;
    [SerializeField] int stageLv;
    [Header("# Check Distance Value")]
    [Space]
    [SerializeField] float checkDistance0;
    [SerializeField] float checkDistance1;
    [SerializeField] float checkDistance2;
    float stageTime;
    float[] timeOutSpeed = new float[3];
    float[] addSpeed = new float[3];

    GameManager gm;
    SpawnManager sm;

    private void Awake()
    {
        if(inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        gm = GameManager.Inst;
        sm = SpawnManager.inst;
        darkCloud.transform.position = startPoint.position;
    }

    private void FixedUpdate()
    {

        DarkCloudeMove();
    }
    float count;
    void Update()
    {
        
      
        if (cloudMoveStart == false && gm.MainGameStart == true)
        {
            count += Time.deltaTime;

            if(count > dealyTime)
            {

                cloudMoveStart = true;
                Pattern0 = true;
                count = 0;
            }
        }

    }
    

    // 어둠구름 속도 계산 함수
    private void Set_SpeedValue()
    {
        if (gm.MainGameStart == false) { return; }

        stageTime = sm.F_Get_StageTime();

        float curruntX = darkCloud.transform.position.x; //구름의 포지션
        float curruentTime = Time.time; // 게임의진행시간

        float targetPosX = 0;

        switch (stageLv)
        {
            case 0:
                targetPosX = stopPoint0.position.x;
                break;

            case 1:
                targetPosX = stopPoint1.position.x;
                break;
            
            case 3: // 보스방
                targetPosX = stopPoint2.position.x;
                break;
        }

        float dis = targetPosX - curruntX; // 목표지점 - 현재 지점
        float movetime = stageTime - curruentTime; // 스테이지시간 -게임진행시간

        cloudSpeed = dis / movetime; // 나눔
    }



    /// <summary>
    /// int type, int aeraNumber, float value
    /// </summary>
    /// <param name="type"> 0 = timeOutSpeed / 1 = addSpeed</param>
    /// <param name="aeraNumber"> AreaNumber </param>
    /// <param name="value"> AddSpeed value</param>
    public void F_darkCloudeSpeedUp(int type, int aeraNumber, float value)
    {
        if(type == 0)
        {
            timeOutSpeed[aeraNumber] = value;
        }
        else if(type == 1)
        {
            addSpeed[aeraNumber] = value;
        }
        
    }



    [SerializeField]  bool Pattern0, Pattern1, Pattern2;
    //어둠구름 움직임
    private void DarkCloudeMove()
    {
        if (gm.MainGameStart == false) { return; }

        stageLv = sm.StageLv;
        checkDistance0 = darkCloud.position.x - stopPoint0.position.x;
        checkDistance1 = darkCloud.position.x - stopPoint1.position.x;
        checkDistance2 = darkCloud.position.x - stopPoint2.position.x;
         // TimeOutSpeed = 시간이 다되엇을때 캐릭터를 우측으로 밀어내는용도
         // addSpeed = 다음 스테이지로 넘어갔을떄 이전스테이지로 못넘어가게 빨리 덮는용도
        float sumSpeed0 = cloudSpeed + timeOutSpeed[0] + addSpeed[0];
        float sumSpeed1 = cloudSpeed + timeOutSpeed[1] + addSpeed[1];
        float sumSpeed2 = cloudSpeed + timeOutSpeed[2] + addSpeed[2];
                                        //0.6 + 1.1 +  2.4

        if(Pattern0 == true) // 1스테이지 어둠구름이동
        {
            if (checkDistance0 < -1 && cloudMoveStart == true)
            {
                darkCloud.MovePosition(darkCloud.position + Vector2.right * (sumSpeed0 * Time.deltaTime));
            }
            else if (checkDistance0 > -1 && cloudMoveStart == true)
            {
                Pattern1 = true;
                Pattern0 = false;

                //Invoke("Pattern1Active", dealyTime);
                
            }
        }
        
        if(Pattern1 == true)  // 2스테이지 어둠구름 이동
        {
            if (checkDistance1 < -1 && cloudMoveStart == true)
            {
                darkCloud.MovePosition(darkCloud.position + Vector2.right * (sumSpeed1 * Time.deltaTime));
            }
            else if (checkDistance1 > -1 && cloudMoveStart == true)
            {
                Pattern1 = false;
            }
        }
    
        // 4분 끝 이동시작
        if(Pattern2 == true)
        {
            if (checkDistance2 < -1 && cloudMoveStart == true)
            {
                darkCloud.MovePosition(darkCloud.position + Vector2.right * (sumSpeed2 * Time.deltaTime));
            }
            else if (stageLv == 3 && checkDistance2 > -1 && cloudMoveStart == true)
            {
                cloudMoveStart = false; // 딜레이 시간
            }
        }
    }

    public void F_Pattern2Active(bool value)
    {
        Pattern2 = value;
    }

}
