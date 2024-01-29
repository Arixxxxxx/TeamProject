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
    /// 조명 오르 내리기 (매개변수 = 스테이지 번호 )
    /// </summary>
    /// <param name="StageNum">3 통로 / 4 보스방</param>
    /// <param name="SwitchValue"></param>
    public void F_LightControl(int StageNum)
    {
        float CheckCurrentLightValue = globalLight.intensity; // 현재 조명값가져옴
        bool lightSwitch = false; // 불리언 변수로 값을 올려야 하는지 내려야하는지 확인

        if(CheckCurrentLightValue < stageLightValue[StageNum])  //현재 조명값과 플레이어가 바꾸려는 조명위치 비교
        {
            lightSwitch = true;
        }

        StartCoroutine(SetLight(stageLightValue[StageNum], lightSwitch)); 

    }

    IEnumerator SetLight(float value, bool Bvalue)
    {
        if(Bvalue == true) // 올려야한다면
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
