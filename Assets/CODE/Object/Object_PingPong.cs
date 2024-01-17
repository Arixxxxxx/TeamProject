using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_PingPong : MonoBehaviour
{
    [SerializeField] float pingpongRange;
    [SerializeField] float minValue;
    [SerializeField] float maxValue;
    [SerializeField] float speed;

    void Start()
    {
        
    }
    float t;
    private void FixedUpdate()
    {
        float t = Mathf.SmoothStep(0.0f, 1.0f, Mathf.PingPong(Time.time * speed, 1.0f));

        // 부드럽게 움직이는 값을 얻어내기
        float smoothValue = Mathf.Lerp(minValue, maxValue, t);

        // 움직이는 값에 따라 위치 설정
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + smoothValue);
        transform.position = pos;
    }
    void Update()
    {
      
    }
}
