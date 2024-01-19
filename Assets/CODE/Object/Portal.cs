using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    bool entry;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && entry == false)
        {
            entry = true;
            GameUIManager.Inst.F_OpenSelectWindow(0);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && entry == true)
        {
            Invoke("entryFalse", 0.5f);
        }
    }

    private void entryFalse()
    {
        entry = false;
    }
}
