using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutton_Controller : MonoBehaviour
{
    public static Cutton_Controller inst;

    [Header("# Insert Cutton UI")]
    [Space]
    [SerializeField]  Animator anim;
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
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
