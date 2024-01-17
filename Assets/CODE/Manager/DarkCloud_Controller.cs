using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkCloud_Controller : MonoBehaviour
{
    [Header("# Insert Inspecter")]
    [SerializeField] Rigidbody2D darkCloud;
    [SerializeField] float cloudSpeed;
    [SerializeField] Transform startPoint, stopPoint0, stopPoint1;

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
    void Update()
    {
        
    }
    [SerializeField] bool Patten0, Patten1;

    [SerializeField] float moveX;
    [SerializeField] float checkDistance0;
    [SerializeField] float checkDistance1;
    private void DarkCloudeMove()
    {
        checkDistance0 = darkCloud.position.x - stopPoint0.position.x;
        checkDistance1 = darkCloud.position.x - stopPoint1.position.x;

        if (Patten0 && checkDistance0 < -1)
        {
            
            darkCloud.MovePosition(darkCloud.position + Vector2.right * (cloudSpeed * Time.deltaTime));
        }
        else if(Patten1 && checkDistance1 < -1)
        {
            darkCloud.MovePosition(darkCloud.position + Vector2.right * (cloudSpeed * Time.deltaTime));
        }

    }
}
