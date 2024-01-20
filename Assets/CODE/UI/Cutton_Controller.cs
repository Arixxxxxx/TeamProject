using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cutton_Controller : MonoBehaviour
{
    public static Cutton_Controller inst;

    [Header("# Insert Cutton UI")]
    [Space]
    [SerializeField]  Animator anim;
    Image bgColor;
    WaitForSeconds fadeoff;
    private void Awake()
    {
        if(inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }

        bgColor = anim.transform.GetComponent<Image>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 알파값 1 - 0 함수
    /// </summary>
    public void F_FadeAlpha10CuttonActive(float value)
    {
        StartCoroutine(fadeOff(value));
    }
    IEnumerator fadeOff(float value)
    {
        bgColor.color = Color.white;

        yield return new WaitForSeconds(value);

        anim.SetTrigger("Off");
    }


    /// <summary>
    /// 알파값 0~1 -> 1 ~0 함수
    /// </summary>
    /// <param name="value"></param>
    public void F_FadeCuttonActive(float value)
    {
        StopCoroutine(fadeoffActive(value));
        StartCoroutine(fadeoffActive(value));

    }

    IEnumerator fadeoffActive(float value)
    {
        anim.SetTrigger("On");

        fadeoff = new WaitForSeconds(value);

        yield return fadeoff;

        anim.SetTrigger("Off");
    }
}
