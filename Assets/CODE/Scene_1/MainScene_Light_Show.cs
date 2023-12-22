using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MainScene_Light_Show : MonoBehaviour
{
    public enum LightDir
    {
        Left,Right,Top
    }
    public LightDir type;

    Light2D light;

    [SerializeField] float zValue;
    [SerializeField] float UpDown_Time;
    [SerializeField] float Speed;

    void Start()
    {
        light = GetComponent<Light2D>();
    }

    [SerializeField] float count;
    [SerializeField] bool dirChange;
    void Update()
    {
        Valuie_RepeatChange();
        ZValue_Change();



    }

    private void ZValue_Change()
    {
        if(dirChange == false)
        {
            switch (type)
            {
                case LightDir.Left:
                    transform.eulerAngles += new Vector3(0, 0, 0.1f) * Time.deltaTime * Speed;
                    break;

                    case LightDir.Right:
                    transform.eulerAngles -= new Vector3(0, 0, 0.1f) * Time.deltaTime * Speed;
                    break;

                case LightDir.Top:
                    light.intensity += Time.deltaTime * Speed;
                    break;

            }


        }
        else
        {
            switch (type)
            {
                case LightDir.Left:
                    transform.eulerAngles -= new Vector3(0, 0, 0.1f) * Time.deltaTime * Speed; ;
                    break;

                case LightDir.Right:
                    transform.eulerAngles += new Vector3(0, 0, 0.1f) * Time.deltaTime * Speed;
                    break;

                case LightDir.Top:
                    light.intensity -= Time.deltaTime * Speed;
                    break;
            }
        }
    }



    private void Valuie_RepeatChange()
    {
        count += Time.deltaTime;
        if (count < UpDown_Time)
        {
            dirChange = false;
        }
        else if (count > UpDown_Time && count < UpDown_Time * 2)
        {
            dirChange = true;
        }
        else if (count >= UpDown_Time * 2)
        {
            count = 0;
        }
    }
}
