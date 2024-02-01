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
    Animator anim;
    ParticleSystem ballPs;
    BossSpinLaser spinLaser_Sc;


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
    [SerializeField] Transform[] BulletTransformRef0 = new Transform[8];
    [SerializeField] Transform[] BulletTransformRef1 = new Transform[8];
    List<Transform> spinMuzzle = new List<Transform>();

    Transform Bosscenter;
  
    private void Awake()
    {
        anim = GetComponent<Animator>();
        Shield = transform.Find("Skill/Shield").gameObject;
        ballPs = transform.Find("bossball/Ps").GetComponent<ParticleSystem>();
        BulletTransformRef0 = transform.parent.Find("RandomPos/BallPos0").GetComponentsInChildren<Transform>().Skip(1).ToArray();
        BulletTransformRef1 = transform.parent.Find("RandomPos/BallPos1").GetComponentsInChildren<Transform>().Skip(1).ToArray();
        Bosscenter = transform.Find("Center").GetComponent<Transform>();
        spinLaser_Sc = transform.Find("Skill/SpinLaser").GetComponent<BossSpinLaser>();


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
        
    }


    private void togetherCastingOff()
    {
        if(togetherCasting == true && !PatternEnd3 && !PatternEnd2 && !PatternEnd1 && !PatternEnd0)
        {
            togetherCasting = false;
        }
    }

    // 활동공격 시작 함수
    private void StartAttack()
    {
        StopCoroutine(Think());
        StartCoroutine(Think());
    }

    int selectPattern;
    [SerializeField] int nextLv;
    [SerializeField] bool PatternEnd0, PatternEnd1, PatternEnd2, PatternEnd3;
    [SerializeField] bool togetherCasting;
    Coroutine[] PlaySkill = new Coroutine[4];
    IEnumerator Think() // 패턴 공격 시작
    {
        Shield.gameObject.SetActive(true);
        selectPattern = Random.Range(0, 4); // 나중에 4로변경해줘야함
     

        yield return new WaitForSeconds(1.5f);

        //if(nextLv > 3) // 한바퀴 돌게해줌
        //{
        //    nextLv = 0;
        //}

        switch (nextLv)  //selectPattern= 랜덤
        {
            case 0:
                GameUIManager.Inst.F_SetMSGUI(2, false);
                break;

            case 1:
                GameUIManager.Inst.F_SetMSGUI(3, false);
                break;

            case 2:
                GameUIManager.Inst.F_SetMSGUI(4, false);
                break;

            case 3:
                //GameUIManager.Inst.F_SetMSGUI(5, false);
                break;
        }
        anim.SetTrigger("Attack");

        yield return ThinkTime;
        
        if(nextLv < 4) //기본 패턴
        {
            spinLaser_Sc.F_SetSpeedValue(20.5f);  //기본값으로 설정
            StartCoroutine(CastingSkill(nextLv)); // 순차적
        }
        else if(nextLv == 4)
        {
            togetherCasting = true;

            PlaySkill[0] = StartCoroutine(CastingSkill(0)); // 번개
            PlaySkill[1] = StartCoroutine(CastingSkill(1)); // 바닥 // 2구슬 // 레이저
        }
        else if (nextLv == 5)
        {
            togetherCasting = true;

            spinLaser_Sc.F_SetSpeedValue(13f); // 2개 패턴이라 레이저 속도 늦춰줌
            PlaySkill[1]  = StartCoroutine(CastingSkill(1)); // 바닥
            PlaySkill[3] = StartCoroutine(CastingSkill(3)); // 레이저 
        }
        else if (nextLv == 6)
        {
            togetherCasting = true;

            PlaySkill[3] = StartCoroutine(CastingSkill(3)); // 레이저
            PlaySkill[2] = StartCoroutine(CastingSkill(2)); // 구슬
            
            nextLv = 0; // 모든패턴 다 수행시 맨처음으로 돌아감
        }


    }

    List<Transform> selectSitePos = new List<Transform>();

    [SerializeField] bool attackEnd;
    IEnumerator CastingSkill(int value)
    {
        yield return null;

        switch (value)
        {
            case 0: // 벼락
                PatternEnd0 = true;
                
                StartCoroutine(CastingLightning());
                yield return new WaitForSeconds(4);
                StartCoroutine(CastingLightning());
                yield return new WaitForSeconds(4);
                StartCoroutine(CastingLightning());
                yield return new WaitForSeconds(1.5f);



                while (PatternEnd0)
                {
                    yield return null;
                }
                
                togetherCastingOff();

                if(togetherCasting) { StopCoroutine(PlaySkill[0]); }

                while (togetherCasting)
                {
                    yield return null;
                }

                StartCoroutine(BossTired()); // 공격끝 
                anim.SetTrigger("Rest");
                ballPs.Stop();
                break;

            case 1: // 바닥
                PatternEnd1 = true;
                
                StartCoroutine(GroundPatternActionStart(2));
                yield return new WaitForSeconds(4);
                StartCoroutine(GroundPatternActionStart(1));
                yield return new WaitForSeconds(4);
                StartCoroutine(GroundPatternActionStart(2));
                yield return new WaitForSeconds(4);
                StartCoroutine(GroundPatternActionStart(1));
                yield return new WaitForSeconds(4);
                StartCoroutine(GroundPatternActionStart(1));
                

                while (PatternEnd1)
                {
                    yield return null;
                }
                
                togetherCastingOff();
                if (togetherCasting) { StopCoroutine(PlaySkill[1]); }

                while (togetherCasting)
                {
                    yield return null;
                }

                StartCoroutine(BossTired()); // 공격끝 
                anim.SetTrigger("Rest");
                ballPs.Stop();
                break;

            case 2: // 구슬공격
                PatternEnd2 = true;
                
                StartCoroutine(BulletAttack(0));
                yield return new WaitForSeconds(6f);
                StartCoroutine(BulletAttack(1));
                yield return new WaitForSeconds(7f);
                StartCoroutine(BulletAttack(2));

                while (PatternEnd2)
                {
                    yield return null;
                }
                
                togetherCastingOff();
                if (togetherCasting) { StopCoroutine(PlaySkill[2]); }

                while (togetherCasting)
                {
                    yield return null;
                }

                StartCoroutine(BossTired()); // 공격끝 
                anim.SetTrigger("Rest");
                ballPs.Stop();
                break;

            case 3: // Spin Laser Pattern
                PatternEnd3 = true;
                
                StartCoroutine(SpinLaserAction());

                yield return null;
                

                while (PatternEnd3)
                {
                    yield return null;
                }

                
                togetherCastingOff(); // 전부 다 꺼져잇는지 한번더 확인

                if (togetherCasting) { StopCoroutine(PlaySkill[3]); } // 무언가 돌고있다면 해당코루틴은 종료

                while (togetherCasting)
                {
                    yield return null;
                }

                StartCoroutine(BossTired()); // 공격끝 
                anim.SetTrigger("Rest");
                ballPs.Stop();
                break;
                
        }
    }
    
    // 3번 레이저 스킬 실행
    IEnumerator SpinLaserAction()
    {
        spinLaser_Sc.F_ActionPattern(0);
        yield return new WaitForSeconds(2f);
        spinLaser_Sc.F_ActionPattern(1);
        yield return new WaitForSeconds(3);  // 1,2번 레이저 켜줌

        float RandomValue = Random.Range(2.5f, 3.5f);
        yield return new WaitForSeconds(RandomValue); // 역회전

        spinLaser_Sc.F_PatternReverse();

        RandomValue = Random.Range(2.5f, 3.5f);
        yield return new WaitForSeconds(RandomValue); // 역회전

        spinLaser_Sc.F_PatternReverse();

        RandomValue = Random.Range(2.5f, 3.5f);
        yield return new WaitForSeconds(RandomValue); // 역회전

        spinLaser_Sc.F_PatternReverse();

        RandomValue = Random.Range(2.5f, 3.5f);
        yield return new WaitForSeconds(RandomValue); // 역회전

        spinLaser_Sc.F_PatternReverse();

        yield return new WaitForSeconds(3);  // 1,2번 레이저 켜줌
        spinLaser_Sc.F_ActionEnd();
        yield return new WaitForSeconds(1.5f);

        PatternEnd3 = false;


    }
    float randomDir;
    int patternNum;

    float rotationZ;

    // 2번 구슬공격
    IEnumerator BulletAttack(int PatternNum)
    {
        yield return null;
        switch(PatternNum)
        {
            case 0: // 전체

                int count = BulletTransformRef0.Length;
                for(int j= 0; j < 5; j++)
                {
                    for (int i = 0; i < count; i++)
                    {
                        GameObject obj = BossSkill_Pool.inst.F_GetSkillObj(1);

                        switch (patternNum) // 2개의 배열 포지션으로 발사함
                        {
                            case 0:
                                obj.transform.GetChild(0).position = BulletTransformRef0[i].transform.position;
                                obj.transform.GetChild(0).eulerAngles = BulletTransformRef0[i].transform.eulerAngles;
                                break;

                                case 1:
                                obj.transform.GetChild(0).position = BulletTransformRef1[i].transform.position;
                                obj.transform.GetChild(0).eulerAngles = BulletTransformRef1[i].transform.eulerAngles;
                                break;

                        }
                        randomDir = 0;
                        obj.SetActive(true);
                        
                        yield return null;
                    }

                    if(patternNum == 0) // 정방향 사이방향 패턴갈라주는 부분
                    {
                        patternNum++;
                    }
                    else if(patternNum == 1)
                    {
                        patternNum = 0;
                    }
                    
                    yield return new WaitForSeconds(1);
                }
                patternNum = 0;
                break;

            case 1: // 한바퀴

                for (int i = 0; i < 2; i++)
                {

                    for (int j = 0; j < 24; j++) // 360 나누기 32 
                    {
                        GameObject obj = BossSkill_Pool.inst.F_GetSkillObj(1);
                        obj.transform.GetChild(0).position = Bosscenter.transform.position;
                        obj.transform.GetChild(0).eulerAngles = new Vector3(0,0, rotationZ);
                        obj.transform.GetChild(0).Translate(Vector3.up * 4f, Space.Self);
                        obj.SetActive(true);
                        

                        rotationZ += 15;
                        yield return new WaitForSeconds(0.15f);
                    }
                    rotationZ = 0;
                    yield return new WaitForSeconds(0.1f);
                }

                break; 

            case 2: // 랜덤

                int totalCount0 = BulletTransformRef0.Length;

                for (int i = 0; i < totalCount0; i++)
                {
                    spinMuzzle.Add(BulletTransformRef0[i]);
                    spinMuzzle.Add(BulletTransformRef1[i]);
                }

                spinMuzzle.Sort((x, y) => Random.Range(-spinMuzzle.Count, spinMuzzle.Count));
                spinMuzzle.Sort((x, y) => Random.Range(-spinMuzzle.Count, spinMuzzle.Count));

                for (int i = 0; i < 6; i++)
                {

                    int bulletCount = spinMuzzle.Count;
                    spinMuzzle.Sort((x, y) => Random.Range(-spinMuzzle.Count, spinMuzzle.Count));

                    for (int j = 0; j < bulletCount; j++)
                    {
                        GameObject obj = BossSkill_Pool.inst.F_GetSkillObj(1);
                        obj.transform.GetChild(0).position = spinMuzzle[j].transform.position;
                        obj.transform.GetChild(0).eulerAngles = spinMuzzle[j].transform.eulerAngles;
                        obj.SetActive(true);
                        yield return new WaitForSeconds(0.1f);
                    }
                    yield return new WaitForSeconds(0.1f);
                }

                spinMuzzle.Clear();
                yield return new WaitForSeconds(1.5f);
                PatternEnd2 = false;


                break; 
        }
    }

    List<int> GroundPattern = new List<int>();
    List<int> deletePattern = new List<int>();


    // 1번 바닥 공격
    IEnumerator GroundPatternActionStart(int value)
    {
        castingCounting1++;

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

        if (castingCounting1 == 5)
        {
            yield return new WaitForSeconds(4);
            castingCounting1 = 0;
            PatternEnd1 = false;

        }
    }


    // 0번 번개공격
    [SerializeField] int castingCounting;
    [SerializeField] int castingCounting1;
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
            PatternEnd0 = false;

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
        nextLv++;
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
