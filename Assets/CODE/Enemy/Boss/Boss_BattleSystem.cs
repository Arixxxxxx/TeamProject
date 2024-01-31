using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Boss_BattleSystem : MonoBehaviour
{
    [Header("# Cheak Value")]
    [Space]
    [SerializeField] bool bossBattleStart;
    [SerializeField] float thinkTime;
    [SerializeField] float tiredTime;
    GameObject Shield;
    Animator anim;
    ParticleSystem ballPs;

    // 공격 변수들
    WaitForSeconds ThinkTime; // 공격구상 대기시간
    WaitForSeconds TiredTime; // 공격구상 대기시간

    [Header("#Skill Interval Value")]
    [Space]
    [SerializeField] float lightingInterval;
    [SerializeField][Tooltip("구역별로 소환할 번개 갯수")] int AreaAttackCycleValue; // 구역별로 번개 소환할 갯수 [ ex : a구역 n회]

    WaitForSeconds LightingInterval;
    GameObject[] SiteArea = new GameObject[4];
    [SerializeField] List<Transform>[] SiteTrs = new List<Transform>[4];
    BossSkill_GroundPattern[] groundPatternAr = new BossSkill_GroundPattern[6];

    //[Header("# 추후에 인스펙터 지워야할 변수들")]
    //[Space]
    List<Vector3> BulletStartPosList = new List<Vector3>();
    List<Vector3> BulletStartRotList = new List<Vector3>();
    [SerializeField] Transform[] BulletTransformRef = new Transform[8];


    private void Awake()
    {
        anim = GetComponent<Animator>();
        Shield = transform.Find("Skill/Shield").gameObject;
        ballPs = transform.Find("bossball/Ps").GetComponent<ParticleSystem>();
        BulletTransformRef = transform.parent.Find("RandomPos/BallPos").GetComponentsInChildren<Transform>().Skip(1).ToArray();

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

        SiteArea[0] = transform.parent.Find("RandomPos/A").gameObject;
        SiteArea[1] = transform.parent.Find("RandomPos/B").gameObject;
        SiteArea[2] = transform.parent.Find("RandomPos/C").gameObject;
        SiteArea[3] = transform.parent.Find("RandomPos/D").gameObject;

        for (int i = 0; i < SiteArea.Length; i++) // ABCD / 4구역 초기화
        {
            SiteTrs[i] = new List<Transform>();

            int count = SiteArea[i].transform.childCount; // 스킬 구역 트랜스 갯수 초기화

            for (int Index = 0; Index < count; Index++)
            {
                SiteTrs[i].Add(SiteArea[i].transform.GetChild(Index).transform); // 각구역별 리스트 배열에 모든 값 저장
            }

        }

        for (int i = 0; i < groundPatternAr.Length; i++)  // 그라운드 스킬 패턴 바닥 스크립트 순서대로 가져옴
        {
            groundPatternAr[i] = transform.Find("Skill/GroundPattern").GetChild(i).GetComponent<BossSkill_GroundPattern>();
        }



    }
    void Start()
    {

    }

    bool once;
    void Update()
    {
        returnUpdate();
        if (bossBattleStart == true && once == false)
        {
            once = true;
            StartAttack();
        }


        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(BulletAttack(0));
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
        selectPattern = Random.Range(0, 2); // 나중에 4로변경해줘야함

        yield return ThinkTime;

        anim.SetTrigger("Attack");

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



                while (attackEnd)
                {
                    yield return null;
                }

                StartCoroutine(BossTired()); // 공격끝 
                anim.SetTrigger("Rest");
                ballPs.Stop();
                break;

            case 1: // 벼락
                attackEnd = true;
                StartCoroutine(GroundPatternActionStart(2));
                yield return new WaitForSeconds(4);
                StartCoroutine(GroundPatternActionStart(1));
                yield return new WaitForSeconds(4);
                StartCoroutine(GroundPatternActionStart(2));
                yield return new WaitForSeconds(4);
                StartCoroutine(GroundPatternActionStart(1));
                yield return new WaitForSeconds(4);
                StartCoroutine(GroundPatternActionStart(1));
                

                while (attackEnd)
                {
                    yield return null;
                }

                StartCoroutine(BossTired()); // 공격끝 
                anim.SetTrigger("Rest");
                ballPs.Stop();
                break;

            case 2: // 구슬공격
                attackEnd = true;
                StartCoroutine(BulletAttack(0)); 
                


                while (attackEnd)
                {
                    yield return null;
                }

                StartCoroutine(BossTired()); // 공격끝 
                anim.SetTrigger("Rest");
                ballPs.Stop();
                break;

        }
    }

    IEnumerator BulletAttack(int PatternNum)
    {
        yield return null;
        switch(PatternNum)
        {
            case 0: // 전체

                for(int i = 0; i < BulletTransformRef.Length; i++)
                {
                    GameObject obj = BossSkill_Pool.inst.F_GetSkillObj(1);
                    obj.transform.position = BulletTransformRef[i].transform.position;
                    obj.transform.eulerAngles = BulletTransformRef[i].transform.eulerAngles;
                    obj.SetActive(true);

                    yield return null;
                }

                break;

            case 1: // 한바퀴

                break; 

                    case 2: // 역바퀴

                break;

            case 3: // 랜덤

                break; ;
        }
    }

    List<int> GroundPattern = new List<int>();
    List<int> deletePattern = new List<int>();
    IEnumerator GroundPatternActionStart(int value)
    {
        castingCounting++;

        for (int i = 0; i < groundPatternAr.Length; i++) // 공격패턴가능한 숫자 만들기
        {
            GroundPattern.Add(i);
        }

        int RandomDeleteValue = Random.Range(0, groundPatternAr.Length); // 제외시킬 번호 생성
        int RandomDeleteValue1 = Random.Range(0, groundPatternAr.Length); // 생성

        while (RandomDeleteValue == RandomDeleteValue1) // 두번째 값이 1번과 같으면 다시만듬
        {
            RandomDeleteValue1 = Random.Range(0, groundPatternAr.Length);
            yield return null;
        }

        GroundPattern.Remove(RandomDeleteValue); // 한개만 지우는거면 한개만 지움

        if(value == 2) // 2개면 2개지움
        {
            GroundPattern.Remove(RandomDeleteValue1);
        }

        //시전

        for (int i = 0; i < groundPatternAr.Length; i++)
        {
            if (GroundPattern.Contains(i) == true) // 예외시킨 번호가 
            {
                groundPatternAr[i].F_StartAction();
            }
        }

        GroundPattern.Clear();

        if (castingCounting == 5)
        {
            yield return new WaitForSeconds(4);
            castingCounting = 0;
            attackEnd = false;
        }
    }

    [SerializeField] int castingCounting;
    IEnumerator CastingLightning()
    {
        castingCounting++;

        for (int Index = 0; Index < SiteTrs.Length; Index++) // A,B,C,D 구역 [4회 만큼]  초기화
        {

            for (int i = 0; i < AreaAttackCycleValue; i++) // 지정한 갯수만큼 구역별로
            {
                int RandomValue = Random.Range(0, SiteTrs[Index].Count); //구역번호에서 지정한 갯수만큼 랜덤포인트 뺌

                while (selectSitePos.Contains(SiteTrs[Index][RandomValue]) == true) // 이미 들어잇는 번호면 다시
                {
                    RandomValue = Random.Range(0, SiteTrs[Index].Count);
                    yield return null;
                }

                selectSitePos.Add(SiteTrs[Index][RandomValue]); // 값을 넣어줌
            }
        }

        selectSitePos.Sort((x, y) => Random.Range(-1, 2)); // 안에 있는 패섞어줌

        for (int i = 0; i < selectSitePos.Count; i++) // 총 입력된 갯수 만큼 시전
        {
            GameObject obj = BossSkill_Pool.inst.F_GetSkillObj(0);
            obj.transform.position = selectSitePos[i].transform.position;
            obj.gameObject.SetActive(true);

            yield return LightingInterval;
        }


        selectSitePos.Clear(); // 포지션 초기화

        if (castingCounting == 3) // 횟수 초기화
        {
            yield return new WaitForSeconds(2.5f);
            attackEnd = false;
            castingCounting = 0;
        }
    }


    IEnumerator BossTired()
    {
        yield return null;

        Shield.SetActive(false);
        // 공격가능 애니메이션 실행
        yield return TiredTime;
        // 공격가능 기본 아이들 애니메이션 실행

        anim.SetTrigger("Idle");
        ballPs.Play();
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
