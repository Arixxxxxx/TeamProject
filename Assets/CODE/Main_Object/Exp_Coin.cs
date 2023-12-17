using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exp_Coin : MonoBehaviour
{
     SpriteRenderer sr;
    [SerializeField] Sprite[] CoinImage;
    [Header("# 스몰, 미디움, 라지 순으로 경험치 입력")]
    [SerializeField] float[] Exp_Value;

    [Header("# Drop Exp (Cheak!!)")]
    [SerializeField] float Exp;
    void Start()
    {
        
    }

    /// <summary>
    /// 코인생성시 여러가지 타입 셋팅
    /// </summary>
    /// <param name="value"> 0 스몰 / 1미디움 / 2라지</param>
    public void F_SettingCoin(int value)
    {
        if(sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }

        sr.sprite = CoinImage[value];
        Exp = Exp_Value[value];
    }
    

    bool once;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && once == false && collision.GetComponent<Player_Stats>() != null)
        {
            collision.GetComponent<Player_Stats>().F_GetExp_LevelupSystem(Exp);
            PoolManager.Inst.F_ReturnObj(gameObject, 1);
        }
    }
}
