using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hub : MonoBehaviour
{
    public static Hub Inst;

    Player_Skill_System skill;
    public Player_Skill_System player_skill_system_sc { get {  return skill; } }

    [SerializeField] LvUp_Ui_Manager ui;
    public LvUp_Ui_Manager LvUpUI_Manager { get { return ui; } }

    [SerializeField] GameObject[] Player;
    

    Player_Stats status;
    public Player_Stats Player_Status_sc { get { return status; } }

    Movement move;
    public Movement Movement_sc { get { return move; } }


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

        if (Player[0].gameObject.activeSelf == true)  // ¿©Ä³
        {
            status = Player[0].GetComponent<Player_Stats>();
            skill = Player[0].GetComponent<Player_Skill_System>();
            move = Player[0].GetComponent<Movement>();
        }

    }
   

}
