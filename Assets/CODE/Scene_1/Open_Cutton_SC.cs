using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Open_Cutton_SC : MonoBehaviour
{
    Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    public void A_OpeningCanvansOff()
    {
        if (anim.gameObject.activeSelf == true)
        {
            anim.gameObject.SetActive(false);
            OpeningManager.inst.F_ActionEnd(0, true);
        }
    }

    public void A_PressAnykey_Obj_Active_True()
    {
        OpeningManager.inst.F_PressObjActiveTrue();
        Debug.Log(" 애니함수 시적");
    }
    

    public void A_NextScene()
    {
        SceneManager.LoadScene(1);
    }
}
