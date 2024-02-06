using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager inst;

    Camera mainCam;
    [SerializeField] CinemachineVirtualCamera playerCam;
    public CinemachineVirtualCamera PlayerCam { get { return playerCam; } }

    [SerializeField] float pos_Minusx;
    [SerializeField] float pos_PlusX;
    [SerializeField] float pos_MinusY;
    [SerializeField] float pos_PlusY;
    [Header("Boss Room Camera Limit")]
    [Space]
    [SerializeField] float boss_Minusx;
    [SerializeField] float boss_PlusX;
    [SerializeField] float boss_MinusY;
    [SerializeField] float booss_PlusY;
    [Header("Camera Zoom value = 4 value")]
    [SerializeField] float[] camZoomValue;
    [SerializeField] float zoomOutSpeed;
    GameManager gm;
    Transform endingPos;
    int stageLv;
    
    private void Awake()
    {
        mainCam = Camera.main;
        if(inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }

        endingPos = transform.Find("Ending_CamPosition").GetComponent<Transform>();
    }
    void Start()
    {
        gm = GameManager.Inst;
        playerCam.Follow = GameManager.Inst.F_GetPalyerTargetPoint();
    }

    // Update is called once per frame
    void Update()
    {
        stageLv = SpawnManager.inst.StageLv;

    }

    private void LateUpdate()
    {
        playerCamsLimitPos(); 
    }

    //ī�޶� ������ ����
    private void playerCamsLimitPos()
    {
      if(gm.EnterBossRoom == false)
        {
            Vector3 campos = playerCam.transform.position;
            campos.x = Mathf.Clamp(campos.x, pos_Minusx, pos_PlusX);
            campos.y = Mathf.Clamp(campos.y, pos_MinusY, pos_PlusY);

            mainCam.transform.position = campos;
            playerCam.transform.position = campos;
        }
      else if(gm.EnterBossRoom == true) 
        {
            Vector3 campos = playerCam.transform.position;
            campos.x = Mathf.Clamp(campos.x, boss_Minusx, boss_PlusX);
            campos.y = Mathf.Clamp(campos.y, boss_MinusY, booss_PlusY);

            mainCam.transform.position = campos;
            playerCam.transform.position = campos;
        }
            
       
     
    }

    /// <summary>
    /// ī�޶� �ܾƿ��Լ�
    /// </summary>
    /// <param name="value"></param>
    public void F_CameraZoomOut(int value) // 
    {
        float sumValue = playerCam.m_Lens.OrthographicSize + camZoomValue[value];

       StartCoroutine(ZoomOut(sumValue));
    }

    public void F_CameraDirectZoomOut(float Value)
    {
        StartCoroutine(ZoomOut(Value));
    }

    IEnumerator ZoomOut(float sumValue)
    {
        while (playerCam.m_Lens.OrthographicSize < sumValue)
        {
            playerCam.m_Lens.OrthographicSize += Time.deltaTime * 2.5f;

            yield return null;
        }
    }

    public void F_CameraZoomIn(float value)
    {
        StartCoroutine(ZoomIn(value));
    }

    float zoomInLerpValue;
    float elapsedTime;
    float duration = 300f;
    IEnumerator ZoomIn(float sumValue)
    {
        while (playerCam.m_Lens.OrthographicSize> sumValue)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration); // 0���� 1 ���̷� ����ȭ

            // Lerp �Լ��� ����Ͽ� �ε巴�� ���� ����
            zoomInLerpValue = Mathf.Lerp(playerCam.m_Lens.OrthographicSize, sumValue - 0.1f, t);
            playerCam.m_Lens.OrthographicSize = zoomInLerpValue;
            //playerCam.m_Lens.OrthographicSize -= Time.deltaTime * 8;

            yield return null;
        }
    }


    /// <summary>
    /// �׼Ǿ� ������ ����� �Լ� 
    /// </summary>
    /// <param name="target"></param>
    /// <param name="Zoomvalue"></param>
    /// <param name="value"> true ����, false �ܾƿ�</param>
    /// <param name="camPositon"></param>
    public void F_OP_CamTargetSetting(Transform target, float Zoomvalue, bool value, Transform camPositon)
    {
        if(value == true)
        {
            StartCoroutine(MoveCam(target, Zoomvalue));
        }
        else
        {
            playerCam.Follow = null;  //ī�޶� ����
            playerCam.transform.position = camPositon.position; // �ȱ��
            playerCam.m_Lens.OrthographicSize = Zoomvalue;  // �ٺ�����
        }

    }

    [SerializeField] float dis;
    Vector3 targetPos;
    IEnumerator MoveCam(Transform target, float Zoomvalue)
    {
        dis = Vector2.Distance(target.transform.position, playerCam.transform.position);
        //dis = target.transform.position.x - playerCam.transform.position.x;

        targetPos = target.transform.position - playerCam.transform.position;
        targetPos.Normalize();

        while (dis > 0.3f)
        {
            dis = Vector2.Distance(target.transform.position, playerCam.transform.position);

            playerCam.transform.position += new Vector3(targetPos.x, targetPos.y, 0) * Time.deltaTime * 6f;

            if (playerCam.m_Lens.OrthographicSize >= Zoomvalue)
            {
                playerCam.m_Lens.OrthographicSize -= Time.deltaTime * 3.33f;
            }

            yield return null;

            //F_CameraZoomIn(Zoomvalue);
        }

        

        playerCam.Follow = GameManager.Inst.F_GetPalyerTargetPoint();
        playerCam.m_Lens.OrthographicSize = Zoomvalue;

        Opening_Manager.inst.F_Action2Start(); // �׼� 2 ���� (���� ����)

    }

    public void F_DirectAction()
    {
        playerCam.Follow = GameManager.Inst.F_GetPalyerTargetPoint();
        playerCam.m_Lens.OrthographicSize = 9.5f;
    }
     

    // ���� ���� �÷��̾� ���� �����Լ�
    public void F_EndingCamera() 
    {
        StartCoroutine(EndingCorutine());
    }

    IEnumerator EndingCorutine()
    {
        playerCam.Follow = null;
        playerCam.m_Lens.OrthographicSize = 10;
        yield return null;
        Vector3 movePos = endingPos.position;
        movePos.z = -10;
        playerCam.transform.position = movePos; // �ȱ��
    }
}
