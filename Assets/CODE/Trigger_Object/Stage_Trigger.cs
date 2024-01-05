using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_Trigger : MonoBehaviour
{
    public enum DoorNumber { First, Sec }
    public DoorNumber number;

    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            SpawnManager.inst.F_DeleteCloud((int)number);
            //if (number == DoorNumber.First)
            //{

            //}
            //else if(number == DoorNumber.Sec)
            //{

            //}

        }
    }
}
