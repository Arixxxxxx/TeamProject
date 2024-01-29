using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Movement : MonoBehaviour
{
    [Header("# PlayerObj Insert stats    =>    (예진) ")]
    [Space]
    [SerializeField] float CharMove_Speed;
    [SerializeField] float Sprint_Speed;


    Rigidbody2D rb;
    SpriteRenderer sr;
    Vector2 moveVec;
    Animator anim;
    bool InputSpaceBar;
    bool InputLshift;
    Image Sprint_Bar_Ui;
    GameObject Sprint_Bar;
    GameManager gm;


    [SerializeField] float TeleportDealy;
    WaitForSeconds TelePortDealys;
    [SerializeField] float[] TeleportLimitP_X;
    [SerializeField][Tooltip("PXY, MXY")] float[] bossPortalWayTelePortLimit;
    [SerializeField] float TeleportLimitM_X;
    [SerializeField] float TeleportLimitP_Y;
    [SerializeField] float TeleportLimitM_Y;
    [Header("BossRoom Limit Telleport")]
    [Space]
    [SerializeField][Tooltip("X,-X,Y,-Y")] float[] TeleportLimit;

    // 텔레포트 쿨타임 UI 변수
    GameObject telePortCoolTimeBar;
    Image teleFrontIMG;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        Sprint_Bar_Ui = transform.Find("Character_Mini_Ui/Sprint_Bar/Front").GetComponent<Image>();
        Sprint_Bar = transform.Find("Character_Mini_Ui/Sprint_Bar").gameObject;
        telePortCoolTimeBar = transform.Find("Character_Mini_Ui/TelePort_CoolTimeBar").gameObject;
        teleFrontIMG = telePortCoolTimeBar.transform.Find("Front").GetComponent<Image>();
    }
    void Start()
    {
        curSpintTime = maxSpintTime;
        Origin_maxSpintTime = maxSpintTime;
        TelePortDealys = new WaitForSeconds(TeleportDealy);
        OriginSpeed = CharMove_Speed;
        Origin_TeleportCoolTim = TeleportCoolTime;
        Origin_TeleportDistance = TeleportDistance;
        gm = GameManager.Inst;
    }
    private void FixedUpdate()
    {
        if(gm.MoveStop == true) { return; }
        Move_Character();
    }

    void Update()
    {
        

        Input_Key_Funtion();
        Change_Sclae_Xvalue();
        Animator_Updater();

        Sprint_Time_Updater();
        TelePortCoolTimeBar_Updater();
        FrontTeleport();
    }


    private void Input_Key_Funtion()
    {
        moveVec.x = Input.GetAxisRaw("Horizontal");
        moveVec.y = Input.GetAxisRaw("Vertical");
        InputLshift = Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift);
        InputSpaceBar = Input.GetKeyDown(KeyCode.Space);
    }

    private void Change_Sclae_Xvalue()
    {
        if (gm.MoveStop == true) { return; }

        if (moveVec.x < 0)
        {
            sr.flipX = true;
        }
        else if (moveVec.x > 0)
        {
            sr.flipX = false;
        }
    }

    float animMoveCeahkFloat;
    private void Animator_Updater()
    {
        if (gm.MoveStop == true) { anim.SetBool("Run", false); return; }

        float horizontalCheck = Mathf.Abs(moveVec.x);
        float verticalCheck = Mathf.Abs(moveVec.y);
        animMoveCeahkFloat = horizontalCheck + verticalCheck;

        if (animMoveCeahkFloat > 0)
        {
            anim.SetBool("Run", true);
        }
        else if (animMoveCeahkFloat >= 0)
        {
            anim.SetBool("Run", false);
        }
    }

    float MoveCheakX; // 스프린트 작동 시간 제어
    float MoveCheakY; // 스프린트 작동 시간 제어
    float CheakMoveFloat; // 스프린트 작동 시간 제어
    [SerializeField] float curSpintTime;
    [SerializeField] float maxSpintTime;
    float Origin_maxSpintTime;
    private void Sprint_Time_Updater()
    {
        MoveCheakX = Mathf.Abs(moveVec.x);
        MoveCheakY = Mathf.Abs(moveVec.y);

        CheakMoveFloat = MoveCheakX + MoveCheakY;

        if (InputLshift && CheakMoveFloat > 0) // 소모
        {

            if(Sprint_Bar.gameObject.activeSelf == false)
            {
                Sprint_Bar.gameObject.SetActive(true);
            }
           
            Sprint_Bar_Ui.fillAmount = curSpintTime / maxSpintTime;

            if (curSpintTime > 0)
            {
                curSpintTime -= Time.deltaTime;
            }
            else if (curSpintTime <= 0)
            {
                curSpintTime = 0;
            }
        }
        else // 회복
        {
            curSpintTime += Time.deltaTime;
            Sprint_Bar_Ui.fillAmount = curSpintTime / maxSpintTime;

            if (curSpintTime > maxSpintTime)
            {
                curSpintTime = maxSpintTime;

                if(Sprint_Bar.gameObject.activeSelf == true)
                {
                    Sprint_Bar.gameObject.SetActive(false);
                }
            }
        }
    }

    [SerializeField] bool isRun;
    private void Move_Character()
    {
        moveVec = moveVec.normalized;

        if (InputLshift == true && curSpintTime > 0)
        {
            isRun = true;
        }
        else if (InputLshift == false || curSpintTime <= 0)
        {
            isRun = false;
        }


        if (isRun)
        { 
            rb.MovePosition(rb.position + moveVec * (CharMove_Speed + Sprint_Speed) * Time.deltaTime);
        }
        else
        {
            rb.MovePosition(rb.position + moveVec * CharMove_Speed * Time.deltaTime);
        }
    }

    [SerializeField] float TeleportDistance;
    [SerializeField] float Origin_TeleportDistance;
    [SerializeField] float TeleportCoolTime;
    float Origin_TeleportCoolTim;
    [SerializeField] float teleportCount;
    bool doTeleport;

    private void FrontTeleport()
    {
        if (InputSpaceBar == true && doTeleport == false && CheakMoveFloat > 0)
        {
            StartCoroutine(Player_Input_Spacebar_TelePort());
        }

        if (doTeleport)
        {

            teleportCount += Time.deltaTime;

            if (teleportCount > TeleportCoolTime)
            {
                teleportCount = 0;
                doTeleport = false;

            }

        }
    }

    private void TelePortCoolTimeBar_Updater()
    {
        if (doTeleport)
        {
            if(telePortCoolTimeBar.activeSelf == false)
            {
                teleFrontIMG.fillAmount = 0;
                telePortCoolTimeBar.SetActive(true);
            }

            teleFrontIMG.fillAmount = teleportCount / TeleportCoolTime;

        }
        else
        {
            if(telePortCoolTimeBar.activeSelf == true)
            {
                telePortCoolTimeBar.SetActive(false);
            }
        }
    }

    float OriginSpeed;
    public void F_SetMoveSpeedAdd(float Value) // 이동속드증가 버프를 위한 함수
    {
        CharMove_Speed = OriginSpeed * Value;
    }

    public void F_SetSprintTimeAdd(float Value) // 이동 스태미너량 버프를 위한 함수
    {
        maxSpintTime = Origin_maxSpintTime * Value;
    }

    IEnumerator Player_Input_Spacebar_TelePort() // 순간이동 로직 코루틴
    {
        doTeleport = true;
        //anim.SetTrigger("Tel"); // 애니메이션
        GameManager.Inst.TelePort(0); // 파티클로 변경 24/01/19
        yield return TelePortDealys;
        Vector2 cheakPos = rb.position + moveVec * TeleportDistance;

        if(gm.EnterBossRoom == false)
        {
            if(SpawnManager.inst.StageLv <= 3)
            {
                if (cheakPos.x < TeleportLimitM_X)
                {
                    cheakPos.x = TeleportLimitM_X;
                }
                else if (cheakPos.x > TeleportLimitP_X[SpawnManager.inst.StageLv])
                {
                    cheakPos.x = TeleportLimitP_X[SpawnManager.inst.StageLv];
                }

                if (cheakPos.y < TeleportLimitM_Y)
                {
                    cheakPos.y = TeleportLimitM_Y;
                }
                else if (cheakPos.y > TeleportLimitP_Y)
                {
                    cheakPos.y = TeleportLimitP_Y;
                }
            }
            else if(SpawnManager.inst.StageLv == 4) // 통로
            {
                if (cheakPos.x > bossPortalWayTelePortLimit[0])
                {
                    cheakPos.x = bossPortalWayTelePortLimit[0];
                }
                else if (cheakPos.x < bossPortalWayTelePortLimit[2])
                {
                    cheakPos.x = bossPortalWayTelePortLimit[2];
                }

                if (cheakPos.y > bossPortalWayTelePortLimit[1])
                {
                    cheakPos.y = bossPortalWayTelePortLimit[1];
                }
                else if (cheakPos.y < bossPortalWayTelePortLimit[3])
                {
                    cheakPos.y = bossPortalWayTelePortLimit[3];
                }
            }
            
        }
        else // 보스방일시
        {
            if (cheakPos.x < TeleportLimit[1])
            {
                cheakPos.x = TeleportLimit[1];
            }
            else if (cheakPos.x > TeleportLimit[0])
            {
                cheakPos.x = TeleportLimit[0];
            }

            if (cheakPos.y < TeleportLimit[3])
            {
                cheakPos.y = TeleportLimit[3];
            }
            else if (cheakPos.y > TeleportLimit[2])
            {
                cheakPos.y = TeleportLimit[2];
            }
        }
        //순간이동 좌표값 제한 
      

        // 이동
        rb.position = cheakPos;
    }

    /// <summary>
    /// 텔레포트 쿨타임 감소
    /// </summary>
    /// <param name="value"></param>
    public void F_Set_TelePortDleayDown(float value)
    {
        TeleportCoolTime = Origin_TeleportCoolTim * value;
    }

    public void F_Set_Add_TelePortDistance(float value)
    {
        TeleportDistance = Origin_TeleportDistance * value;
    }

}
