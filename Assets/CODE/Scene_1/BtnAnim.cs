using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        anim.SetTrigger("On");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        anim.SetTrigger("Off");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
