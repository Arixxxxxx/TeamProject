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

    // ���� ������
    WaitForSeconds ThinkTime; // ���ݱ��� ���ð�
    WaitForSeconds TiredTime; // ���ݱ��� ���ð�

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
        selectPattern = Random.Range(0, 1); // ���߿� 4�κ����������

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
                attackEnd = false;


                while (attackEnd)
                {
                    yield return null;
                }

                StartCoroutine(BossTired()); // ���ݳ� 
                
                
                break;

        }
    }

    IEnumerator CastingLightning()
    {
        for (int Index = 0; Index < SiteTrs.Length; Index++) // ��ġ����
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



        for (int i = 0; i < selectSitePos.Count; i++) // ����
        {
            GameObject obj = BossSkill_Pool.inst.F_GetSkillObj(0);
            obj.transform.position = selectSitePos[i].transform.position;
            obj.gameObject.SetActive(true);

            yield return LightingInterval;
        }
        selectSitePos.Clear(); // ������ �ʱ�ȭ
    }


    IEnumerator BossTired()
    {
        yield return null;

        Shield.SetActive(false);
        // ���ݰ��� �ִϸ��̼� ����
        yield return TiredTime;
        // ���ݰ��� �⺻ ���̵� �ִϸ��̼� ����


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
