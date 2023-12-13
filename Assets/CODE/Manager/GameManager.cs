using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;

    NavMeshSurface nav_Surface;
    [SerializeField] GameObject Player;
    [SerializeField] Light2D globalLight;
    [SerializeField] GameObject playerLight;
    [SerializeField] float light_Change_Speed;

    bool isPlayer_Dead;
    public bool IsPlayer_Dead { get { return isPlayer_Dead; } set { isPlayer_Dead = value; } }

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
    }

    void Start()
    {
        NavMapBake_init();
    }

    
    void Update()
    {
        
    }


    private void NavMapBake_init()
    {
        nav_Surface = GetComponent<NavMeshSurface>();
        nav_Surface.BuildNavMeshAsync();
    }

    public Vector2 F_Get_PlayerObj()
    {
        return Player.transform.position;
    }



    /// <summary>
    /// 글로벌 라이트 조절
    /// </summary>
    /// <param name="Value"> ture 불켜기 // false 불끄기</param>
    public void F_Light_On_Off(bool Value)
    {
        StartCoroutine(Light_ON(Value));
    }

    IEnumerator Light_ON(bool Value)
    {
        playerLight.SetActive(!Value);

        switch (Value) 
        {
            case true:

                while (globalLight.intensity <= 1)
                {
                    globalLight.intensity += Time.deltaTime * light_Change_Speed;
                    yield return null;
                }
                globalLight.intensity = 1;

             

                break;

                case false:
                while (globalLight.intensity >= 0.1f)
                {
                    globalLight.intensity -= Time.deltaTime * light_Change_Speed;
                    yield return null;
                }

                globalLight.intensity = 0.1f;
                break;
        }
    }

    public Player_Stats F_Get_PlayerSc()
    {
        return Player.GetComponent< Player_Stats >();
    }
}
