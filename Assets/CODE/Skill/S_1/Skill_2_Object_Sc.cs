using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_2_Object_Sc : MonoBehaviour
{
    [SerializeField] float count, timer;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        Enable_Obj();

    }

    private void Enable_Obj()
    {
        count += Time.deltaTime;
        if (count > timer)
        {
            count = 0;
            gameObject.SetActive(false);
        }
    }
}
