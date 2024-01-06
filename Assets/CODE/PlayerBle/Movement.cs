using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Movement : MonoBehaviour
{
    [Header("# PlayerObj Insert stats    =>    (����) ")]
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

    [SerializeField] float TeleportDealy;
    WaitForSeconds TelePortDealys;
    [SerializeField] float[] TeleportLimitP_X;
    [SerializeField] float TeleportLimitM_X;
    [SerializeField] float TeleportLimitP_Y;
    [SerializeField] float TeleportLimitM_Y;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        Sprint_Bar_Ui = transform.Find("Character_Mini_Ui/Sprint_Bar").GetComponent<Image>();
    }
    void Start()
    {
        curSpintTime = maxSpintTime;
        Origin_maxSpintTime = maxSpintTime;
        TelePortDealys = new WaitForSeconds(TeleportDealy);
        OriginSpeed = CharMove_Speed;
        Origin_TeleportCoolTim = TeleportCoolTime;
        Origin_TeleportDistance = TeleportDistance;
    }
    private void FixedUpdate()
    {
        Move_Character();
    }

    void Update()
    {
        

        Input_Key_Funtion();
        Change_Sclae_Xvalue();
        Animator_Updater();

        Sprint_Time_Updater();
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

    float MoveCheakX; // ������Ʈ �۵� �ð� ����
    float MoveCheakY; // ������Ʈ �۵� �ð� ����
    float CheakMoveFloat; // ������Ʈ �۵� �ð� ����
    [SerializeField] float curSpintTime;
    [SerializeField] float maxSpintTime;
    float Origin_maxSpintTime;
    private void Sprint_Time_Updater()
    {
        MoveCheakX = Mathf.Abs(moveVec.x);
        MoveCheakY = Mathf.Abs(moveVec.y);

        CheakMoveFloat = MoveCheakX + MoveCheakY;

        if (InputLshift && CheakMoveFloat > 0) // �Ҹ�
        {

            if(Sprint_Bar_Ui.gameObject.activeSelf == false)
            {
                Sprint_Bar_Ui.gameObject.SetActive(true);
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
        else // ȸ��
        {
            curSpintTime += Time.deltaTime;
            Sprint_Bar_Ui.fillAmount = curSpintTime / maxSpintTime;

            if (curSpintTime > maxSpintTime)
            {
                curSpintTime = maxSpintTime;

                if(Sprint_Bar_Ui.gameObject.activeSelf == true)
                {
                    Sprint_Bar_Ui.gameObject.SetActive(false);
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
    [SerializeField] float Count;
    bool doTeleport , once;

    private void FrontTeleport()
    {
        if (InputSpaceBar == true && doTeleport == false && CheakMoveFloat > 0)
        {
            StartCoroutine(Player_Input_Spacebar_TelePort());
        }

        if (doTeleport)
        {
            if (Count != TeleportCoolTime && once == false)
            {
                once = true;
                Count = TeleportCoolTime;
            }

            Count -= Time.deltaTime;

            if (Count <= 0)
            {
                doTeleport = false;
                once = false;

            }

        }
    }

    float OriginSpeed;
    public void F_SetMoveSpeedAdd(float Value) // �̵��ӵ����� ������ ���� �Լ�
    {
        CharMove_Speed = OriginSpeed * Value;
    }

    public void F_SetSprintTimeAdd(float Value) // �̵� ���¹̳ʷ� ������ ���� �Լ�
    {
        maxSpintTime = Origin_maxSpintTime * Value;
    }

    IEnumerator Player_Input_Spacebar_TelePort() // �����̵� ���� �ڷ�ƾ
    {
        doTeleport = true;
        anim.SetTrigger("Tel");

        yield return TelePortDealys;
        Vector2 cheakPos = rb.position + moveVec * TeleportDistance;

        //�����̵� ��ǥ�� ���� 
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

        // �̵�
        rb.position = cheakPos;
    }

    /// <summary>
    /// �ڷ���Ʈ ��Ÿ�� ����
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
