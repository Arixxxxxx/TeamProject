using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dmg_Object : MonoBehaviour
{
    [Header(" # Input Object DMG !! ")]
    [SerializeField] float DMG;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision.GetComponent<EnemyStats>() != null)
        {
            collision.GetComponent<EnemyStats>().F_Enemy_On_Hit(DMG);
        }
    }
}
