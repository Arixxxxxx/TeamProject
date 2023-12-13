using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Skill_System : MonoBehaviour
{
    [Header("# Insert Skill Object in Hierarchy")]
    [Space]
    [SerializeField] GameObject[] skill_Obj;
    [SerializeField] bool Active_Skill_01;
    [SerializeField] bool Active_Skill_02;
    [Header("# Input Skill Spec  ==>  # ¿¹Áø ")]
    [Space]
    [SerializeField] float Skill_one_distance;
    [SerializeField] float Skill_one_spinSpeed;
        
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Active_Skill_Cheaker();
        Play_Skill_01();
    }

    private void Active_Skill_Cheaker()
    {
        if(Active_Skill_01 == true && skill_Obj[0].gameObject.activeSelf == false)
        {
            skill_Obj[0].gameObject.SetActive(true);
        }
    }

    private void Play_Skill_01()
    {
        if(skill_Obj[0].gameObject.activeSelf == true)
        {
            skill_Obj[0].transform.eulerAngles += Vector3.forward * Skill_one_spinSpeed * Time.deltaTime;
        }
    }
}
