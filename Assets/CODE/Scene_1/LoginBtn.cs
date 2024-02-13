using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LoginBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        

        anim.SetTrigger("On");
        SoundManager.inst.F_Get_SoundPreFabs_PlaySFX(0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      
        anim.SetTrigger("Off");
    }

  
    void Start()
    {

    }

   
}
