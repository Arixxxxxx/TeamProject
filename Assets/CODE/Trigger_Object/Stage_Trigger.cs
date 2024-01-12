using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_Trigger : MonoBehaviour
{
    public enum DoorNumber { First, Sec }
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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && boxColl.isTrigger == true)
        {
            SpawnManager.inst.F_DeleteCloud((int)number);
        }
    }
}
