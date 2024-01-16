using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class NextMapArrow : MonoBehaviour
{
    [SerializeField] Transform[] targetList;
    [SerializeField] float enableDistance;
    float dis;
    Transform target;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {  
        if(Input.GetKeyDown(KeyCode.H)) 
        {
            target = targetList[0];
        }

        if(target == null) { return; }
        
        dis = Vector3.Distance(transform.position, target.position);

        if(dis > enableDistance)
        {
            Vector2 direction = target.position - transform.position;
            direction.Normalize();

            // 아크탄젠트를 사용하여 각도 계산
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // 계산된 각도를 설정
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
        else if(dis < enableDistance)
        {
            target = null;
            gameObject.SetActive(false);
        }
    }

    public void F_SetTarget(int value)
    {
        target = targetList[value];
    }
}
