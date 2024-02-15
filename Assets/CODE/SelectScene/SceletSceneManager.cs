using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class SceletSceneManager : MonoBehaviour
{

    SpriteRenderer[] femaleSr;
    SpriteRenderer[] maleSr;
    [SerializeField] Color nonColickspriteOffColor;
    [SerializeField] Color nonColickshaowColor;
    [SerializeField] float fadeSpeed;
    [SerializeField] Animator[] anim;
    Animator cutton;

    // 오더인레이어 셋팅
    SpriteRenderer leftBgIMG;
    SpriteRenderer rightBgIMG;
    private void Awake()
    {
        femaleSr = transform.Find("Bg/W").GetComponentsInChildren<SpriteRenderer>().Skip(1).ToArray();
        leftBgIMG = transform.Find("Bg/W").GetComponent<SpriteRenderer>();
        rightBgIMG = transform.Find("Bg/M").GetComponent<SpriteRenderer>();
        maleSr = transform.Find("Bg/M").GetComponentsInChildren<SpriteRenderer>().Skip(1).ToArray();
        cutton = transform.Find("Bg/UI/Cutton").GetComponent<Animator>();
        anim = transform.GetComponentsInChildren<Animator>();
        femaleSr[0].color = nonColickspriteOffColor;
        maleSr[0].color = nonColickspriteOffColor;
        femaleSr[1].color = nonColickshaowColor;
        maleSr[1].color = nonColickshaowColor;

        cutton.SetTrigger("Off");
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseAction(int value, bool B)
    {
        if (B == true)
        {
            anim[value].SetTrigger("On");

            if(value == 0) // 파티클 경계선 넘어가는것 방지 레이어 재정렬
            {
                leftBgIMG.sortingOrder = 1;
                rightBgIMG.sortingOrder = 3;
            }
            else if(value == 1)
            {
                leftBgIMG.sortingOrder = 3;
                rightBgIMG.sortingOrder = 1;
            }
        }
        else
        {
            anim[value].SetTrigger("Off");
        }
        
    }
    bool sceletEnd;
    public void SelectAction(int value)
    {
        if(sceletEnd== true) { return; }

        sceletEnd = true;
        StartCoroutine(NextScene(value));
    }
   IEnumerator NextScene(int value)
    {
        anim[value].SetTrigger("Select");
        yield return new WaitForSeconds(2f);
        cutton.SetTrigger("On");
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(2);
    }
    

}
