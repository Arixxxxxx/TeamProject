using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boss_BattleSystem : MonoBehaviour
{
    [Header("# Cheak Value")]
    [Space]
    [SerializeField] bool bossBattleStart;
    [SerializeField] float thinkTime;
    [SerializeField] float tiredTime;
    GameObject Shield;

    // 공격 변수들
    WaitForSeconds ThinkTime; // 공격구상 대기시간
    WaitForSeconds TiredTime; // 공격구상 대기시간

    [Header("#Skill Interval Value")]
    [Space]
    [SerializeField] float lightingInterval;
    WaitForSeconds LightingInterval;
    GameObject[] SiteArea = new GameObject[4];
    [SerializeField] List<Transform>[] SiteTrs = new List<Transform>[4];
    

    private void Awake()
    {
     
        Shield = transform.Find("Skill/Shield").gameObject;

        if (thinkTime == 0)
        {
            thinkTime = 6;
        }

        if (tiredTime == 0)
        {
            tiredTime = 8;
        }

        ThinkTime = new WaitForSeconds(thinkTime);
        TiredTime = new WaitForSeconds(tiredTime);
        LightingInterval = new WaitForSeconds(lightingInterval);

        SiteArea[0] = transform.Find("RandomPos/A").gameObject;
        SiteArea[1] = transform.Find("RandomPos/B").gameObject;
        SiteArea[2] = transform.Find("RandomPos/C").gameObject;
        SiteArea[3] = transform.Find("RandomPos/D").gameObject;

        for (int i = 0; i < SiteArea.Length; i++)
        {
            SiteTrs[i] = new List<Transform>();

            int count = SiteArea[i].transform.childCount;
       
            for(int Index = 0;  Index < count; Index++)
            {
                SiteTrs[i].Add(SiteArea[i].transform.GetChild(Index).transform);
            }

        }


    }
    void Start()
    {

    }

    bool once;
    void Update()
    {
        returnUpdate();
        if(bossBattleStart == true && once == false)
        {
            once = true;
            StartAttack();
        }
    }



    // 활동공격 시작 함수
    private void StartAttack()
    {
        StopCoroutine(Think());
        StartCoroutine(Think());
    }

    int selectPattern;
    IEnumerator Think() // 패턴 공격 시작
    {
        Shield.gameObject.SetActive(true);
        selectPattern = Random.Range(0, 1); // 나중에 4로변경해줘야함

        yield return ThinkTime;

        StartCoroutine(CastingSkill(selectPattern));
    }

    List<Transform> selectSitePos = new List<Transform>();

    bool attackEnd;
    IEnumerator CastingSkill(int value)
    {
        yield return null;

        switch (value)
        {
            case 0: // 벼락

                attackEnd = true;
                StartCoroutine(CastingLightning());
                yield return new WaitForSeconds(4);
                StartCoroutine(CastingLightning());
                yield return new WaitForSeconds(4);
                StartCoroutine(CastingLightning());
                yield return new WaitForSeconds(1.5f);
                attackEnd = false;


                while (attackEnd)
                {
                    yield return null;
                }

                StartCoroutine(BossTired()); // 공격끝 
                
                
                break;

        }
    }

    IEnumerator CastingLightning()
    {
        for (int Index = 0; Index < SiteTrs.Length; Index++) // 위치선정
        {
            for (int i = 0; i < 5; i++)
            {
                int RandomValue = Random.Range(0, SiteTrs[Index].Count);

                while (selectSitePos.Contains(SiteTrs[Index][RandomValue]) == true)
                {
                    RandomValue = Random.Range(0, SiteTrs[Index].Count);
                    yield return null;
                }

                selectSitePos.Add(SiteTrs[Index][RandomValue]);
            }
        }



        for (int i = 0; i < selectSitePos.Count; i++) // 시전
        {
            GameObject obj = BossSkill_Pool.inst.F_GetSkillObj(0);
            obj.transform.position = selectSitePos[i].transform.position;
            obj.gameObject.SetActive(true);

            yield return LightingInterval;
        }
        selectSitePos.Clear(); // 포지션 초기화
    }


    IEnumerator BossTired()
    {
        yield return null;

        Shield.SetActive(false);
        // 공격가능 애니메이션 실행
        yield return TiredTime;
        // 공격가능 기본 아이들 애니메이션 실행


        StartCoroutine(Think()); // 최초로 돌아감
    }





    

    private void returnUpdate() //전투시작 체크 및 알림
    {
        if (bossBattleStart == true) { return; }

        else if (bossBattleStart == false)
        {
            bossBattleStart = GameManager.Inst.BossBattleStart;
        }
    }

}
