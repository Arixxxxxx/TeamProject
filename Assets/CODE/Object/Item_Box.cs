using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Box : MonoBehaviour
{
    [SerializeField] float ItemPoolingDelay;
    [SerializeField] float closeBox_Delay;
    [SerializeField] float showColorDownSpeed;
    [SerializeField] SpriteRenderer[] Shodow;
    Animator anim;
    float count;
    bool openBox;
    bool once;
   
    private void Awake()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
    }
    void Start()
    {
        
    }
   
    // Update is called once per frame
    void Update()
    {
        if (openBox)
        {
            count += Time.deltaTime;
            if(count > closeBox_Delay && !once)
            {
                once = true;
                anim.SetTrigger("Close");
            }

            if (count > closeBox_Delay + 0.5f && Shodow[0].color.a > 0)
            {
                Shodow[0].color += new Color(0, 0, 0, -0.15f) * Time.deltaTime * showColorDownSpeed;
                Shodow[1].color += new Color(0, 0, 0, -0.15f) * Time.deltaTime * showColorDownSpeed;
            }
            else if(Shodow[0].color.a == 0)
            {
                // È¸¼ö
            }
        }    
    }

    private void PoolingItem()
    {
        GameObject magnet_Obj = PoolManager.Inst.F_GetItem(0);
        magnet_Obj.transform.position = transform.position;
        magnet_Obj.gameObject.SetActive(true);

        GameObject Hp_Obj = PoolManager.Inst.F_GetItem(1);
        Hp_Obj.transform.position = transform.position;
        Hp_Obj.gameObject.SetActive(true);

    }

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !openBox)
        {
            openBox = true;
            anim.SetTrigger("Open");
            Invoke("PoolingItem", ItemPoolingDelay);
            SoundManager.inst.F_Get_ControllSoundPreFabs_ETC_PlaySFX(7, 1);
        }
    }

}
