using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    Vector3 moveVec;
    [SerializeField] float xArea;
    [SerializeField] float speed;
    void Start()
    {
        xArea = Random.Range(0f, 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        float X = Mathf.PingPong(Time.time, xArea);
        X -= xArea / 2;
        moveVec.x = X;
        transform.position += moveVec * Time.deltaTime * speed;
    }
}
