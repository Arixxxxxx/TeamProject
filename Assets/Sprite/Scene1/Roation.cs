using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Roation : MonoBehaviour
{

    RectTransform trs;
    [SerializeField] Vector3 Move;
    [SerializeField] float CludeSpeed;

    public enum UiType
    {
        Text, Cloude
    }

    public UiType type;
    // Start is called before the first frame update
    void Start()
    {
       
       
    }

    bool moveRight;
    [SerializeField] float count;
    [SerializeField] float A;
    // Update is called once per frame
    void Update()
    {
        if(type == UiType.Text)
        {
            transform.Rotate(Vector3.back * Time.deltaTime * 4);
        }
        else if(type == UiType.Cloude)
        {
            if (moveRight)
            {
                A = Mathf.Lerp(A, 0.2f, Time.deltaTime *0.2f);
                transform.position += new Vector3 (A, 0, 0);
            }
            else
            {
                A = Mathf.Lerp(A, -0.2f, Time.deltaTime * 0.2f);
                transform.position += new Vector3(A, 0, 0);
            }

            count += Time.deltaTime;
            if(count > 0 && count < 1)
            {
                moveRight = true;
            }
            else if(count > 1 && count < 2)
            {
                moveRight = false;
            }
            else if(count > 2)
            {
                count = 0;
            }
        }
    }
}

