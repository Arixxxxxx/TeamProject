using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Btn_IPointer_Enter_Sc : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum BtnType
    {
        SkillBtn, SelectWindow
    }
    public BtnType type;

    Button Btn;
    Image Light;
    private void Awake()
    {
        Btn = GetComponent<Button>();
    }
    public bool noSfx;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!noSfx)
        {
            SoundManager.inst.F_Get_ControllSoundPreFabs_ETC_PlaySFX(9, 1);
        }
        

        switch (type)
        {
            case BtnType.SkillBtn:

                if (Light.gameObject.activeSelf == false && Btn.interactable == true)
                {
                    
                    Light.gameObject.SetActive(true);
                }

                break;

            case BtnType.SelectWindow:
                if (Light.gameObject.activeSelf == false)
                {
                    Light.transform.position = Btn.transform.position;
                    Light.gameObject.SetActive(true);
                }
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Light.gameObject.activeSelf == true)
        {
            Light.gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        switch (type) 
        {
          case BtnType.SkillBtn:
                Light = transform.parent.Find("Light2").GetComponent<Image>();
                break;

                case BtnType.SelectWindow:
                Light = transform.parent.Find("Light2").GetComponent<Image>();
                break;
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (type)
        {
            case BtnType.SkillBtn:

                if (Btn.interactable == false)
                {
                    Light.gameObject.SetActive(false);
                }

                break;

            case BtnType.SelectWindow:
              
                break;
        }
       
    }
}
