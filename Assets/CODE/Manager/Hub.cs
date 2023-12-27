using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hub : MonoBehaviour
{
    public static Hub Inst;

    [SerializeField] Player_Skill_System skill;
    public Player_Skill_System player_skill_system_sc { get {  return skill; } }


    private void Awake()
    {
        if(Inst == null)
        {
            Inst = this;
        }
        else
        {
            Destroy(this);
        }

    }
    void Start()
    {
        
    }

}
