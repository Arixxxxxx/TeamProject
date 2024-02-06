using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SelectChar : MonoBehaviour
{

    
    public CharacterNum Character;
    SceletSceneManager sm;
    [SerializeField] int CheakMouse;
    bool choiseStart;
    bool choiseEnd;
    
    ParticleSystem ps;
    private void Awake()
    {
        sm = transform.GetComponentInParent<SceletSceneManager>();
        ps = transform.parent.Find("Ps").GetComponent<ParticleSystem>();

    }
    void Start()
    {
        Invoke("choiseStartOk", 2f);
    }
    bool cheakBool;

    private void choiseStartOk()
    {
        choiseStart = true;
    }
    private void Update()
    {
        if(realClick == false)
        {
            if (CheakMouse == 1 && choiseEnd == false && cheakBool == false)
            {
                cheakBool = true;
                sm.OnMouseAction((int)Character, true);
                ps.gameObject.SetActive(true);
            }
            else if (CheakMouse == 0 && choiseEnd == false && cheakBool == true)
            {
                cheakBool = false;
                sm.OnMouseAction((int)Character, false);
                ps.gameObject.SetActive(false);
            }
        }
        
    }


    private void OnMouseEnter()
    {
        CheakMouse = 1;
        
    }
    private void OnMouseOver()
    {
        if(CheakMouse != 1)
        {
            CheakMouse = 1;
        }

    }
    private void OnMouseExit()
    {
        CheakMouse = 0;
    }
    bool realClick;
    private void OnMouseUpAsButton()
    {
        if(choiseStart == false) { return; }

        realClick = true;
        DataManager.inst.CurrentCharacter = Character;
        sm.SelectAction((int)Character);

    }
}
