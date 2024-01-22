using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    ParticleSystem leafPs;
    Animator anim;

    void Start()
    {
        leafPs = transform.parent.Find("Leaf").GetComponent<ParticleSystem>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    [SerializeField] bool waitPs;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.transform.position.y >= transform.position.y + 1.5f && !waitPs)
        {
            waitPs = true;
            anim.SetTrigger("Touch");
            Invoke("LeafPs", 0.2f);
        }

        if (collision.CompareTag("Enemy") && collision.transform.position.y >= transform.position.y + 1.5f && !waitPs)
        {
            waitPs = true;
            anim.SetTrigger("Touch");
            Invoke("LeafPs", 0.05f);
        }
    }

    private void LeafPs()
    {
        leafPs.Play();
    }

    public void A_waitPsFals()
    {
        waitPs = false;
    }
}


