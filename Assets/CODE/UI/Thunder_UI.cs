using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder_UI : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();  
    }


    [SerializeField] bool Action;
    [SerializeField] float count;
    [SerializeField] float RandomTimevalue;
    
    
    void Update()
    {
        cheakStageLv();
        Thunder();
    }

    private void cheakStageLv()
    {
        if(SpawnManager.inst.StageLv == 2)
        {
            Action = true;
        }
        else 
        {
            Action = false;
        }
    }

    bool once;
    private void Thunder()
    {
        if (Action)
        {
            if (!once)
            {
                once = true;
                RandomTimevalue = 0;
                RandomTimevalue = Random.Range(5f, 10f);
            }

            count += Time.deltaTime;
            if (count > RandomTimevalue) 
            {
                count = 0;
                anim.SetTrigger("Go");
                SoundManager.inst.F_Get_ControllSoundPreFabs_ETC_PlaySFX(12, 0.8f);
                once = false;
            }
        }
    }
}
