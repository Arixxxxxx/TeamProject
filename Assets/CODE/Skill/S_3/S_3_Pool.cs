using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_3_Pool : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void A_ReturnObj()
    {
        PoolManager.Inst.F_Return_PlayerBullet(gameObject, 3);
    }
}
