using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkCloud_Controller : MonoBehaviour
{
    [Header("# Insert Inspecter")]
    [SerializeField] Rigidbody2D darkCloud;
    [SerializeField] float cloudSpeed;
    [SerializeField] Transform startPoint, stopPoint0, stopPoint1, stopPoint2;
    [SerializeField] bool cloudMoveStart;
    [SerializeField] float dealyTime;
    GameManager gm;
    SpawnManager sm;



    void Start()
    {
        gm = GameManager.Inst;
        sm = SpawnManager.inst;
    }

    private void FixedUpdate()
    {

        DarkCloudeMove();
    }
    float count;
    void Update()
    {
        Set_SpeedValue();
      
        if (cloudMoveStart == false && gm.MainGameStart == true)
        {
            count += Time.deltaTime;
            if(count > dealyTime)
            {
                cloudMoveStart = true;
                count = 0;
            }
        }

    }
    float stageTime;
    [SerializeField] int stageLv;

    // ��ұ��� �ӵ� ��� �Լ�
    private void Set_SpeedValue()
    {
        //if (gm.MainGameStart == false) { return; }

        stageTime = sm.F_Get_StageTime();

        float curruntX = darkCloud.transform.position.x; //������ ������
        float curruentTime = Time.time; // ����������ð�

        float targetPosX = 0;

        switch (stageLv)
        {
            case 0:
                targetPosX = stopPoint0.position.x;
                break;

            case 1:
                targetPosX = stopPoint1.position.x;
                break;
            
            case 3: // ������
                targetPosX = stopPoint2.position.x;
                break;
        }

        float dis = targetPosX - curruntX; // ��ǥ���� - ���� ����
        float movetime = stageTime - curruentTime; // ���������ð� -��������ð�

        cloudSpeed = dis / movetime; // ����
    }


    [SerializeField] bool Patten0, Patten1;
    [SerializeField] float moveX;
    [SerializeField] float checkDistance0;
    [SerializeField] float checkDistance1;
    [SerializeField] float checkDistance2;

    //��ұ��� ������
    private void DarkCloudeMove()
    {
        if (gm.MainGameStart == false) { return; }

        stageLv = sm.StageLv;
        checkDistance0 = darkCloud.position.x - stopPoint0.position.x;
        checkDistance1 = darkCloud.position.x - stopPoint1.position.x;
        checkDistance2 = darkCloud.position.x - stopPoint2.position.x;

        if (stageLv == 0 && checkDistance0 < -1 && cloudMoveStart == true)
        {
            darkCloud.MovePosition(darkCloud.position + Vector2.right * (cloudSpeed * Time.deltaTime));
        }
        else if (stageLv == 1 && checkDistance1 < -1 && cloudMoveStart == true)
        {
            darkCloud.MovePosition(darkCloud.position + Vector2.right * (cloudSpeed * Time.deltaTime));
        }
        else if (stageLv == 3 && checkDistance2 < -1 && cloudMoveStart == true)
        {
            darkCloud.MovePosition(darkCloud.position + Vector2.right * (2f * Time.deltaTime));
        }

    }
}
