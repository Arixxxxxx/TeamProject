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

        // �ε巴�� �����̴� ���� ����
        float smoothValue = Mathf.Lerp(minValue, maxValue, t);

        // �����̴� ���� ���� ��ġ ����
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + smoothValue);
        transform.position = pos;
    }
    void Update()
    {
      
    }
}
