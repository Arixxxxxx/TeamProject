using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Mode : MonoBehaviour
{
    [SerializeField] GameObject UI;
    [SerializeField] GameObject OpeningOBJ;
    void Start()
    {
        if(OpeningOBJ.activeSelf == false)
        {
            UI.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
