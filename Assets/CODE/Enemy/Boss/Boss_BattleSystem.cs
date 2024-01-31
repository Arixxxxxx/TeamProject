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
            StartCoroutine(BulletAttack(0));
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
        selectPattern = Random.Range(0, 2); // ���߿� 4�κ����������

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

            case 1: // ����
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

    IEnumerator BulletAttack(int PatternNum)
    {
        yield return null;
        switch(PatternNum)
        {
            case 0: // ��ü

                for(int i = 0; i < BulletTransformRef.Length; i++)
                {
                    GameObject obj = BossSkill_Pool.inst.F_GetSkillObj(1);
                    obj.transform.position = BulletTransformRef[i].transform.position;
                    obj.transform.eulerAngles = BulletTransformRef[i].transform.eulerAngles;
                    obj.SetActive(true);

                    yield return null;
                }

                break;

            case 1: // �ѹ���

                break; 

                    case 2: // ������

                break;

            case 3: // ����

                break; ;
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
