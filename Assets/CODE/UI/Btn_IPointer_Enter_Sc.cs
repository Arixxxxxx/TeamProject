using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Btn_IPointer_Enter_Sc : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    Image Light;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(Light.gameObject.activeSelf == false)
        {
            Light.gameObject.SetActive(true);  
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
        Light = transform.parent.Find("Light2").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
