using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlobalLightController : MonoBehaviour
{
    public static GlobalLightController Inst;

     Light2D globalLight;

    [Header("# Stage LightValue")]
    [Space]
    [SerializeField] float[] stageLightValue;
    private void Awake()
    {
        if(Inst == null)
        {
            Inst = this;
        }
        else
        {
            Destroy(this);
        }

        
        globalLight = GameObject.Find("----  [ Envio ]/Global Light").GetComponent<Light2D>();
    }
    void Start()
    {
        
    }

    
   
    /// <summary>
    /// ���� ���� ������ (�Ű����� = �������� ��ȣ )
    /// </summary>
    /// <param name="StageNum">3 ��� / 4 ������</param>
    /// <param name="SwitchValue"></param>
    public void F_LightControl(int StageNum)
    {
        float CheckCurrentLightValue = globalLight.intensity; // ���� ����������
        bool lightSwitch = false; // �Ҹ��� ������ ���� �÷��� �ϴ��� �������ϴ��� Ȯ��

        if(CheckCurrentLightValue < stageLightValue[StageNum])  //���� ������ �÷��̾ �ٲٷ��� ������ġ ��
        {
            lightSwitch = true;
        }

        StartCoroutine(SetLight(stageLightValue[StageNum], lightSwitch)); 

    }

    IEnumerator SetLight(float value, bool Bvalue)
    {
        if(Bvalue == true) // �÷����Ѵٸ�
        {
            while(globalLight.intensity < value)
            {
                globalLight.intensity += Time.deltaTime;
                yield return null;  
            }
        }
        else
        {
            while (globalLight.intensity > value)
            {
                globalLight.intensity -= Time.deltaTime;
                yield return null;
            }
        }

    }
}
