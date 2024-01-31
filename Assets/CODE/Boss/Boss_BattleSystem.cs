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

    // ���� ������
    WaitForSeconds ThinkTime; // ���ݱ��� ���ð�
    WaitForSeconds TiredTime; // ���ݱ��� ���ð�

    [Header("#Skill Interval Value")]
    [Space]
    [SerializeField] float lightingInterval;
    [SerializeField][Tooltip("�������� ��ȯ�� ���� ����")] int AreaAttackCycleValue; // �������� ���� ��ȯ�� ���� [ ex : a���� nȸ]

    WaitForSeconds LightingInterval;
    GameObject[] SiteArea = new GameObject[4];
    [SerializeField] List<Transform>[] SiteTrs = new List<Transform>[4];
    BossSkill_GroundPattern[] groundPatternAr = new BossSkill_GroundPattern[6];

    //[Header("# ���Ŀ� �ν����� �������� ������")]
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

        for (int i = 0; i < SiteArea.Length; i++) // ABCD / 4���� �ʱ�ȭ
        {
            SiteTrs[i] = new List<Transform>();

            int count = SiteArea[i].transform.childCount; // ��ų ���� Ʈ���� ���� �ʱ�ȭ

            for (int Index = 0; Index < count; Index++)
            {
                SiteTrs[i].Add(SiteArea[i].transform.GetChild(Index).transform); // �������� ����Ʈ �迭�� ��� �� ����
            }

        }

        for (int i = 0; i < groundPatternAr.Length; i++)  // �׶��� ��ų ���� �ٴ� ��ũ��Ʈ ������� ������
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
            StartCoroutine(CastingSkill(2));
        }

       
    }



    // Ȱ������ ���� �Լ�
    private void StartAttack()
    {
        StopCoroutine(Think());
        StartCoroutine(Think());
    }

    int selectPattern; 
    IEnumerator Think() // ���� ���� ����
    {
        Shield.gameObject.SetActive(true);
        selectPattern = Random.Range(0, 3); // ���߿� 4�κ����������

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
            case 0: // ����

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

                StartCoroutine(BossTired()); // ���ݳ� 
                anim.SetTrigger("Rest");
                ballPs.Stop();
                break;

            case 1: // �ٴ�
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

                StartCoroutine(BossTired()); // ���ݳ� 
                anim.SetTrigger("Rest");
                ballPs.Stop();
                break;

            case 2: // ��������
                attackEnd = true;
                StartCoroutine(BulletAttack(0));
                yield return new WaitForSeconds(6f);
                StartCoroutine(BulletAttack(1));
                yield return new WaitForSeconds(7f);
                StartCoroutine(BulletAttack(2));

                while (attackEnd)
                {
                    yield return null;
                }

                StartCoroutine(BossTired()); // ���ݳ� 
                anim.SetTrigger("Rest");
                ballPs.Stop();
                break;

        }
    }

    float randomDir;
    int patternNum;

    float rotationZ;
    IEnumerator BulletAttack(int PatternNum)
    {
        yield return null;
        switch(PatternNum)
        {
            case 0: // ��ü

                int count = BulletTransformRef0.Length;
                for(int j= 0; j < 5; j++)
                {
                    for (int i = 0; i < count; i++)
                    {
                        GameObject obj = BossSkill_Pool.inst.F_GetSkillObj(1);

                        switch (patternNum) // 2���� �迭 ���������� �߻���
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

                    if(patternNum == 0) // ������ ���̹��� ���ϰ����ִ� �κ�
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

            case 1: // �ѹ���

                for (int i = 0; i < 2; i++)
                {

                    for (int j = 0; j < 24; j++) // 360 ������ 32 
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



                //int totalCount = BulletTransformRef0.Length;

                //for (int i = 0; i < totalCount; i++)
                //{
                //    spinMuzzle.Add(BulletTransformRef0[i]);
                //    spinMuzzle.Add(BulletTransformRef1[i]);
                //}

                //for (int i = 0; i < 6; i++)
                //{
                //    int bulletCount = spinMuzzle.Count;

                //    for (int j = 0; j < bulletCount; j++)
                //    {
                //        GameObject obj = BossSkill_Pool.inst.F_GetSkillObj(1);
                //        obj.transform.GetChild(0).position = spinMuzzle[j].transform.position;
                //        obj.transform.GetChild(0).eulerAngles = spinMuzzle[j].transform.eulerAngles;
                //        obj.SetActive(true);
                //        yield return new WaitForSeconds(0.1f);
                //    }
                //    yield return new WaitForSeconds(0.1f);
                //}

                //spinMuzzle.Clear();

                break; 




            case 2: // ����

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
                attackEnd = false;
                break; 
        }
    }

    List<int> GroundPattern = new List<int>();
    List<int> deletePattern = new List<int>();
    IEnumerator GroundPatternActionStart(int value)
    {
        castingCounting++;

        for (int i = 0; i < groundPatternAr.Length; i++) // �������ϰ����� ���� �����
        {
            GroundPattern.Add(i);
        }

        int RandomDeleteValue = Random.Range(0, groundPatternAr.Length); // ���ܽ�ų ��ȣ ����
        int RandomDeleteValue1 = Random.Range(0, groundPatternAr.Length); // ����

        while (RandomDeleteValue == RandomDeleteValue1) // �ι�° ���� 1���� ������ �ٽø���
        {
            RandomDeleteValue1 = Random.Range(0, groundPatternAr.Length);
            yield return null;
        }

        GroundPattern.Remove(RandomDeleteValue); // �Ѱ��� ����°Ÿ� �Ѱ��� ����

        if(value == 2) // 2���� 2������
        {
            GroundPattern.Remove(RandomDeleteValue1);
        }

        //����

        for (int i = 0; i < groundPatternAr.Length; i++)
        {
            if (GroundPattern.Contains(i) == true) // ���ܽ�Ų ��ȣ�� 
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


        for (int Index = 0; Index < SiteTrs.Length; Index++) // A,B,C,D ���� [4ȸ ��ŭ]  �ʱ�ȭ
        {

            for (int i = 0; i < AreaAttackCycleValue; i++) // ������ ������ŭ ��������
            {
                int RandomValue = Random.Range(0, SiteTrs[Index].Count); //������ȣ���� ������ ������ŭ ��������Ʈ ��

                while (selectSitePos.Contains(SiteTrs[Index][RandomValue]) == true) // �̹� ����մ� ��ȣ�� �ٽ�
                {
                    RandomValue = Random.Range(0, SiteTrs[Index].Count);
                    yield return null;
                }

                selectSitePos.Add(SiteTrs[Index][RandomValue]); // ���� �־���
            }
        }

        selectSitePos.Sort((x, y) => Random.Range(-1, 2)); // �ȿ� �ִ� �м�����

        for (int i = 0; i < selectSitePos.Count; i++) // �� �Էµ� ���� ��ŭ ����
        {
            GameObject obj = BossSkill_Pool.inst.F_GetSkillObj(0);
            obj.transform.position = selectSitePos[i].transform.position;
            obj.gameObject.SetActive(true);

            yield return LightingInterval;
        }


        selectSitePos.Clear(); // ������ �ʱ�ȭ

        if (castingCounting == 3) // Ƚ�� �ʱ�ȭ
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
        // ���ݰ��� �ִϸ��̼� ����
        yield return TiredTime;
        // ���ݰ��� �⺻ ���̵� �ִϸ��̼� ����

        anim.SetTrigger("Idle");
        ballPs.Play();
        StartCoroutine(Think()); // ���ʷ� ���ư�
    }







    private void returnUpdate() //�������� üũ �� �˸�
    {
        if (bossBattleStart == true) { return; }

        else if (bossBattleStart == false)
        {
            bossBattleStart = GameManager.Inst.BossBattleStart;
        }
    }

}
