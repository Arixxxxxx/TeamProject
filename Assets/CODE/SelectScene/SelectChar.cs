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
        SoundManager.inst.F_Bgm_Player(0, 1);
    }
    bool cheakBool;

    private void choiseStartOk()
    {
        choiseStart = true;
    }
    private void Update()
    {
        if (realClick == false)
        {
            if (CheakMouse == 1 && choiseEnd == false && cheakBool == false)
            {
                cheakBool = true;
                sm.OnMouseAction((int)Character, true);
                ps.gameObject.SetActive(true);
                SoundManager.inst.F_Get_SoundPreFabs_PlaySFX(0);

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
        if (CheakMouse != 1)
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
        if (choiseStart == false) { return; }

        realClick = true;
        DataManager.inst.CurrentCharacter = Character;
        sm.SelectAction((int)Character);
        StartCoroutine(PlaySFX());
    }

    IEnumerator PlaySFX()
    {
        SoundManager.inst.F_Get_SoundPreFabs_PlaySFX(3);

        yield return new WaitForSeconds(1f);
        switch ((int)Character)
        {
            case 0:
                SoundManager.inst.F_Get_SoundPreFabs_PlaySFX(1);
                break;
            case 1:
                SoundManager.inst.F_Get_SoundPreFabs_PlaySFX(2);
                break;
        }
    }
}
