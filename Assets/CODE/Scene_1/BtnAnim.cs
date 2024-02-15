using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Animator anim;
    OpeningManager openingManager;
    
    private void Awake()
    {
        openingManager = FindAnyObjectByType<OpeningManager>();

        anim = GetComponent<Animator>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (openingManager.ThisSceneEnd == true) { return; }

        anim.SetTrigger("On");

        SoundManager.inst.F_Get_SoundPreFabs_PlaySFX(0, 1);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (openingManager.ThisSceneEnd == true) { return; }
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
