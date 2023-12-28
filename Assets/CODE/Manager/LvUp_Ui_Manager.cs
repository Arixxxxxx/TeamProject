using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvUp_Ui_Manager : MonoBehaviour
{
    [Header("# Insert Window Obj in Hierarchy")]
    [Space]
    [SerializeField] GameObject LvupWindow;
    [Space]
    [SerializeField] GameObject SlotObj;


    Transform slot1, slot2, slot3;
    void Start()
    {
        slot1 = LvupWindow.transform.Find("Box/Slot_Box/Slot_1").GetComponent<Transform>();
        slot2 = LvupWindow.transform.Find("Box/Slot_Box/Slot_2").GetComponent<Transform>();
        slot3= LvupWindow.transform.Find("Box/Slot_Box/Slot_3").GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
