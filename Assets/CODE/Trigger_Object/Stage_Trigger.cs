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
            
            if(number == DoorNumber.Boss)
            {
                CameraManager.inst.F_CameraZoomIn(8);
                SpawnManager.inst.F_DeleteCloud((int)number);
               
            }
            else if(number != DoorNumber.Boss)
            {
                SpawnManager.inst.F_DeleteCloud((int)number);
                SpawnManager.inst.F_StageLvUp();
            }
        }
    }

 
}
