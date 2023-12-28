using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;

    NavMeshSurface nav_Surface;
    [Header("# Insert Object in Hiearachy")]
    [Space]
    [SerializeField] GameObject Player;
    [SerializeField] Light2D globalLight;
    [SerializeField] GameObject playerLight;
    [SerializeField] float light_Change_Speed;
    [SerializeField] GameObject[] GameStop_Ui_Window;
    
    bool isPlayer_Dead;

    [Header("# Ingame Cheak & Test Value")]
    [Space]
    [Space]
    [SerializeField] bool mainGameStart;
    [Space]
    [SerializeField] bool uiOpen_EveryObecjtStop;
    [Space] int plyaer_Area_Value;

    [Header("# Battle Count")]
    [Space]
    [SerializeField] int killCount;
    public bool MainGameStart { get { return mainGameStart; } set { mainGameStart = value; } }
    public bool UiOpen_EveryObecjtStop { get { return uiOpen_EveryObecjtStop; } set { uiOpen_EveryObecjtStop = value; } }
    public bool IsPlayer_Dead { get { return isPlayer_Dead; } set { isPlayer_Dead = value; } }
    public int KillCount { get { return killCount; } }

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
        uiOpen_EveryObecjtStopFuntion();
        UiOpen_Cheaker();
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

    public float F_Get_PlyerPos(Vector3 InputPos)
    {
        float dis;
        dis = Vector3.Distance(Player.transform.position, InputPos);
        return dis;
    }

    /// <summary>
    /// 플레이어와 에너미 위치 X값 계산
    /// </summary>
    /// <param name="InputPos"></param>
    /// <returns></returns>

    public float F_Get_Filpx_Value(Vector3 InputPos)
    {
        float dis;
        dis = InputPos.x - Player.transform.position.x;
        return dis;
    }

    /// <summary>
    /// 투사체 플레이어방향 계산
    /// </summary>
    /// <param name="StartPos"></param>
    /// <returns></returns>
    public Vector2 F_Enemy_BulletTargetPos(Vector3 StartPos)
    {
        Vector2 Target = Player.transform.position - StartPos;
        return Target;
    }

    /// <summary>
    /// 투사체 각도계산
    /// </summary>
    /// <param name="StartPos"></param>
    /// <returns></returns>
    public float F_EnemyBulletRotation(Vector3 StartPos)
    {
        Vector3 Target = Player.transform.position - StartPos;
        float angle = Mathf.Atan2(Target.y, Target.x) * Mathf.Rad2Deg;
        return angle;
    }

    public void F_KillCountUp()
    {
        killCount++;
    }

    private void uiOpen_EveryObecjtStopFuntion()
    {
        if(uiOpen_EveryObecjtStop == true && Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else if (uiOpen_EveryObecjtStop == false && Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    private void UiOpen_Cheaker()
    {
        if (GameStop_Ui_Window[0].gameObject.activeSelf == true && uiOpen_EveryObecjtStop == false)
         {
            uiOpen_EveryObecjtStop = true;
        }
        else if(GameStop_Ui_Window[0].gameObject.activeSelf == false && uiOpen_EveryObecjtStop == true)
        {
            uiOpen_EveryObecjtStop = false;
        }
        if (GameStop_Ui_Window[1].gameObject.activeSelf == true && uiOpen_EveryObecjtStop == false)
        {
            uiOpen_EveryObecjtStop = true;
        }
        else if (GameStop_Ui_Window[1].gameObject.activeSelf == false && uiOpen_EveryObecjtStop == true)
        {
            uiOpen_EveryObecjtStop = false;
        }
    }
  
  


}
