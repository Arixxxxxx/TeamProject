using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Magnet : MonoBehaviour
{

    BoxCollider2D boxColl;
    Exp_Coin[] exp;
    private void Awake()
    {
        boxColl = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        if(boxColl.enabled == false) { boxColl.enabled = true; }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ManetTrue()
    {
        exp = FindObjectsOfType<Exp_Coin>();

        if(exp.Length == 0) 
        {
             gameObject.SetActive(false);
        }
        else if(exp.Length >= 1) 
        {
            for(int i  = 0; i < exp.Length; i++)
            {
                exp[i].F_magnetActive();
            }

            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (boxColl.enabled == true && collision.CompareTag("Player"))
        {
            ManetTrue();
        }


     

    }
}
